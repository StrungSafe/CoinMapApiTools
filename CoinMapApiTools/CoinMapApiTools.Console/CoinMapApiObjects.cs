namespace CoinMapApiTools.Console
{
    public class VenueList
    {
        public Venue[] Venues { get; set; }
    }

    public class Venue
    {
        public string Country { get; set; }

        public string Email { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}