namespace ATMCompass.Core.Entities
{
    public class ATM
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public int NodeId { get; set; }

        public Node Node { get; set; }

        public string? BankName { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        public bool? Wheelchair { get; set; }

        public bool? DriveThrough { get; set; }

        public bool? CashIn { get; set; }

        public bool? Indoor { get; set; }

        public bool? Covered { get; set; }

        public bool? WithinBank { get; set; }

        public string? OpeningHours { get; set; }
    }
}
