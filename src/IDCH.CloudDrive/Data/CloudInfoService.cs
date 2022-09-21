using IDCH.CloudDrive.Models;
using IDCH.CloudDrive.Services;
using NextcloudApi;

namespace IDCH.CloudDrive.Data
{
    public class CloudInfoService 
    {
        CloudApi api;
        public CloudInfoService(CloudApi api)
        {
            this.api = api;


        }
        public async Task<bool> DeleteData(string FolderName)
        {
            var res = await api.Folders.DeleteFolder(FolderName);
            return res;
        }

        public async Task<List<CloudInfo>> FindByKeyword(string Keyword)
        {
            var list = await api.Folders.ListAll();
            var data = list.Where(x => x.Path.Contains(Keyword) || x.Id.Contains(Keyword) || x.FileId.Contains(Keyword)).ToList();
            return data;
        }

        public async Task<List<CloudInfo>> GetAllData()
        {
            try
            {
                var list = await api.Folders.ListAll();
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                
            }
            return default;
        }



        public async Task<CloudInfo> GetDataById(string Id)
        {
            var list = await api.Folders.ListAll();
            var data = list.Where(x => x.Id == Id).FirstOrDefault();
            return data;
        }


        public async Task<bool> InsertData(string FolderPath)
        {
            try
            {
                var res = await api.Folders.CreateFolder(FolderPath);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

       


    }
}
