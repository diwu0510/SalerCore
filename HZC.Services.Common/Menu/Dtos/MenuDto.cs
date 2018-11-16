namespace HZC.Common.Services
{
    public class MenuDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        public string Roles { get; set; } = "";

        public int ParentId { get; set; }

        public int Level { get; set; }

        public int Sort { get; set; }
    }
}
