namespace CoinMapApiTools.Console
{
    using System;
    using System.Collections.Generic;
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

            var venuesWithEmail = new List<Venue>();

            foreach (Venue venue in venues.Venues)
            {
                Venue venueDetail = GetVenue(venue.Id);

                if (string.IsNullOrWhiteSpace(venueDetail.Email))
                {
                    continue;
                }

                venuesWithEmail.Add(venueDetail);
            }

            WriteVenuesToFile(venuesWithEmail.ToArray());
        }

        private Uri GetUri()
        {
            return new Uri(ConfigurationManager.AppSettings["uri"]);
        }

        private Venue GetVenue(int venueId)
        {
            var uri = new Uri(GetUri(), venueId.ToString());

            string venue = HttpGet(uri);

            return JsonConvert.DeserializeObject<Venue>(venue);
        }

        private VenueList GetVenues()
        {
            string venues = HttpGet(GetUri());

            return JsonConvert.DeserializeObject<VenueList>(venues);
        }

        private string HttpGet(Uri uri)
        {
            WebRequest request = WebRequest.Create(uri);

            using (WebResponse response = request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (var streamReader =
                new StreamReader(responseStream ?? throw new ArgumentNullException(nameof(responseStream))))
            {
                return streamReader.ReadToEnd();
            }
        }

        private void WriteVenuesToFile(Venue[] venues)
        {
            string venuesCsv = string.Empty;

            foreach (Venue venue in venues)
            {
                venuesCsv += $"{venue.Id},{venue.Name},{venue.Email},{venue.Country}{Environment.NewLine}";
            }

            File.WriteAllText(ConfigurationManager.AppSettings["output"], venuesCsv);
        }
    }
}