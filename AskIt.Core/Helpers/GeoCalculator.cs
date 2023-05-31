using ATMCompass.Core.Models.GeoCalculator;
using ATMCompass.Core.Validators;

namespace ATMCompass.Core.Helpers
{
    public static class GeoCalculator
    {
        private static double EarthRadiusInKilometers = 6371.0;
        private const double MetersPerKilometer = 1000.0; // Number of meters in one kilometer

        public static double DegreesToMeters(double distanceInDegrees, double latitude)
        {
            double latRadians = Math.PI * latitude / 180.0;

            double latDegreeLengthInKilometers = 2.0 * Math.PI * EarthRadiusInKilometers * Math.Cos(latRadians);

            double distanceInMeters = distanceInDegrees * latDegreeLengthInKilometers;

            return distanceInMeters;
        }

        private static double GetDistance(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude, int decimalPlaces = 1)
        {
            if (!CoordinateValidator.Validate(originLatitude, originLongitude))
            {
                throw new ArgumentException("Invalid origin coordinates supplied.");
            }

            if (!CoordinateValidator.Validate(destinationLatitude, destinationLongitude))
            {
                throw new ArgumentException("Invalid destination coordinates supplied.");
            }

            return Math.Round(EarthRadiusInKilometers * 2.0 * Math.Asin(Math.Min(1.0, Math.Sqrt(Math.Pow(Math.Sin(originLatitude.DiffRadian(destinationLatitude) / 2.0), 2.0) + Math.Cos(originLatitude.ToRadian()) * Math.Cos(destinationLatitude.ToRadian()) * Math.Pow(Math.Sin(originLongitude.DiffRadian(destinationLongitude) / 2.0), 2.0)))), decimalPlaces);
        }

        public static double GetDistance(Coordinate originCoordinate, Coordinate destinationCoordinate, int decimalPlaces = 1)
        {
            return GetDistance(originCoordinate.Latitude, originCoordinate.Longitude, destinationCoordinate.Latitude, destinationCoordinate.Longitude, decimalPlaces);
        }

        private static double ToRadian(this double d)
        {
            return d * (Math.PI / 180.0);
        }

        private static double DiffRadian(this double val1, double val2)
        {
            return val2.ToRadian() - val1.ToRadian();
        }
    }
}
