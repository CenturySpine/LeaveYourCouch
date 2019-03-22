using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace LeaveYourCouch.Mvc.Business.Services
{
    public class SecretConfiguration
    {
        static private string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Microsoft\\UserSecrets\\leaveyourcouch\\secrets.json");

        private static Dictionary<string, string> _secdic;


        static SecretConfiguration()
        {
            using (var secrets = new StreamReader(path))

            {
                var content = secrets.ReadToEnd();
                _secdic = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            }

            foreach (var VARIABLE in _secdic)
            {
                SimpleLogger.Log("SecretConfiguration.ctor", "Key Found:" + VARIABLE.Key + " : " + VARIABLE.Value);
            }
        }

        public static string Get(string id)
        {
            string target;
            if (_secdic.TryGetValue(id, out target))
            {
                return target;
            }
            throw new ArgumentException("No secret found for key " + id);
        }

    }

    class Secretscontainer
    {
        public List<Secret> Secrets { get; set; }
    }
    internal class Secret
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
