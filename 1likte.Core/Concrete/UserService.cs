using _1likte.Core.Services;
using _1likte.Model.DbModels;
using _1likte.Data;
using Microsoft.EntityFrameworkCore;
using _1likte.Model.ViewModels;
using AutoMapper;
using _1likte.Model.ViewModels.User;




namespace _1likte.Core.Concrete
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public UserService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<User> GetUserById(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid user ID.");

            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found.");

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                var users = await _dbContext.Users.ToListAsync();
                return _mapper.Map<IEnumerable<User>>(users);
            }
            catch (Exception ex)
            {
                throw new Exception($"Kullanıcılar alınırken bir hata oluştu: {ex.Message}");
            }
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



        public async Task<User> UpdateUser(UserUpdateModel user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var existingUser = await _dbContext.Users.FindAsync(user.Id);
            if (existingUser == null) throw new KeyNotFoundException("User not found.");

            if (!string.IsNullOrEmpty(user.Email) && user.Email != existingUser.Email)
            {
                if (await _dbContext.Users.AnyAsync(u => u.Email == user.Email))
                    throw new InvalidOperationException("The email is already in use.");
            }

            existingUser.FullName = user.FullName ?? existingUser.FullName;
            existingUser.Email = user.Email ?? existingUser.Email;

            _dbContext.Users.Update(existingUser);
            await _dbContext.SaveChangesAsync();
            return existingUser;
        }



    }
}