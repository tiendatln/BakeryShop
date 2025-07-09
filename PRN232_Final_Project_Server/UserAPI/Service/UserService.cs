using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public IQueryable<User> GetAllUsersForOData()
        {
            // Trả về IQueryable<User> để OData có thể truy vấn
            return _userRepository.GetAllUsers();
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
            // Kiểm tra người dùng có tồn tại không
            var existingUser = await _userRepository.GetUserByIdAsync(updateUserProfileDTO.UserId);
            if (existingUser == null)
            {
                return null; // Người dùng không tồn tại
            }
            // Map dữ liệu từ DTO sang mô hình User
            var updatedUser = _mapper.Map<User>(updateUserProfileDTO);
            updatedUser.RegistrationDate = existingUser.RegistrationDate; // Giữ nguyên ngày đăng ký
            updatedUser.PasswordHash = existingUser.PasswordHash; // Giữ nguyên mật khẩu
            updatedUser.PasswordSalt = existingUser.PasswordSalt; // Giữ nguyên salt

            var result = await _userRepository.UpdateUserAsync(updatedUser);
            if (result == null)
            {
                return null; // Người dùng không tồn tại
            }

            // Map dữ liệu sang DTO kết quả
            ReadUserDTO readUserDTO = _mapper.Map<ReadUserDTO>(result);
            return readUserDTO;
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

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            // Kiểm tra email có tồn tại không
            var user = await _userRepository.GetUserByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
            {
                return false; // Người dùng không tồn tại
            }

            // Tạo password hash và salt mới
            CreatePasswordHash(resetPasswordDTO.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Cập nhật thông tin người dùng
            var updatedUser = await _userRepository.UpdateUserAsync(user);
            return updatedUser != null; // Trả về true nếu cập nhật thành công
        }
    }
}
