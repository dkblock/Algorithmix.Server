namespace Algorithmix.Common.Constants
{
    public static class Roles
    {
        public const string Administrator = "admin";
        public const string Moderator = "moderator";
        public const string User = "user";

        public const string Executive = Administrator + "," + Moderator;
    }
}
