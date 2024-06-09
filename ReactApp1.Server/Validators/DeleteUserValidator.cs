using ReactApp1.Server.Models;

namespace ReactApp1.Server.Validators
{
    public static class DeleteUserValidator
    {
        public static bool IsDeletePermitted(string currentUserId, string userToDeleteId, AppUserRole roleToDelete, AppUserRole currentRole)
        {
            bool samePerson = UserValidationHelper.IsOperationForTheSamePerson(userToDeleteId, currentUserId);

            if (currentRole == AppUserRole.SuperAdmin)
            {
                return roleToDelete == AppUserRole.Regular || roleToDelete == AppUserRole.Admin;
            }

            if (currentRole == AppUserRole.Admin)
            { 
                return samePerson || (roleToDelete == AppUserRole.Regular);
            }

            if (currentRole == AppUserRole.Regular)
            {
                return samePerson;
            }

            return false;
        }
    }
}
