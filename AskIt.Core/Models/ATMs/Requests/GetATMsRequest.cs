namespace ATMCompass.Core.Models.ATMs.Requests
{
    public class GetATMsRequest
    {
        public double? CurrentLat { get; set; }

        public double? CurrentLon { get; set; }

        public string? BankName { get; set; }

        public string? Location { get; set; }

        public bool? Wheelchair { get; set; }

        public bool? DriveThrough { get; set; }

        public bool? CashIn { get; set; }

        public bool? Indoor { get; set; }

        public bool? Covered { get; set; }

        public bool? WithinBank { get; set; }
    }
}
