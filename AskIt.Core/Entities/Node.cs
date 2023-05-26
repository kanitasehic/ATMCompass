namespace ATMCompass.Core.Entities
{
    public class Node
    {
        public int Id { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        public virtual ATM? ATM { get; set; }
    }
}
