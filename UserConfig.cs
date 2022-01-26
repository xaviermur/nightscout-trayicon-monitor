using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightscoutTrayIconMonitor
{
    internal class UserConfig
    {
        public UserConfigData Data;
        public UserConfig()
        {
            Data = new UserConfigData();
        }

        public UserConfig(string _server, string _token, string _interval, string _units)
        {
            Data = new UserConfigData();
            if (!int.TryParse(_interval, out int _intervalAux))
                _intervalAux = -1;
            Data.Server = _server;
            Data.Token = _token;
            Data.Interval = _intervalAux;
            Data.Units = _units;
        }
        public void Save()
        {
            StreamIO.Save(Data);
        }
        public void Read()
        {
            Data = StreamIO.Read();
        }

    }

    public class UserConfigData
    {
        public string Server { get; set; }
        public string Token { get; set; }
        public int Interval { get; set; }
        public string Units { get; set; }
    }

    public static class StreamIO
    {
        public static void Save(UserConfigData data)
        {
            using (StreamWriter outputFile = new StreamWriter("userdata.config"))
            {
                outputFile.Write(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            }
        }
        public static UserConfigData Read()
        {
            try
            {
                using (StreamReader outputFile = new StreamReader("userdata.config"))
                {
                    // Read the stream as a string, and write the string to the console.
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<UserConfigData>(outputFile.ReadToEnd());
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return new UserConfigData();
            }
        }
    }
}
