namespace _0sechill.Static
{
    public static class UserRole
    {
        public static string Admin = "admin";
        public static string Staff = "staff";
        public static string Citizen = "citizen";
        public static string BlockManager = "blockManager";

        public static List<string> GetFields()
        {
            var listFields = new List<string>();
            listFields.Add(Admin);
            listFields.Add(Staff);
            listFields.Add(Citizen);
            listFields.Add(BlockManager);
            return listFields;
        }
    }
}
