using AutoMapper;
using HDBank.API.Models;
using HDBank.API.Models.Response;
using HDBank.Core.Aggregate.AppResult;
using HDBank.Infrastructure.Data;
using HDBank.Infrastructure.Models;
using HDBank.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace HDBank.API.Services
{
    public class AppService : IAppService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtManager _jwtManager;
        private readonly IMapper _mapper;
        private readonly HDBankDbContext _context;

        public AppService(UserManager<AppUser> userManager, IJwtManager jwtManager, IMapper mapper, HDBankDbContext context)
        {
            _userManager = userManager;
            _jwtManager = jwtManager;
            _mapper = mapper;
            _context = context;
        }
        public async Task<ApiResult<string>> Authenticate(LoginModel request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return new ApiErrorResult<string>("User does not exist!");
            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
                return new ApiErrorResult<string>("Username or Password incorrect!");
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtManager.Authenticate(user, roles);
            return new ApiSuccessResult<string>(token);
        }

        public async Task<ApiResult<UserInfoResponse>> GetByClaims(ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);
            if (user == null)
                return new ApiErrorResult<UserInfoResponse>("User does not exist!");
            var result = _mapper.Map<UserInfoResponse>(user);
            return new ApiSuccessResult<UserInfoResponse>(result);
        }
        public async Task<ApiResult<UserInfoResponse>> GetByAccountNo(string accountNo)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.AccountNo == accountNo);
            if (user == null)
                return new ApiErrorResult<UserInfoResponse>("User does not exist!");
            var result = _mapper.Map<UserInfoResponse>(user);
            return new ApiSuccessResult<UserInfoResponse>(result);
        }

        public async Task<ApiResult<string>> Register(RegisterModel request, string accountNo)
        {
            var findUserName = await _userManager.FindByNameAsync(request.UserName);
            var findEmail = await _userManager.FindByEmailAsync(request.Email);

            var isUserNameExists = findUserName != null;
            var isEmailExists = findEmail != null;

            if (isUserNameExists)
                return new ApiErrorResult<string>("This Username Already Used!");
            if (isEmailExists)
                return new ApiErrorResult<string>("This Email Already Used");
            //if (request.Password != request.ConfirmPassword)
            //    return new ApiErrorResult<bool>("Password and Confirm Password are not the same");
            var user = _mapper.Map<AppUser>(request);
            user.AccountNo = accountNo;
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {

                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtManager.Authenticate(user, roles);
                return new ApiSuccessResult<string>(token);
            }
            return new ApiErrorResult<string>(string.Join(' ', result.Errors.Select(error => error.Description)));
        }

        public async Task<ApiResult<bool>> CreateTransaction(TransferModel request, ClaimsPrincipal claims)
        {
            var senderResponse = await GetByClaims(claims);
            if (!senderResponse.Succeeded)
                return new ApiErrorResult<bool>("Cannot found sender");
            var receiverResponse = await GetByAccountNo(request.ToAccount);
            if (!receiverResponse.Succeeded)
                return new ApiErrorResult<bool>("Cannot found receiver");


            var transaction = new Transaction()
            {
                SenderId = senderResponse.ResultObject.Id,
                ReceiverId = receiverResponse.ResultObject.Id,
                Amount = request.Amount,
                Description = request.Description,
            };

            await _context.Transactions.AddAsync(transaction);
            int result = await _context.SaveChangesAsync();
            if (result > 0)
                return new ApiSuccessResult<bool>(true);
            return new ApiErrorResult<bool>("Nothing change");
        }

        public async Task<ApiResult<bool>> ChangePassword(ChangePasswordModel request, ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);
            if (user == null)
                return new ApiErrorResult<bool>("Cannot found user");

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
                return new ApiErrorResult<bool>(string.Join(' ', result.Errors.Select(error => error.Description)));
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<TransactionHistoryResponse>> GetTransactionHistory(TranferHistoryModel request, string acctNo)
        {
            var transactions = await _context.Transactions
                .Where(t => t.CreatedDate >= request.FromDate && t.CreatedDate < request.ToDate)
                .Include(t => t.Sender)
                .Include(t => t.Receiver)
                .ToListAsync();
            var history = _mapper.Map<IList<TransactionHistory>>(transactions);
            var histories = new TransactionHistoryResponse();
            histories.Histories = history;
            return new ApiSuccessResult<TransactionHistoryResponse>(histories);
        }
    }
}
