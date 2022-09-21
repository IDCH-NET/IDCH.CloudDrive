using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IDCH.CloudDrive.Models
{
    public interface ICrud<T> where T : class
    {
        Task<bool> InsertData(T data);
        Task<bool> UpdateData(T data);
        Task<List<T>> GetAllData();
        Task<T> GetDataById(long Id);
        Task<bool> DeleteData(long Id);
        Task<long> GetLastId();
        Task<List<T>> FindByKeyword(string Keyword);
    }
    [DataContract]
    public class AuthenticatedUserModel
    {
        [DataMember(Order = 1)]
        public string Username { get; set; }
        [DataMember(Order = 2)]
        public string AccessToken { get; set; }
        [DataMember(Order = 3)]
        public string TokenType { get; set; }
        [DataMember(Order = 4)]
        public DateTime? ExpiredDate { get; set; }
    }
    internal class Entities
    {
    }
}
