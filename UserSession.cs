using System;

namespace Student_Management_Syestem
{
    public static class UserSession
    {
        public static string Role { get; set; } = "Admin";
        public static string UserName { get; set; } = "System Admin";
        public static string Email { get; set; } = "admin@nsbm.lk";

        public static bool IsAdmin =>
            string.Equals(Role, "Admin", StringComparison.OrdinalIgnoreCase);

        public static void SetUser(string role, string userName, string email)
        {
            Role = role;
            UserName = userName;
            Email = email;
        }

        public static void Clear()
        {
            Role = "Admin";
            UserName = "System Admin";
            Email = "";
        }
    }
}
