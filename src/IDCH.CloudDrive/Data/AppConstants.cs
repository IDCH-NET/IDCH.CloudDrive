using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDCH.CloudDrive.Data
{
    public class AppConstants
    {
        public const string NameKey = "Nama";
        public const string Authentication = "auth";
        public const string GemLic = "EDWG-SKFA-D7J1-LDQ5";
        public static string? DefaultPass { get; set; } = "123qweasd";
        
        public static LocalMemoryStorageService DataSession { set; get; }
    }
}
