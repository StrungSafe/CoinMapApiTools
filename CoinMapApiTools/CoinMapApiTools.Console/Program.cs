using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace CoinMapApiTools.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        public void Run()
        {
            var venues = GetVenues();
        }

        private string GetVenues()
        {
            var request = WebRequest.Create(ConfigurationManager.AppSettings["url"]);

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var streamReader = new StreamReader(responseStream ?? throw new ArgumentNullException(nameof(responseStream))))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}