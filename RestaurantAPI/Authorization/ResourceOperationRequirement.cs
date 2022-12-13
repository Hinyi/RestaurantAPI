using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public enum ResouceOperation
    {
        Create,
        Read,
        Update,
        Delete
    }
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperationRequirement(ResouceOperation resouceOperation)
        {
            ResouceOperation = resouceOperation;
        }
        public ResouceOperation ResouceOperation { get;}
    }
}
