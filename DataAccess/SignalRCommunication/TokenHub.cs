using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.SignalR;
namespace DataAccess
{
    public class TokenHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        public TokenHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task UpdateTokenCount(string userId, int tokenCount)
        {
            await Clients.User(userId).SendAsync("ReceiveTokenUpdate", tokenCount);
        }
        public async Task<int> GetCurrentTokenCount(string userId)
        {
            // Fetch the user's current token count from the database using the UnitOfWork
            var user = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(u => u.Id == userId);
            return user?.Tokens ?? 0;
        }
    }
}
