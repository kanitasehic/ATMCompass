using System.ComponentModel.DataAnnotations;

namespace ATMCompass.Core.Models.ATMs.Requests
{
    public class AddATMRequest
    {
        [Required]
        public string Lat { get; set; }

        [Required]
        public string Lon { get; set; }

        public bool? IsAccessibleUsingWheelchair { get; set; }

        public string? Name { get; set; }

        public bool? IsDriveThroughEnabled { get; set; }

        public string? Location { get; set; }

        public string? Address { get; set; }

        public string? OpeningHours { get; set; }

        public string? Website { get; set; }
    }
}
