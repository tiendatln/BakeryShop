using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Security.Cryptography;
using System.Text;
using UserAPI.DTOs;
using UserAPI.Model;
using UserAPI.Repository.Interface;
using UserAPI.Service.Interface;

namespace UserAPI.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IQueryable<ReadUserDTO> GetAllUsers()
        {
            return _userRepository.GetAllUsers().ProjectTo<ReadUserDTO>(_mapper.ConfigurationProvider);
        }

        public async Task<ReadUserDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return null; // Người dùng không tồn tại
            }
            // Map dữ liệu sang DTO kết quả
            ReadUserDTO result = _mapper.Map<ReadUserDTO>(user);
            return result;
        }

        public async Task<ReadUserDTO> CreateUserAsync(CreateUserDTO createUserDTO)
        {
            var user = _mapper.Map<User>(createUserDTO);

            // Tạo password hash và salt
            CreatePasswordHash(createUserDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.RegistrationDate = DateTime.UtcNow;

            var createdUser = await _userRepository.CreateUserAsync(user);

            // Map dữ liệu sang DTO kết quả
            ReadUserDTO result = _mapper.Map<ReadUserDTO>(createdUser);
            return result;
        }

        public async Task<UserValidateResultDTO> ValidateUserAsync(UserValidateDTO dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user == null)
            {
                return new UserValidateResultDTO
                {
                    IsValid = false,
                    ErrorMessage = "Email không tồn tại."
                };
            }

            if (!VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new UserValidateResultDTO
                {
                    IsValid = false,
                    ErrorMessage = "Sai mật khẩu."
                };
            }

            // Map dữ liệu sang DTO kết quả
            UserValidateResultDTO result = _mapper.Map<UserValidateResultDTO>(user);
            result.IsValid = true;
            result.ErrorMessage = null;
            return result;
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key; // tự động sinh salt
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return false; // Người dùng không tồn tại
            }
            return await _userRepository.DeleteUserAsync(id);
        }

        public async Task<ReadUserDTO> UpdateUserAsync(UpdateUserProfileDTO updateUserProfileDTO)
        {
            var user = _mapper.Map<User>(updateUserProfileDTO);

            var result = await _userRepository.UpdateUserAsync(user);
            if (result == null)
            {
                return null; // Người dùng không tồn tại
            }

            // Map dữ liệu sang DTO kết quả
            ReadUserDTO updatedUser = _mapper.Map<ReadUserDTO>(result);
            return updatedUser;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return false; // Email không tồn tại
            }
            return true; // Email đã tồn tại
        }
    }
}
