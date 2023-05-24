namespace AskIt.Core.Exceptions
{
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException()
        {
        }

        public NotAuthenticatedException(string message)
            : base(message)
        {
        }
    }
}
