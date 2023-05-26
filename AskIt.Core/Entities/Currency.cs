namespace ATMCompass.Core.Entities
{
    public class Currency
    {
        public int Id { get; set; }

        public bool? BAM { get; set; }

        public bool? EUR { get; set; }

        public bool? USD { get; set; }

        public bool? Others { get; set; }

        public virtual ATM? ATM { get; set; }
    }
}
