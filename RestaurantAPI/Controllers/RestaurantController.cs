using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Data;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    public class RestaurantController : Controller
    {
        private readonly RestaurantDbContext _dbContext;
        public RestaurantController(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var result = _dbContext
                .Restaurants
                .ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurant> Get([FromRoute] int id)
        {
            var result = _dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
