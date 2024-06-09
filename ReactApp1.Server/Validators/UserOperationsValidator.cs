using ReactApp1.Server.Models;

namespace ReactApp1.Server.Validators
{
    public static class UserOperationsValidator
    {
        public static bool IsChangeRolePermitted(string currentUserId, string userToChangeId, AppUserRole roleToChange, AppUserRole currentRole)
        {
            bool samePerson = IsOperationForTheSamePerson(userToChangeId, currentUserId);

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

        public static bool IsSetAvatarPermitted(string currentUserId, string userToSetAvatarId, AppUserRole roleToSetAvatar, AppUserRole currentRole)
        {
            bool samePerson = IsOperationForTheSamePerson(userToSetAvatarId, currentUserId);

            if (currentRole == AppUserRole.SuperAdmin)
            {
                return true;
            }

            if (currentRole == AppUserRole.Admin)
            {
                return roleToSetAvatar == AppUserRole.Regular || roleToSetAvatar == AppUserRole.Admin;
            }

            if (currentRole == AppUserRole.Regular)
            {
                return samePerson;
            }

            return false;
        }

        public static bool IsDeletePermitted(string currentUserId, string userToDeleteId, AppUserRole roleToDelete, AppUserRole currentRole)
        {
            bool samePerson = IsOperationForTheSamePerson(userToDeleteId, currentUserId);

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

        private static bool IsOperationForTheSamePerson(string id1, string id2)
        {
            return id1 == id2;
        }
    }
}
