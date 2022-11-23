namespace _0sechill.Static
{
    public static class UserRole
    {
        public static string Admin = "admin";
        public static string Staffbt = "staffbt";
        public static string Staffst = "staffst";
        public static string Citizen = "resident";
        public static string BlockManager = "blockManager";

        public static List<string> GetFields()
        {
            var listFields = new List<string>();
            listFields.Add(Admin);
            listFields.Add(Staffbt);
            listFields.Add(Staffst);
            listFields.Add(Citizen);
            listFields.Add(BlockManager);
            return listFields;
        }
    }
}
