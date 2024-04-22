using Store.Service.Services.UserService.Dtos;

namespace Store.Service.Services.UserService
{
    public interface IUserService
    {
        Task<UserDto> Register(RegisterDto input);
        Task<UserDto> Login(LoginDto input);
    }
}
