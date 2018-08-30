using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 30.08.2018 01:24

namespace TelegramBot
{
    public class Version_t
    {
        public UInt32 ClientVersion = 0;
        public UInt32 ServerVersion = 0;
        public String PatchVersion = "";
        public String VersionDate = "";
        public String VersionTime = "";

        public Version_t()
        {

        }

        public bool Parse(string str)
        {
            try
            {
                var lines = str.Split('\n');
                ClientVersion = UInt32.Parse(lines[0].Replace("ClientVersion=", String.Empty));
                ServerVersion = UInt32.Parse(lines[1].Replace("ServerVersion=", String.Empty));
                PatchVersion = lines[2].Replace("PatchVersion=", String.Empty);
                VersionDate = lines[6].Replace("VersionDate=", String.Empty);
                VersionTime = lines[7].Replace("VersionTime=", String.Empty);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool operator ==(Version_t rhs, Version_t lhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                if (object.ReferenceEquals(rhs, null))
                {
                    return true;
                }

                return false;
            }

            return rhs.ClientVersion == lhs.ClientVersion;
        }

        public static bool operator !=(Version_t rhs, Version_t lhs)
        {
            return !(rhs == lhs);
        }
    }
}
