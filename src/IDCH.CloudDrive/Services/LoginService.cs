using IDCH.CloudDrive.Data;
using IDCH.CloudDrive.Helpers;
using IDCH.CloudDrive.Models;

namespace IDCH.CloudDrive.Services
{
    public interface ILoginService
    {
        Task<AuthenticatedUserModel> Authenticate(string username, string password);
        Task<AuthenticatedUserModel> Authenticate(string apiKey);
    }
    public class LoginService : ILoginService
    {
        private readonly CloudApi _api;
        ISyncMemoryStorageService _localStorage;
        public LoginService(CloudApi api, ISyncMemoryStorageService localStorage)
        {
            this._api = api;
            this._localStorage = localStorage;

        }

        public async Task<AuthenticatedUserModel> Authenticate(string username, string password)
        {

            var res = new AuthenticatedUserModel()  {  };
            var hasil = await _api.Login(username, password);
            if (hasil)
                res.Username = username;
            if (!string.IsNullOrEmpty(res.Username))
            {
               
                await _localStorage.SetItem(AppConstants.Authentication, res);
                //AppConstant.ApiSecret = data.AccessToken;
                return res;
            }
            return null;
        }

        public Task<AuthenticatedUserModel> Authenticate(string apiKey)
        {
            throw new NotImplementedException();
        }
    }
}
