using AutoCar.Entities;
using Microsoft.AspNetCore.Identity;

namespace AutoCar
{
    public interface IDbInitializer
    {
        public void Seed();
    }

    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        public IPasswordHasher<User> _passwordHasher { get; }

        public DbInitializer(ApplicationDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public void Seed()
        {

            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    foreach (var item in roles)
                    {
                        _dbContext.Add(item);
                        _dbContext.SaveChanges();
                    }
                }

                var user = _dbContext.Users.FirstOrDefault(x => x.Email == "admin@test.pl");
                if (user == null)
                {
                    var newAdmin = CreateAdmin();
                    _dbContext.Users.Add(newAdmin);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name="Admin"
                },
                new Role()
                {
                    Name="Manager"
                },
                new Role()
                {
                    Name="Serviceman"
                },
                new Role()
                {
                    Name="Client"
                }
            };
            return roles;
        }

        private User CreateAdmin()// created admin account just for testing (hard coded).
        {
            var newAdmin = new User()
            {
                Email = "admin@test.pl",
                Name = "admin",
                RoleId = 1
            };

            var password = "admin";

            var hashedPassword = _passwordHasher.HashPassword(newAdmin, password);

            newAdmin.PasswordHash = hashedPassword;

            return newAdmin;
        }
    }
}
