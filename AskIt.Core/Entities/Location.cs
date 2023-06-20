using Microsoft.SqlServer.Types;

namespace ATMCompass.Core.Entities
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Geometry { get; set; }
    }
}
