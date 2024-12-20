using _1likte.Core.Services;
using _1likte.Model.DbModels;
using _1likte.Data;
using Microsoft.EntityFrameworkCore;
using _1likte.Model.ViewModels;
using AutoMapper;
using _1likte.Model.ViewModels.User;
using _1likte.Model.ViewModels.Common.MachineGo.Model.ViewModel;




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

        public async Task<ValidatedModel<UserResponseModel>> GetUserById(int id)
        {
            if (id <= 0) throw new ArgumentException("Geçersiz kullanıcı kimliği.");

            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("Kullanıcı bulunamadı.");

            var map =  _mapper.Map<UserResponseModel>(user);
            var response = new ValidatedModel<UserResponseModel>(map);
            return response;
        }

        public async Task<ValidatedModel<IEnumerable<UserResponseModel>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _dbContext.Users.ToListAsync();
                var map = _mapper.Map<IEnumerable<UserResponseModel>>(users);
                var response = new ValidatedModel<IEnumerable<UserResponseModel>>(map);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Kullanıcılar alınırken bir hata oluştu: {ex.Message}");
            }
        }
       
        public async Task<ValidatedModel<UserResponseModel>> UpdateUser(UserUpdateModel user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var existingUser = await _dbContext.Users.FindAsync(user.Id);
            if (existingUser == null) throw new KeyNotFoundException("Kullanıcı bulunamadı.");

            if (!string.IsNullOrEmpty(user.Email) && user.Email != existingUser.Email)
            {
                if (await _dbContext.Users.AnyAsync(u => u.Email == user.Email))
                    throw new InvalidOperationException("Bu email zaten kullanılıyor.");
            }

            existingUser.FullName = user.FullName ?? existingUser.FullName;
            existingUser.Email = user.Email ?? existingUser.Email;
            existingUser.UpdatedAt = DateTime.UtcNow;
            existingUser.UpdatedBy = user.Id;


            _dbContext.Users.Update(existingUser);
            await _dbContext.SaveChangesAsync();
            var map = _mapper.Map<UserResponseModel>(existingUser);
            var response = new ValidatedModel<UserResponseModel>(map);
            return response;
        }
    }
}