using AutoCar.Entities;
using AutoCar.Exceptions;
using AutoCar.Models.DTO;
using AutoCar.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutoCar.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        bool Delete(int i);
        string GenerateJwt(LoginDto dto);
        public UserView GetUser(int id);
        List<UserView> GetAll();
        void ChangePassword(NewPasswordDto dto);
        void EditUser(UserEditDto dto);
        void ChangeRole(int id, ChangeRoleDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;


        public AccountService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IMapper mapper, IUserContextService userContextService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            if (_context.Users.Any(x => x.Name == dto.Name))
                throw new ConflictException("This name is taken");
            var newUser = new User()
            {
                Name = dto.Name,
                Email = dto.Email,
                RoleId = dto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public bool Delete(int i)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Id == i);
            if (user is null)
            {
                return false;
            }
            if (user.Name == "admin")
                throw new ForbiddenException("You can't delete this account");
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _context.Users.Include(x => x.Role)
                .FirstOrDefault(u => u.Email == dto.Email);
            if (user is null)
            {
                throw new BadRequestException("Invalid Login or password");
            }


            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
            var token = new JwtSecurityToken(_authenticationSettings.JwtIssurl,
                _authenticationSettings.JwtIssurl,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public UserView GetUser(int id)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Id == id);
            if (user == null)
                throw new NotFoundException("User not exist");

            var result = _mapper.Map<UserView>(user);

            return result;
        }

        public List<UserView> GetAll()
        {
            var list = _context.Users.ToList();

            if (list == null)
                throw new NotFoundException("Empty list of Users");

            var result = _mapper.Map<List<UserView>>(list);

            return result;
        }

        public void ChangePassword(NewPasswordDto dto)
        {
            var iduser = (int)_userContextService.GetUserId;

            var user = _context.Users
                .FirstOrDefault(u => u.Id == iduser);
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.OldPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Incorrect password");
            }
            var newhashedPassword = _passwordHasher.HashPassword(user, dto.NewPassword);
            user.PasswordHash = newhashedPassword;
            _context.SaveChanges();
        }

        public void EditUser(UserEditDto dto)
        {
            var iduser = (int)_userContextService.GetUserId;

            var user = _context.Users
                .FirstOrDefault(u => u.Id == iduser);
            if (user == null)
                throw new NotFoundException("User not exist");

            user.Name = dto.Name;
            user.Email = dto.Email;
            _context.SaveChanges();
        }

        public void ChangeRole(int id, ChangeRoleDto dto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Id == id);
            if (user == null)
                throw new NotFoundException("User not exist");

            var role = _context.Roles.FirstOrDefault(u => u.Id == dto.RoleId);
            if (user == null)
                throw new NotFoundException($"Role with id {dto.RoleId} not exist");

            user.RoleId = dto.RoleId;

            _context.SaveChanges();
        }
    }
}
