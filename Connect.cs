using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary2
{
    public class Connect
    {
        static readonly Connect connect = new Connect();

        public static Connect GetConnect
        {
            get { return connect; }
        }

        private static string connectbd;
        private static string providername;


        public static string Connectbd
        {

            get { return connectbd; }
        }

        public static string ProviderName
        {
            get { return providername; }
        }

        private Connect() { }

        public static void GetInstation(string connect, string provider)
        {
            connectbd = connect;
            providername = provider;
        }

    }
}
