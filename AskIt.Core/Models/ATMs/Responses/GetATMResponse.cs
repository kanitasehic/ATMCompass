namespace ATMCompass.Core.Models.ATMs.Responses
{
    public class GetATMResponse
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        public string BankName { get; set; }

        public string City { get; set; }

        public string? Street { get; set; }

        public string? HouseNumber { get; set; }

        public bool? Wheelchair { get; set; }

        public bool? DriveThrough { get; set; }

        public bool? CashIn { get; set; }

        public bool? Indoor { get; set; }

        public bool? Covered { get; set; }

        public bool? WithinBank { get; set; }

        public string? OpeningHours { get; set; }

        public double? Distance { get; set; }
    }
}
