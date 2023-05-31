namespace ATMCompass.Core.Models.ATMs.Requests
{
    public class GetCannibalATMsRequest
    {
        public string CenterLat { get; set; }

        public string CenterLon { get; set; }

        public string BankName { get; set; }

        public double RadiusInKilometers { get; set; }
    }
}
