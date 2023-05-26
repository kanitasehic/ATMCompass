namespace ATMCompass.Core.Models.ATMs.Requests
{
    public class GetATMsRequest
    {
        public double CurrentLat { get; set; }

        public double CurrentLon { get; set; }

        public string? BankName { get; set; }

        public string? Location { get; set; }

        public bool? IsAccessibleUsingWheelchair { get; set; }

        public bool? IsDriveThroughEnabled { get; set; }
    }
}
