namespace ATMCompass.Core.Entities
{
    public class Transport
    {
        public int Id { get; set; }

        public int NodeId { get; set; }

        public Node Node { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        public string? Name { get; set; }

        public string Type { get; set; }
    }
}
