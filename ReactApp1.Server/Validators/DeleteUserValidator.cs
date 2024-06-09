using ReactApp1.Server.Models;

namespace ReactApp1.Server.Validators
{
    public static class DeleteUserValidator
    {
        public static bool IsDeletePermitted(string currentUserId, string userToDeleteId, AppUserRole roleToDelete, AppUserRole currentRole)
        {
            if (currentRole == AppUserRole.SuperAdmin)
            {
                return !IsOperationForTheSamePerson(currentUserId, userToDeleteId) && (roleToDelete == AppUserRole.Regular || roleToDelete == AppUserRole.Admin);
            }

            if (currentRole == AppUserRole.Admin)
            { 
                return IsOperationForTheSamePerson(currentUserId, userToDeleteId) || (roleToDelete == AppUserRole.Regular);
            }

            if (currentRole == AppUserRole.Regular)
            {
                return IsOperationForTheSamePerson(currentUserId, userToDeleteId);
            }

            return false;
        }

        private static bool IsOperationForTheSamePerson (string id1, string id2)
        {
            return id1 == id2;
        }
    }
}
