using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IDCH.CloudDrive.Models
{
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
