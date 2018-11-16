using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CodeGenerator
{
    public partial class ConnectForm : Form
    {
        public ConnectForm()
        {
            InitializeComponent();
        }

        #region 属性
        protected string ip = "";
        protected string user = "";
        protected string pw = "";
        protected string db = "";
        protected string ns = "";
        protected string dir = "";

        protected List<Host> hosts = new List<Host>();
        protected Form1 owner;
        #endregion

        #region 获取主机上的所有数据库
        protected void GetAllDb(string hostIp, string dbUser, string dbPw)
        {
            string connStr = GetConnectionString(hostIp, dbUser, dbPw);
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    string sql = string.Format("select name from sysdatabases order by name");
                    SqlCommand sc = new SqlCommand(sql, conn);
                    conn.Open();
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        comboBox1.Items.Add(sdr["name"].ToString());
                    }
                    conn.Close();
                    comboBox1.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }
        #endregion

        #region 返回链接字符串方法
        private string GetConnectionString(string ip, string dbName, string user, string pw)
        {
            return string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", ip, dbName, user, pw);
        }
        private string GetConnectionString(string ip, string user, string pw)
        {
            return string.Format("Data Source={0};Persist Security Info=True;User ID={1};Password={2}", ip, user, pw);
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            ip = tbIp.Text.Trim();
            user = tbUser.Text.Trim();
            pw = tbPw.Text.Trim();

            GetAllDb(ip, user, pw);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            db = comboBox1.SelectedItem as string;
            dir = tbPath.Text.Trim();
            ns = tbNameSpace.Text.Trim();

            if (db.Length > 0)
            {
                Host host = new Host
                {
                    IP = ip,
                    DBName = db,
                    User = user,
                    Pw = pw,
                    Name = string.Format("{0}({1})", ip, db),
                    NameSpace = ns,
                    Dir = dir
                };

                var m = hosts.Where(h => h.Name == host.Name).SingleOrDefault();
                if (m == null)
                {
                    hosts.Add(host);
                }
                else
                {
                    hosts.Remove(m);
                    hosts.Add(host);
                }
                SerializerHelper.SaveToXml(Environment.CurrentDirectory + "/hosts.xml", hosts, typeof(List<Host>), "");

                owner.LinkDb(GetConnectionString(ip, db, user, pw), host);
                this.Close();
            }

            string path = tbPath.Text;
            if(!Directory.Exists(path + "\\Models"))
            {
                Directory.CreateDirectory(path + "\\models");
            }
            if (!Directory.Exists(path + "\\Controllers"))
            {
                Directory.CreateDirectory(path + "\\Controllers");
            }
            if (!Directory.Exists(path + "\\Views"))
            {
                Directory.CreateDirectory(path + "\\Views");
            }
            if (!Directory.Exists(path + "\\Factory"))
            {
                Directory.CreateDirectory(path + "\\Factory");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string path = "";
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowserDialog.SelectedPath;
            }
            else
            {
            }
            tbPath.Text = path;
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            string name = tbIp.Text.Trim() + "(" + comboBox1.SelectedItem + ")";
            var host = hosts.Where(h => h.Name == name).SingleOrDefault();
            if (host != null)
            {
                tbNameSpace.Text = host.NameSpace;
                tbPath.Text = host.Dir;
            }
        }

        private void ConnectForm_Load(object sender, EventArgs e)
        {
            owner = this.Owner as Form1;
            hosts = owner.hosts;
        }
    }
}
