using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TrueFireCallTestCLI
{
    class Program
    {
        static CookieContainer Cookies = new CookieContainer();

        static void Main(string[] args)
        {
            Login("TenaciousG", "TN1Pqhs8ib");

            // Now the cookies in "Cookies" are all set.
            // Ensure you set CookieContainer on all subsequent requests
        }

        static void Login(string username, string password)
        {
            var wr = (HttpWebRequest)WebRequest.Create("https://truefire.com/api/kudzu/access/login");
            wr.Method = "POST";
            wr.ContentType = "application/x-www-form-urlencoded";
            wr.Referer = "https://truefire.com/login/"; // my tests show this is needed
            wr.CookieContainer = Cookies;

            var parameters = new Dictionary<string, string>{
                //{"realm", "vzw"},
                //{"goto",""},
                //{"gotoOnFail",""},
                //{"gx_charset", "UTF-8"},
                //{"rememberUserNameCheckBoxExists","Y"},
                //{"IDToken1", username},
                //{"IDToken2", password},
                {"username", password},
                {"password", password},
                {"remember", "false"}
            };

            using (var requestStream = wr.GetRequestStream())
                using (var writer = new StreamWriter(requestStream, Encoding.UTF8))
                    writer.Write(ParamsToFormEncoded(parameters));

            using (var response = (HttpWebResponse)wr.GetResponse())
            {
                // here you need to detect a correct login... this might be one of the cookies.
                // if incorrect throw an exception or something.
            }
        }

        static string ParamsToFormEncoded(Dictionary<string, string> parameters)
        {
            return string.Join("&", parameters.Select(kvp =>
                Uri.EscapeDataString(kvp.Key).Replace("%20", "+") + "=" + Uri.EscapeDataString(kvp.Value).Replace("%20", "+")
            ).ToArray());
        }
    }
}
