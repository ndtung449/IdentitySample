namespace IdentitySample.Authorization
{
    using Microsoft.AspNetCore.Authorization.Infrastructure;

    public class OperationRequirements
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";
        public static readonly string ActivateOperationName = "Activate";
        public static readonly string DeactivateOperationName = "Deactivate";

        public static OperationAuthorizationRequirement Create =
          new OperationAuthorizationRequirement { Name = CreateOperationName };
        public static OperationAuthorizationRequirement Read =
          new OperationAuthorizationRequirement { Name = ReadOperationName };
        public static OperationAuthorizationRequirement Update =
          new OperationAuthorizationRequirement { Name = UpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
          new OperationAuthorizationRequirement { Name = DeleteOperationName };
        public static OperationAuthorizationRequirement Activate =
          new OperationAuthorizationRequirement { Name = ActivateOperationName };
        public static OperationAuthorizationRequirement Deactivate =
          new OperationAuthorizationRequirement { Name = DeactivateOperationName };
    }
}
