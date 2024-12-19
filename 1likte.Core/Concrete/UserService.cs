using _1likte.Core.Services;
using _1likte.Model.DbModels;
using _1likte.Data;
using Microsoft.EntityFrameworkCore;
using _1likte.Model.ViewModels;




namespace _1likte.Core.Concrete
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        LoginResponseModel IUserService.Login(LoginRequest loginRequest)
        {
            // Örnek kullanıcı verileri (mock veri)
            var users = new List<LoginResponseModel>
              {
                  new LoginResponseModel
                  {
                      UserId = 1,
                      Email = "test@example.com",
                      FullName = "John Doe",
                  },
                  new LoginResponseModel
                  {
                      UserId = 2,
                      Email = "jane@example.com",
                      FullName = "Jane Doe",
                  }
              };

            // Kullanıcıyı e-posta ile bul
            var user = users.FirstOrDefault(u => u.Email == loginRequest.Email);

            if (user == null)
                return null;

            // Şifre kontrolü (şifre hashleme ile geliştirilmesi önerilir)
            if (user.Email != loginRequest.Password)
                return null;

            // Kullanıcı doğrulandıysa döndür
            return new LoginResponseModel
            {
                UserId = user.UserId,
                Email = user.Email,
                FullName = user.FullName
            };
        }
        public async Task<User> RegisterUser(User user)
        {
            // Null kontrolü
            if (user == null) throw new ArgumentNullException(nameof(user));

            // Email kontrolü
            if (await _dbContext.Users.AnyAsync(u => u.Email == user.Email))
                throw new InvalidOperationException("The email is already in use.");

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid user ID.");

            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found.");

            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var existingUser = await _dbContext.Users.FindAsync(user.Id);
            if (existingUser == null) throw new KeyNotFoundException("User not found.");

            if (!string.IsNullOrEmpty(user.Email) && user.Email != existingUser.Email)
            {
                if (await _dbContext.Users.AnyAsync(u => u.Email == user.Email))
                    throw new InvalidOperationException("The email is already in use.");
            }

            existingUser.Name = user.Name ?? existingUser.Name;
            existingUser.Email = user.Email ?? existingUser.Email;

            _dbContext.Users.Update(existingUser);
            await _dbContext.SaveChangesAsync();
            return existingUser;
        }



    }
}