using System;
namespace IamService
{
    public sealed class Constants
    {
        public sealed class ServiceMessages
        {
            public const string CREATE_USER_EMAIL_IN_USE = "The email address is already used";
            public const string CREATE_USER_USERNAME_IN_USE = "The username is already used";
            public const string CREATE_USER_INVALID_ROLE = "One or more of the provided role(s) is invalid";
            public const string CREATE_USER_NO_ROLE = "Must provide at least one role";
            public const string CREATE_USER_INVALID_USER_OR_PASSWORD = "Invalid username or password";
            public const string ADD_USER_ACTIVITY_INVALID_USER = "The provided user id is invalid";
            public const string ADD_USER_ACTIVITY_INVALID_ACTION_TYPE = "The provided action type is invalid";
            public const string GET_USER_INVALID_USER_ID = "The provided user id is invalid";
        }
    }
}
