namespace EasyCrudNET.Exceptions
{
    public class EntityMappingException : Exception
    {
        public EntityMappingException(string message, Exception inner) 
            : base($" -> Entity mapping exception. Error: {message}\n", inner) { }

        public EntityMappingException(string message)
            : base($" -> Entity mapping exception. Error: {message}\n") { }
    }
}
