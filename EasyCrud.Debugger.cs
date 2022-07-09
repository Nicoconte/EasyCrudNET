using EasyCrudNET.Interfaces;

namespace EasyCrudNET
{
    public partial class EasyCrud
    {
        public IDatabase DebugQuery(string message = "")
        {
            var messageFixed = string.IsNullOrWhiteSpace(message) ? _query.ToString() : string.Concat(message, ": ", _query.ToString());

            Console.WriteLine(messageFixed);

            return this;
        }
        public string GetRawQuery()
        {
            return _query.ToString();
        }
    }
}
