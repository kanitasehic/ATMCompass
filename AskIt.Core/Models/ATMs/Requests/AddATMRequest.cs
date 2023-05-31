using System.ComponentModel.DataAnnotations;

namespace ATMCompass.Core.Models.ATMs.Requests
{
    public class AddATMRequest
    {
        [Required]
        public double Lat { get; set; }

        [Required]
        public double Lon { get; set; }

        [Required]
        public string BankName { get; set; }

        [Required]
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
    }
}
