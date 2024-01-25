namespace Application.Common.ErrorMessages
{
    public class PropertyErrorMessages
    {
        public static string Required(string name)
        {
            return $"{name} is required";
        }

        public static string InvalidValue(string name, string value)
        {
            return $"The value '{value}' is not valid for {name}.";
        }
    }
}