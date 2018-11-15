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
            try
            {
                Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Exit();
        }

        private static void Exit()
        {
            Console.WriteLine("Press <enter> to exit.");
            Console.ReadLine();
        }

        private static Uri GetUri()
        {
            return new Uri(ConfigurationManager.AppSettings["uri"]);
        }

        private static VenueDetails GetVenue(int venueId)
        {
            var uri = new Uri(GetUri(), venueId.ToString());

            string venue = HttpGet(uri);

            return JsonConvert.DeserializeObject<VenueDetails>(venue);
        }

        private static void GetVenueDetails(VenueList venues, List<VenueDetails> venueDetailsList)
        {
            int venuesLength = venues.Venues.Length;

            for (var index = 0; index < venuesLength; index++)
            {
                Console.WriteLine($"Getting venue details {index + 1} of {venuesLength}...");

                VenueDetails venueDetails = GetVenue(venues.Venues[index].Id);

                if (venueDetails == null)
                {
                    continue;
                }

                venueDetailsList.Add(venueDetails);
            }
        }

        private static VenueList GetVenues()
        {
            string venues = HttpGet(GetUri());

            return JsonConvert.DeserializeObject<VenueList>(venues);
        }

        private static string HttpGet(Uri uri)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        private static void Run()
        {
            var venueDetailsList = new List<VenueDetails>();

            VenueList venues = GetVenues();

            if (venues == null)
            {
                Console.WriteLine("No venues returned.");
                return;
            }

            GetVenueDetails(venues, venueDetailsList);
            WriteVenuesToFile(venueDetailsList.ToArray());
        }

        private static void WriteVenuesToFile(VenueDetails[] venues)
        {
            string venuesCsv = $"Id,Name,Email,Country{Environment.NewLine}";

            foreach (VenueDetails venueDetails in venues)
            {
                Venue venue = venueDetails.Venue;
                venuesCsv += $"{venue.Id},{venue.Name},{venue.Email},{venue.Country}{Environment.NewLine}";
            }

            string directory = ConfigurationManager.AppSettings["output"];

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string path = Path.Combine(directory, "Venues.csv");

            File.WriteAllText(path, venuesCsv);
        }
    }
}