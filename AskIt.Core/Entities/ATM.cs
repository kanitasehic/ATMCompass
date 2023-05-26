namespace ATMCompass.Core.Entities
{
    public class ATM
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public int NodeId { get; set; }

        public virtual Node? Node { get; set; }

        public int? BankId { get; set; }

        public virtual Bank? Bank { get; set; }

        public int? AddressId { get; set; }

        public virtual Address? Address { get; set; }

        public int? BrandId { get; set; }

        public virtual Brand? Brand { get; set; }

        public int? OperatorId { get; set; }

        public virtual Operator? Operator { get; set; }

        public int? CurrencyId { get; set; }

        public virtual Currency? Currency { get; set; }

        public string? Fee { get; set; }

        public string? Phone { get; set; }

        public bool? Wheelchair { get; set; }

        public bool? DriveThrough { get; set; }

        public bool? CashIn { get; set; }

        public bool? Indoor { get; set; }

        public bool? Covered { get; set; }

        public bool? WithinBank { get; set; }

        public string? OpeningHours { get; set; }
    }
}
