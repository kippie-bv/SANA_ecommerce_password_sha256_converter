using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//legacyPassword that needs to be generated: 0:DcSewkJzXofVoADTSKqYmA==:fb1d826cab1e85776c4cde310685ffe7a161559b3cf6bb9618d47170fa5de27a

namespace PasswordMigration
{
    class PasswordObject
    {
        public string ReferenceId { get; }
        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
        public string Salt { get; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            dynamic jsonArray = JArray.Parse(File.ReadAllText(@"PATH_TO_READ.json"));
            Console.Write(jsonArray);
            foreach(var c in jsonArray)
            {
                string password = c.Password;
                string salt = c.Salt;
                string convertedPassword = convert(password);
                c.ShopwareLegacyEncoder = "Sha256";
                c.ShopwareLegacyPassword = "0:" + salt + ":" + convertedPassword;
            }
                
                Console.Write(jsonArray);
                File.WriteAllText(@"PATH_TO_RESULT.json", jsonArray.ToString());
        }

        static string convert(string hash)
        {
            string output = "";
            byte[] hashedValue = Convert.FromBase64String(hash);
            string asciiString = Encoding.UTF7.GetString(hashedValue);
            foreach (char c in asciiString)
            {
                int tmp = c;
                output += String.Format("{0:x2}",
                          (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return output;
        }
    }
}
