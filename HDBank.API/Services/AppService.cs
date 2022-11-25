using AutoMapper;
using HDBank.API.Models;
using HDBank.Core.Aggregate.AppResult;
using HDBank.Infrastructure.Models;
using HDBank.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace HDBank.API.Services
{
    public class AppService : IAppService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtManager _jwtManager;
        private readonly IMapper _mapper;

        public AppService(UserManager<AppUser> userManager, IJwtManager jwtManager, IMapper mapper)
        {
            _userManager = userManager;
            _jwtManager = jwtManager;
            _mapper = mapper;
        }
        public async Task<ApiResult<string>> Authenticate(LoginModel request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return new ApiErrorResult<string>("User does not exist!");
            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
                return new ApiErrorResult<string>("Username or Password Incorrect!");
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtManager.Authenticate(user, roles);
            return new ApiSuccessResult<string>(token);
        }
        public async Task<ApiResult<bool>> Register(RegisterModel request)
        {
            var findUserName = await _userManager.FindByNameAsync(request.UserName);
            var findEmail = await _userManager.FindByEmailAsync(request.Email);

            var isUserNameExists = findUserName != null;
            var isEmailExists = findEmail != null;

            if (isUserNameExists)
                return new ApiErrorResult<bool>("This Username Already Used!");
            if (isEmailExists)
                return new ApiErrorResult<bool>("This Email Already Used");
            //if (request.Password != request.ConfirmPassword)
            //    return new ApiErrorResult<bool>("Password and Confirm Password are not the same");
            var user = _mapper.Map<AppUser>(request);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
                return new ApiSuccessResult<bool>(true);
            return new ApiErrorResult<bool>(string.Join(' ', result.Errors.Select(error => error.Description)));
        }
    }
}
