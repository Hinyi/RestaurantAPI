using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Service;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IMapper _mapper;

        public RestaurantController(IRestaurantService restaurantService, IMapper mapper)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        [Authorize(Policy = "Atleast20")]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var restaurantDtos = _restaurantService.GetAll();


            return Ok(restaurantDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Restaurant> Get([FromRoute] int id)
        {
            var restaurantDtos = _restaurantService.GetById(id);

            return Ok(restaurantDtos);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var id = _restaurantService.Create(dto);

            return Created($"/api/restaurant/{id}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.Delete(id);

            return NoContent();
            
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateRestaurantDto dto)
        {
           _restaurantService.Update(id, dto);

            return NoContent();
        }
    }
}
