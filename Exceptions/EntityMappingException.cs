namespace EasyCrudNET.Exceptions
{
    public class EntityMappingException : Exception
    {
        public EntityMappingException(string message, Exception inner) 
            : base($"\n-> Entity mapping exception. Error: {message}\n", inner) { }

        public EntityMappingException(string message)
            : base($"\n-> Entity mapping exception. Error: {message}\n") { }
    }
}
