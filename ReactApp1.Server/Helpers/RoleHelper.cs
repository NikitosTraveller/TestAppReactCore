using ReactApp1.Server.Models;

namespace ReactApp1.Server.Helpers
{
    public static class RoleHelper
    {
        public static AppUserRole GetRoleByName(string roleName)
        {
            return (AppUserRole)Enum.Parse(typeof(AppUserRole), roleName);
        }
    }
}
