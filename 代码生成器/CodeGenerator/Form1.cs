using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 属性
        private List<DbTable> tables = new List<DbTable>();
        private DbTable currentTable = new DbTable();
        public List<Host> hosts = new List<Host>();
        private Host config = new Host();
        #endregion

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            var win = new ConnectForm();
            win.ShowDialog(this);
        }

        #region 获取连接
        public void LinkDb(string connString, Host host)
        {
            if (connString.Length > 0)
            {
                GetHosts();
                comboBox1.SelectedValue = host.Name;
                GetTables(connString);
                BindDtTree();
            }
        }

        protected void BindDtTree()
        {
            tvHost.Nodes.Clear();

            TreeNode tableNode = new TreeNode();
            tableNode.Text = "数据表";
            tableNode.Name = "table";

            TreeNode viewNode = new TreeNode();
            viewNode.Text = "视图";
            viewNode.Name = "view";

            foreach (var table in tables)
            {
                TreeNode node = new TreeNode();
                node.Text = table.name;

                if (table.type == "VIEW")
                {
                    viewNode.Nodes.Add(node);
                }
                else
                {
                    tableNode.Nodes.Add(node);
                }
            }
            tvHost.Nodes.AddRange(new TreeNode[] { tableNode, viewNode });
            tableNode.Expand();
            viewNode.Expand();
        }

        private void GetTables(string connString)
        {
            tables = new List<DbTable>();
            using (SqlConnection cn = new SqlConnection(connString))
            {
                cn.Open();
                DataTable dt = cn.GetSchema("Tables");
                DataView dv = new DataView(dt);
                dv.Sort = "TABLE_NAME";

                foreach (DataRowView dr in dv)
                {
                    DbTable table = new DbTable();
                    table.name = dr["TABLE_NAME"].ToString();
                    table.type = dr["TABLE_TYPE"].ToString();
                    table.columns = new List<DbTableColumn>();

                    tables.Add(table);
                }
                cn.Close();

                foreach (var table in tables)
                {
                    GetColumns(cn, table);
                }
            }
        }

        private void GetColumns(SqlConnection cn, DbTable dt)
        {
            string sql = @"select sys.columns.name as ColumnName, sys.types.name as DataType, sys.columns.max_length as ColumnSize, sys.columns.is_nullable as AllowDBNull, 
  (select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as IsKey ,
  (select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as Description
  from sys.columns, sys.tables, sys.types where sys.types.name<>'sysname' and sys.columns.object_id = sys.tables.object_id and sys.columns.system_type_id=sys.types.system_type_id and sys.tables.name='" + dt.name + "' order by sys.columns.column_id";
            SqlCommand cmd = new SqlCommand(sql, cn);
            DataSet ds = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter(sql, cn);
            sda.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DbTableColumn col = new DbTableColumn();
                col.name = dr["ColumnName"].ToString();
                col.type = dr["DataType"].ToString();
                col.size = Int32.Parse(dr["ColumnSize"].ToString());
                col.isNullAble = dr["AllowDBNull"].ToString();
                col.isKey = dr["IsKey"].ToString();
                col.description = dr["Description"].ToString();

                dt.columns.Add(col);
            }
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            GetHosts();
            if (!string.IsNullOrWhiteSpace(config.NameSpace))
            {
                tbNameSpace.Text = config.NameSpace;
            }
        }

        private void GetHosts()
        {
            hosts = new List<Host>();
            hosts = (List<Host>)SerializerHelper.LoadFromXml(Environment.CurrentDirectory + "/hosts.xml", typeof(List<Host>));
            comboBox1.DataSource = hosts;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Name";
            comboBox1.SelectedIndex = -1;

        }

        private void tvHost_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string tn = e.Node.Text;
            var table = tables.Where(t => t.name == tn).SingleOrDefault();
            if (table != null)
            {
                currentTable = table;

                dgvTable.DataSource = table.columns;

                //lbSearch.Items.Clear();
                
                var cols1 = new List<DbTableColumn>();
                table.columns.ForEach((i) => cols1.Add(i));

                lbSearch.DataSource = cols1;
                lbSearch.DisplayMember = "showName";
                lbSearch.ValueMember = "name";
                lbSearch.SelectedIndex = -1;

                var cols2 = new List<DbTableColumn>();
                table.columns.ForEach((i) => cols2.Add(i));
                lbList.DataSource = cols2;
                lbList.DisplayMember = "showName";
                lbList.ValueMember = "name";
                lbList.SelectedIndex = -1;
            }
            else
            { }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                config = comboBox1.SelectedItem as Host;
                string connString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", config.IP, config.DBName, config.User, config.Pw);
                GetTables(connString);
                BindDtTree();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string ns = tbNameSpace.Text.Trim();
            string an = tbAeaName.Text.Trim();

            string path = config.Dir.Trim() + "\\temp";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // 生成Model
            //string procStr = DataTableToProc.Get(currentTable);
            //string procPath = path + "\\procs";
            //if (!Directory.Exists(procPath))
            //{
            //    Directory.CreateDirectory(procPath);
            //}
            //File.WriteAllText(procPath + "\\" + currentTable.name + ".sql", procStr, Encoding.UTF8);

            string className = "";
            string[] subNames = currentTable.name.Split('_');
            if (subNames.Length > 0)
            {
                className = subNames[subNames.Length - 1];
            }
            else
            {
                className = currentTable.name;
            }

            string msg = "已生成：";

            // 生成Model
            if (cbEntity.Checked)
            {
                List<string> searchCols = new List<string>();
                foreach (var item in lbSearch.SelectedItems)
                {
                    searchCols.Add(((DbTableColumn)item).name);
                }

                string modelStr = DataTableToModel.Get(currentTable, ns);
                //string modelPath = path + "\\" + ns + ".Services\\" + className;
                string modelPath = path + "\\" + className + "\\services";
                if (!Directory.Exists(modelPath))
                {
                    Directory.CreateDirectory(modelPath);
                }
                if (!Directory.Exists(modelPath + "\\Entity"))
                {
                    Directory.CreateDirectory(modelPath + "\\Entity");
                }
                File.WriteAllText(modelPath + "\\Entity\\" + className + "Entity.cs", modelStr, Encoding.UTF8);
                msg += "【Entity】";

                if (!Directory.Exists(modelPath + "\\Search"))
                {
                    Directory.CreateDirectory(modelPath + "\\Search");
                }
                string searchStr = DataTableToSearchParam.Get(currentTable, ns, searchCols);
                File.WriteAllText(modelPath + "\\Search\\" + className + "SearchParam.cs", searchStr, Encoding.UTF8);

                if (!Directory.Exists(modelPath + "\\Dtos"))
                {
                    Directory.CreateDirectory(modelPath + "\\Dtos");
                }
                string dtoStr = DataTableToDto.Get(currentTable, ns);
                File.WriteAllText(modelPath + "\\Dtos\\" + className + "Dto.cs", dtoStr, Encoding.UTF8);
            }

            // 生成Factory
            if (cbService.Checked)
            {
                string factoryStr = DataTableToService.Get(currentTable, ns);
                //string factoryPath = path + "\\" + ns + ".Services\\" + className;
                string factoryPath = path + "\\" + className + "\\services";
                if (!Directory.Exists(factoryPath))
                {
                    Directory.CreateDirectory(factoryPath);
                }
                File.WriteAllText(factoryPath + "\\" + className + "Service.cs", factoryStr, Encoding.UTF8);
                msg += "【Service】";
            }

            // 生成Controller
            if (cbController.Checked)
            {
                string controllerStr = DataTableToController.Get(currentTable, ns, an);
                //string controllerPath = path + "\\";
                //if (!string.IsNullOrWhiteSpace(an))
                //{
                //    controllerPath += ns + ".WebSite\\Areas\\" + an + "\\Controllers";
                //}
                //else
                //{
                //    controllerPath += ns + ".WebSite\\Controllers";
                //}
                string controllerPath = path + "\\" + className + "\\controllers";
                if (!Directory.Exists(controllerPath))
                {
                    Directory.CreateDirectory(controllerPath);
                }
                File.WriteAllText(controllerPath + "\\" + className + "Controller.cs", controllerStr, Encoding.UTF8);
                msg += "【Controller】";
            }

            //// 生成CreateView
            if (cbCreateView.Checked)
            {
                string createStr = DataTableToCreateView.Get(currentTable, ns, an);
                //string createPath = path + "\\";
                string createPath = path + "\\" + className + "\\" + className;
                //if (!string.IsNullOrWhiteSpace(an))
                //{
                //    createPath += ns + ".WebSite\\Areas\\" + an + "\\Views\\" + className;
                //}
                //else
                //{
                //    createPath += ns + ".WebSite\\Views\\" + className;
                //}
                if (!Directory.Exists(createPath))
                {
                    Directory.CreateDirectory(createPath);
                }
                File.WriteAllText(createPath + "\\Create.cshtml", createStr, Encoding.UTF8);
                msg += "【CreateView】";
            }

            //// 生成EditView
            if (cbEditView.Checked)
            {
                string editStr = DataTableToEditView.Get(currentTable, ns, an);
                //string editPath = path + "\\";
                //if (!string.IsNullOrWhiteSpace(an))
                //{
                //    editPath += ns + ".Website\\Areas\\" + an + "\\Views\\" + className;
                //}
                //else
                //{
                //    editPath += ns + ".Website\\Views\\" + className;
                //}
                string editPath = path + "\\" + className + "\\" + className;
                if (!Directory.Exists(editPath))
                {
                    Directory.CreateDirectory(editPath);
                }
                File.WriteAllText(editPath + "\\Edit.cshtml", editStr, Encoding.UTF8);
                msg += "【EditView】";
            }

            //// 生成ListView
            if (cbListView.Checked)
            {
                List<string> searchCols = new List<string>();
                foreach (var item in lbSearch.SelectedItems)
                {
                    searchCols.Add(((DbTableColumn)item).name);
                }

                List<string> listCols = new List<string>();
                foreach (var item in lbList.SelectedItems)
                {
                    listCols.Add(((DbTableColumn)item).name);
                }

                string listStr = DataTableToListView.Get(currentTable, searchCols, listCols, ns, an);
                //string listPath = path + "\\";
                //if (!string.IsNullOrWhiteSpace(an))
                //{
                //    listPath += ns + ".Website\\Areas\\" + an + "\\Views\\" + className;
                //}
                //else
                //{
                //    listPath += ns + ".Website\\Views\\" + className;
                //}
                string listPath = path + "\\" + className + "\\" + className;
                if (!Directory.Exists(listPath))
                {
                    Directory.CreateDirectory(listPath);
                }
                File.WriteAllText(listPath + "\\Index.cshtml", listStr, Encoding.UTF8);
                msg += "【ListView】";
            }
            MessageBox.Show(msg);
        }
    }
}
