namespace EasyCrudNET.Resources
{
    public class Messages
    {
        private static Dictionary<string, string> _messages = new Dictionary<string, string>()
        {
            { "BaseError", "Error: " },
            { "BaseErrorWithMsg", "Error: {0}" },
            { "EmptyQueryError", "Cannot perform operation. Query is empty" },
            { "CannotMapResultError", "There no result to map" },
            { "NotEnoughArgsToMapError", "Not enough parameters provided to perform scalar variable mapping"},
            { "NotEnoughParameterError", "Not enough parameter for the SQL query. Maybe something is missing/empty" },
            { "ConditionQueryError", "Condition not provided. It require for 'BETWEEN' clause" },
            { "DatabaseError", "Something fail during execution" },
            { "CannotExecuteOpError", "This method will only execute with insert/delete/update statement"},
            { "EmptyConnectionStringError", "Connection string has not been initialized" },
            { "TableNotProvidedError", "Table name not provided or it's empty" },
            { "ColumnNotProvidedError", "Column name not provided or it's empty" },
            { "EntityMappingError", "Failed to map column '{0}'. Check if in the ColumnMapper attribute the column is well written" },
            { "MapError", "Cannot cannot map entity {0}. Reason {1}" },
            { "QueryAlreadyBuiltError", "Query built with sql builder is not allowed in this method" }
        };

        public static string Get(string key)
        {
            return _messages[key];
        }
    }
}
