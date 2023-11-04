namespace firstDotnetProject.Repository.iRepository;

public interface IUserRepository
{
    bool IsUniqueUser(string username);
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    Task<LocalUser> Register(RegisterationRequestDto registerationRequestDto);
}