using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class AmountOfCreatedRestaurantRequirement : IAuthorizationRequirement
    {
        public int MinimalRestaurant { get;}

        public AmountOfCreatedRestaurantRequirement(int minimalRestaurant)
        {
            MinimalRestaurant = minimalRestaurant;
        }
    }
}
