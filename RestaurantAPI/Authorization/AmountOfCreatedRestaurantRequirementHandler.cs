using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Entities;
using RestaurantAPI.Service;

namespace RestaurantAPI.Authorization
{
    public class AmountOfCreatedRestaurantRequirementHandler : AuthorizationHandler<AmountOfCreatedRestaurantRequirement>
    {
        private readonly ILogger<AmountOfCreatedRestaurantRequirementHandler> _logger;
        private readonly RestaurantDbContext _dbContext;
        private readonly IUserContextService _userContextService;

        public AmountOfCreatedRestaurantRequirementHandler(ILogger<AmountOfCreatedRestaurantRequirementHandler> logger, RestaurantDbContext dbContext
            , IUserContextService userContextService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userContextService = userContextService;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AmountOfCreatedRestaurantRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var amountOfCreatedRestaurant = _dbContext.Restaurants.Count(r => r.CreatedById == userId);

            if (amountOfCreatedRestaurant >= requirement.MinimalRestaurant)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }

        //var restaurant = _dbContext
        //    .Restaurants
        //    .FirstOrDefault(r => r.Id == id);
    }
}
