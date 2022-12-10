using System.ComponentModel.DataAnnotations;
using RestaurantAPI.Data;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Service
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
    }

    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _context;

        public AccountService(RestaurantDbContext context)
        {
            _context = context;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                //PasswordHash = dto.Password,
                RoleId = dto.RoleId,
                UserName = dto.UserName
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
    }
}
