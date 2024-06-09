using ReactApp1.Server.Models;

namespace ReactApp1.Server.Validators
{
    public static class ChangeUserRoleValidator
    {
        public static bool IsChangeRolePermitted(string currentUserId, string userToChangeId, AppUserRole roleToChange, AppUserRole currentRole)
        {
            bool samePerson = UserValidationHelper.IsOperationForTheSamePerson(userToChangeId, currentUserId);

            if (currentRole == AppUserRole.SuperAdmin)
            {
                return roleToChange == AppUserRole.Regular || roleToChange == AppUserRole.Admin;
            }

            if (currentRole == AppUserRole.Admin)
            {
                return !samePerson || (roleToChange == AppUserRole.Regular);
            }

            return false;
        }
    }
}
