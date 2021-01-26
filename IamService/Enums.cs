using System;
namespace IamService
{
    public static class Enums
    {
        public enum UserActionType
        {
            CreateAccount = 1,
            Login,
            EditProfile
        }

        public enum Role
        {
            Admin = 1,
            Employee,
            Manager
        }
    }
}
