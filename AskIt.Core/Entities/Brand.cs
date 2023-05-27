namespace ATMCompass.Core.Entities
{
    public class Brand
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Wikidata { get; set; }

        public string? Wikipedia { get; set; }

        public virtual ATM? ATM { get; set; }
    }
}
