namespace CoinMapApiTools.Console
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using Newtonsoft.Json;

    public class Program
    {
        public static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        public void Run()
        {
            VenueList venues = GetVenues();
        }

        private VenueList GetVenues()
        {
            WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings["url"]);

            using (WebResponse response = request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (var streamReader =
                new StreamReader(responseStream ?? throw new ArgumentNullException(nameof(responseStream))))
            {
                string venues = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<VenueList>(venues);
            }
        }
    }
}