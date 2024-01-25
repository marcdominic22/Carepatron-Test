namespace Application.Common.ErrorMessages
{
    public class EntityErrorMessages
    {
        public static string DoesNotExist(string key)
        {
            return $"{key} does not exist";
        }

        public static string NoDataExists(string property, string key)
        {
            return $"No {property} Data for {key}";
        }

        public static string AgencyDoesNotMatch(string currUserAgency, string AgencyNo)
        {
            return $"Agency does not match user Agency: {currUserAgency} expected Agency: {AgencyNo}";
        }

        public static string InvalidPropertyValue(string key, string property, string value)
        {
            return $"{key} {property} value: {value} is invalid";
        }

        public static string NoAvailableData(string property, string targetProperty)
        {
            return $"No {property} available to the {targetProperty}";
        }
    }
}