using System;

namespace Student_Management_Syestem
{
    public static class UserSession
    {
        public static string Role { get; set; } = "Student";
        public static string UserName { get; set; } = "Student User";
        public static string Email { get; set; } = "student@nsbm.lk";

        public static bool IsAdmin => false;

        public static void SetUser(string userName, string email)
        {
            Role = "Student";
            UserName = userName;
            Email = email;
        }

        public static void SetUser(string role, string userName, string email)
        {
            Role = "Student";
            UserName = userName;
            Email = email;
        }

        public static void Clear()
        {
            Role = "Student";
            UserName = "Student User";
            Email = "";
        }
    }
}
