<h1> EasyCrudNET (Current version 2.0.0)</h1>

<p>EasyCrudNET is a SQL Builder that allows you to interact with a sql server database by creating declarative queries. It also includes a simple entity mapper as complement.</p>

<hr>

<h3><strong>Usage examples:</strong></h3>
<a href='https://www.nuget.org/packages/EasyCrudNET' target="_blank">Repository</a>

<hr>

<h2><strong>Getting started</strong></h2><br>

<span>Get it from <a href='https://github.com/Nicoconte/EasyCrudNET.Examples.git' target="_blank">Nuget Packages</a> or execute any of the following command:</span><br><br>

<span><strong>Package manager</strong></span><br>
```
Install-Package EasyCrudNET -Version x.x.x
```

<span><strong>NET CLI</strong></span><br>
```
dotnet add package EasyCrudNET --version x.x.x
```

<span>Before any use you should create an instance of EasyCrud class and set the Sql Connection by calling "SetSqlConnection" method. (Using this option we can have multiple connections as instances we have)</span>

```C#

using EasyCrudNET;

EasyCrud easyCrud = new EasyCrud();

easyCrud.SetSqlConnection("ConnectionString");

```

<span>Other options is setting up at the Startup.cs (Using this option we only have one connection)</span>

```C#
services.AddSingleton(s =>
{
    var easyCrud = new EasyCrud();
    easyCrud.SetSqlConnection(Configuration.GetSection("ConnectionString").Value);
    return easyCrud;
});
```

<span>In posterior versions (> 1.2.x and > 2.x.x) you can do something like this</span>

```C#
services.AddEasyCrud(o =>
{
    o.UseSqlServerConnection(Configuration["ConnectionString"]);
});
```

<h2><strong>Features</strong></h2> 

<span>EasyCrud contains a lot of method for building queries</span><br><br>
<span>List of methods separated by statement type:</span><br>

<h3><strong>Select</strong></h3>

```C#
public ISelectStatement Select(params string[] columns);
public ISelectStatement From(string tableName);
public ISelectStatement InnerJoin(string tableToRelate);
public ISelectStatement On(string firstRelation, string secondRelation);
```

<h3><strong>Insert</strong></h3>

```C#
public IInsertStatement Insert(params string[] fields);
public IInsertStatement Into(string table);
public IInsertStatement Values(params string[] scalarValues);
```

<h3><strong>Update</strong></h3>

```C#
public IUpdateStatement Update(string tableName);
public IUpdateStatement Set(string column, string scalarVariable);
```

<h3><strong>Delete</strong></h3>

```C#
public IDeleteStatement Delete();
public IDeleteStatement From(string tableName);
```

<h3><strong>SQL Clauses</strong></h3>

```C#
public IClauseStatement GroupBy(string column);
public IClauseStatement Having(string conditionQuery);
public IClauseStatement OrderBy(string column);
public IClauseStatement Desc();
public IClauseStatement Asc();
```

<h3><strong>Conditions/Filters</strong></h3>

```c#
public IConditionStatement Where();
public IConditionStatement Where(string column, string scalarVariable);       
public IConditionStatement Or();
public IConditionStatement Or(string column, string scalarVariable);        
public IConditionStatement And();
public IConditionStatement And(string column, string scalarVariable);
public IConditionStatement Not();        
public IConditionStatement Like(string column, string expression);
public IConditionStatement In(params object[] values);
public IConditionStatement LessThan(string column, string scalarVariable);
public IConditionStatement GreaterThan(string column, string scalarVariable);
public IConditionStatement IsNotNull(string column);
public IConditionStatement IsNull(string column);
public IConditionStatement Between(string column, string firstScalarVariable, string secondScalarVariable);
```

<h3><strong>Database execution methods:</strong></h3>

```C#
public IDatabase DebugQuery(string message = "");

public IDatabase BindValues(object values); //The property name should match with the scalar variable name passed before at the sql builder

public List<List<(string FieldName, object FieldValue)>> GetResult();

public IEnumerable<T> MapResultTo<T>();        
public IEnumerable<T> MapResultTo<T>(Func<List<(string FieldName, object FieldValue)>, T> map);

public IDatabase ExecuteRawQuery(string query);
public IDatabase ExecuteQuery();

public int SaveChangesRawQuery(string query);
public int SaveChanges();

//Set the connection to sql server database
public void SetSqlConnection(string connectionString);
```

<h3><strong>Entity mapper:</strong></h3>

```C#
public class User 
{
    public int Id { get; set; }
    
    public string Username { get; set; }

    public string Password { get; set; }
 
    [Field("CreatedAt")] //Map the entity property 'CreationDate' with the table field 'CreatedAt'
    public DateTime CreationDate { get; set; }
}
```