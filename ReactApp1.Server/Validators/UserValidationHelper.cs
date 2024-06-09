namespace ReactApp1.Server.Validators
{
    public static class UserValidationHelper
    {
        public static bool IsOperationForTheSamePerson(string id1, string id2)
        {
            return id1 == id2;
        }
    }
}
