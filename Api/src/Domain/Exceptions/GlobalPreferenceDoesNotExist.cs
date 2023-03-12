namespace Domain.Exceptions
{
    public class GlobalPreferenceDoesNotExist : Exception
    {
        public GlobalPreferenceDoesNotExist(string message) : base(message)
        {
        }
    }
}
