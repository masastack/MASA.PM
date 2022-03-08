namespace MASA.PM.UI.Admin.Model
{
    public class EnvClusterModel
    {
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public int Index { get; set; }

        public EnvClusterModel(int index, string name, string description)
        {
            Index = index;
            Name = name;
            Description = description;
        }

        public EnvClusterModel(int index)
        {
            Index = index;
        }
    }
}
