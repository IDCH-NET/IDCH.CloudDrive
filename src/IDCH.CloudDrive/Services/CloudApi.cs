using IDCH.CloudDrive.Data;
using NextcloudApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCH.CloudDrive.Services
{
    public class CloudApi
    {
        static Settings _settings;
        static Api _api;
        public UserLib Users { get; set; }
        public GroupLib Groups { get; set; }
        public GroupFolderLib GroupFolders { get; set; }
        public FolderLib Folders { get; set; }
        public static Api Api
        {
            get
            {
                if (_api == null)
                {
                    _api = new Api(Settings);
                }
                return _api;
            }
        }

        public static Settings Settings
        {
            get
            {
                if (_settings == null)
                {
                    /*
					_settings = new Settings() { };
					_settings.ApplicationName = "files";
					_settings.Username = "nbprayuga";
					_settings.Password = "nandazaqwsx";
					_settings.ServerUri = new Uri("https://drive.idcloudhost.com/");
                    List<string> errors = _settings.Validate();
                    if (errors.Count > 0)
                        throw new ApplicationException(string.Join("\r\n", errors));
                    
                    //_settings.RedirectUri = "";
					*/

                    string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NextcloudApi");
                    Directory.CreateDirectory(dataPath);
                    string filename = Path.Combine(dataPath, "TestSettings.json");
                    _settings = new Settings();
                    _settings.ApplicationName = "fadhil_customer";
                    _settings.ServerUri = new Uri(AppConstants.ServerURI);
                    _settings.RedirectUri = new Uri(AppConstants.RedirectURI);
                    //_settings.RedirectAfterLogin = AppConstants.RedirectURI;
                    _settings.Load(filename);
                    List<string> errors = _settings.Validate();
                    if (errors.Count > 0)
                        throw new ApplicationException(string.Join("\r\n", errors));

                }
                return _settings;
            }
        }

        const string alphabet = "ybndrfg8ejkmcpqxot1uwisza345h769";
        public CloudApi()
        {
            if (Api != null && !string.IsNullOrEmpty( Api.Settings.AccessToken))
            {
                Users = new UserLib(Api);
                Groups = new GroupLib(Api);
                GroupFolders = new GroupFolderLib(Api);
                Folders = new FolderLib(Api, Settings);
            }
        }

        public bool IsReady()
        {
            return !string.IsNullOrEmpty(Api.Settings.AccessToken);
        }
        public string UniqueId()
        {
            Guid guid = Guid.NewGuid();
            byte[] bytes = guid.ToByteArray();
            StringBuilder output = new StringBuilder();
            for (int bitIndex = 0; bitIndex < bytes.Length * 8; bitIndex += 5)
            {
                int dualbyte = bytes[bitIndex / 8] << 8;
                if (bitIndex / 8 + 1 < bytes.Length)
                    dualbyte |= bytes[bitIndex / 8 + 1];
                dualbyte = 0x1f & (dualbyte >> (16 - bitIndex % 8 - 5));
                output.Append(alphabet[dualbyte]);
            }
            return output.ToString().Substring(0, 26);
        }

        public async Task<bool> Login(string username, string password)
        {
            try
            {
                if (Settings != null)
                {
                    Settings.ClientId = AppConstants.ClientIdNextCloud;
                    Settings.ClientSecret = AppConstants.ClientSecretNextCloud;
                    Settings.Username = username;
                    Settings.Password = password;
                    var errs = Settings.Validate();
                    if (errs.Count <= 0)
                    {
                        await Api.LoginAsync();
                        Users = new UserLib(Api);
                        Groups = new GroupLib(Api);
                        GroupFolders = new GroupFolderLib(Api);
                        Folders = new FolderLib(Api, Settings);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("validation auth error:" + String.Join(',', errs));
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("login error:" + ex.ToString());
                return false;
            }

            return false;
        }
    }

    public class UserLib
    {
        Api Api;
        public UserLib(Api api)
        {
            this.Api = api;
        }
        public async Task<User> GetUser()
        {
            return await User.Get(Api);
        }
        public async Task<List<string>> GetUsers()
        {
            var api = await User.List(Api);
            return api.List;
        }
        public async Task<bool> CreateUser(string userName, string displayName, string Email, string userId, string password, string[] groups = null)
        {
            try
            {
                if (groups == null) groups = new string[] { "everyone" };
                UserInfo u = new UserInfo()
                {
                    password = password,
                    groups = groups
                };
                u.userid = userId;
                u.displayName = displayName;
                u.email = Email;
                await User.Create(Api, u);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"create user error: {ex}");
                return false;
            }
        }
        public async Task<List<string>> GetGroups()
        {
            var api = await User.GetGroups(Api, Api.Settings.Username);
            return api.List;
        }
        public async Task<List<string>> ListSubadminGroups()
        {
            var lst = await User.GetSubadminGroups(Api, Api.Settings.Username);
            return lst;
        }
    }

    public class GroupLib
    {
        Api Api;
        public GroupLib(Api api)
        {
            this.Api = api;
        }


        public async Task<List<string>> GetGroups()
        {
            var api = await Group.List(Api);
            return api.List;
        }

        public async Task<PlainList<string>> GetMembers(string GroupId)
        {
            var members = await Group.GetMembers(Api, GroupId);
            return members;
        }
    }

    public class GroupFolderLib
    {
        Api Api;
        public GroupFolderLib(Api api)
        {
            this.Api = api;
        }

        public async Task<List<GroupFolder>> GetGroupFolders()
        {
            var api = await GroupFolder.List(Api);
            return api.List;
        }
        public async Task<GroupFolder> GetGroupFolder(int FolderId)
        {
            var obj = await GroupFolder.Get(Api, FolderId);
            return obj;
        }

        public async Task<bool> CreateFolder(string Name, GroupFolder.Permissions Permission = GroupFolder.Permissions.All, long Quota = 1000000000)
        {
            try
            {
                int id = await GroupFolder.Create(Api, Name);
                /*
                if (!(await (Group.List(Api, new ListRequest() { search = Name }))).List.Any(n => n == Name))
                {
                    RunTest(Group.Create(Api, "test"));
                }*/

                GroupFolder g = await GroupFolder.Get(Api, id);
                await g.SetPermissions(Api, "everyone", Permission);
                await g.SetQuota(Api, Quota);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"create folder error:{ex}");
                return false;
            }


        }

        public async Task<bool> DeleteFolder(int GroupId)
        {
            try
            {
                GroupFolder g = await GroupFolder.Get(Api, GroupId);
                if (g != null)
                {
                    await GroupFolder.Delete(Api, GroupId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"delete folder group error:{ex}");
            }
            return false;
        }


    }

    public class FolderLib
    {
        Settings Settings;
        Api Api;
        public FolderLib(Api api, Settings settings)
        {
            this.Settings = settings;
            this.Api = api;
        }

        public async Task<List<CloudInfo>> List()
        {
            try
            {
                var list = await CloudFolder.List(Api, Settings.Username);

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"list folder error: {ex}");
                return default;
            }

        }

        public async Task<List<CloudInfo>> ListAll()
        {
            var list = await CloudFolder.List(Api, Settings.Username + "/Documents", CloudInfo.Properties.All);
            return list;
        }

        public async Task SetFavorite(string FolderName = "Documents", bool State = true)
        {
            string docs = Settings.Username + "/" + FolderName;
            await CloudFolder.SetFavorite(Api, docs, State);
        }

        public async Task<List<CloudInfo>> GetFavorites()
        {
            var list = await CloudFolder.GetFavorites(Api, Settings.Username, CloudInfo.Properties.All);
            return list;
        }

        public async Task<bool> CreateFolder(string FolderName = "Documents/test")
        {
            try
            {

                await CloudFolder.Create(Api, Settings.Username + "/" + FolderName);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("create folder error:{ex}");
                return false;
            }

        }
        public async Task<bool> DeleteFolder(string FolderName = "Documents/test")
        {
            try
            {
                await CloudFolder.Delete(Api, Settings.Username + "/" + FolderName);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"delete folder error: {ex}");
                return false;
            }

        }
        public async Task<List<Comment>> ListComments(string FolderName = "Documents")
        {
            try
            {
                CloudInfo props = await CloudInfo.GetProperties(Api, Settings.Username + "/" + FolderName, CloudInfo.Properties.FileId);
                if (props != null)
                {
                    var list = await Comment.List(Api, props.FileId);
                    return list.List;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"list comment error: {ex}");
            }
            return default;

        }
    }
}
