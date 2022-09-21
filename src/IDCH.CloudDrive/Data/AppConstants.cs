using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDCH.CloudDrive.Data
{
    public class AppConstants
    {
        public const string ServerURI = "https://drive.idcloudhost.com/";
        public const string RedirectURI = "https://localhost:5000";
        public const string ClientIdNextCloud = "MB7Wg43GucJQBjjuE9SKFT3yseaJcrUoGQHSky5Ei6lEkSjbbKpL5fjaVetB9TEF";
        public const string ClientSecretNextCloud = "yA1XmH1RvXf8ieX3LqoACPYtP10V8oORS9EUJ6LFl6REc3VPoGfwcLlh4H7viEGt";
        public const string NameKey = "Nama";
        public const string Authentication = "auth";
        public const string GemLic = "EDWG-SKFA-D7J1-LDQ5";
        public static string? DefaultPass { get; set; } = "123qweasd";
        
        public static LocalMemoryStorageService DataSession { set; get; }
    }
}
