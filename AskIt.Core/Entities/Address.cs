namespace ATMCompass.Core.Entities
{
    public class Address
    {
        public int Id { get; set; }

        public string? City { get; set; }

        public string? Street { get; set; }

        public string? HouseNumber { get; set; }

        public string? Postcode { get; set; }

        public virtual ATM? ATM { get; set; }
    }
}
