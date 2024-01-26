using Movies.Client.Models;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public interface IAccountService
    {
        public Task<UserInfoViewModel> GetUserInfo();
    }
}
