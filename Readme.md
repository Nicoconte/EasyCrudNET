<h1> EasyCrudNET (Current version 1.2.1)</h1>

<p>EasyCrudNET is a SQL Builder that allows you to interact with a sql server database by creating declarative queries. It also includes a simple class mapper as complement.</p>

<hr>

<h3>Demo project: https://github.com/Nicoconte/OpenPersonalBudget.API/tree/easycrudnet-demo </h3>
<p>It's a simple CRUD project but see how someone can replace Entity Framework with EasyCrudNET without any problems! Check it out.</p>

<hr>

<h2><strong>Getting started</strong></h2><br>

<span>Get it from <a href='https://www.nuget.org/packages/EasyCrudNET' target="_blank">Nuget Packages</a> or execute any of the following command:</span><br><br>

<span><strong>Package manager</strong></span><br>
```
Install-Package EasyCrudNET -Version 1.1.1
```

<span><strong>NET CLI</strong></span><br>
```
dotnet add package EasyCrudNET --version 1.1.1
```

<span>Before any use you should create an instance of EasyCrud class and set the Sql Connection by calling "SetSqlConnection" method. (Using this option we can have multiple connections as instances we have)</span>

```C#

using EasyCrudNET;

EasyCrud easyCrud = new EasyCrud();

easyCrud.SetSqlConnection("Your string connection goes here");

```

<span>Other options is setting up at the Startup.cs (Using this option we only have one connection)</span>

```C#
services.AddSingleton(s =>
{
    var easyCrud = new EasyCrud();
    easyCrud.SetSqlConnection(Configuration.GetSection("ConnString").Value);
    return easyCrud;
});
```

<span>In posterior versions (> 1.2.x) you can do something like this</span>

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

```c#
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

<h3><strong>Database method</strong></h3>

```C#
//Show the query built. You set a custom message before query. Ex: "NASHE: your query"
public IDatabase DebugQuery(string message = "");

//Return the query built
public string GetRawQuery();

//Execute the query and return a SQL Reader object
public SqlDataReader ExecuteAndGetReader(object values=null, string query="");
public Task<SqlDataReader> ExecuteAndGetReaderAsync(object values=null, string query="");

//Execute INSERT/DELETE/UPDATE query and return the rows affected
public Task<int> ExecuteAsync(object values=null, string query="");
public int Execute(object values=null, string query="");

//Execute SELECT query and return a T object
public Task<IEnumerable<T>> ExecuteAsync<T>(object values=null, string query="") where T : class, new();
public IEnumerable<T> Execute<T>(object values=null, string query="") where T : class, new();

//Set the connection to sql server database
public void SetSqlConnection(string connectionString);
```
<h3><strong>NOTES</strong></h3>
<span>When you pass the values to any "Execute" method, the property name should match with the scalar variable name.<br>Ex:</span>

```C#
var user = easyCrud
    .Select("*")
    .From("Users")
    .Where()
    .Like("Username", "@username") //scalar variable is "username"
    .Execute<User>(new
    {
        username = "foo" //So when we pass the value, the property name of the anonymous object should be exactly the same to scalar variable name. It's case sensitive
    })
    .FirstOrDefault();
```

<span>Another thing to keep in mind is that the second parameter of the "Execute" method can receive a Query already built before. It is an alternative in case you do not want to build the query with the sql builder and execute a query directly.<br>Ex:</span>

```C#
//Execute a query directly
easyCrud.Execute<UserFiles>(null, "select * from files");

//Execute query directly but with scalar variables inside. We should pass the values as first parameter
easyCrud.Execute<UserFiles>(new
{
    id = "390d6d03-d1af-4c9f-8c3e-94e256730e0e"
}, "select * from files where UserId=@id");

//Another example
easyCrud.Execute(new {
    id = Guid.NewGuid().ToString(),
    username = "foo",
    email = "foo@gmail.com",
    password = "superSecret",
    date = DateTime.Now
}, "insert into users values (@id, @username, @email, @password, @date)")


string id = "11111";
string username = "foooo";
string email = "fooooo@baaaaaar.com";
string password = "123"
DateTime creationDate = DateTime.now;

easyCrud.Execute(null, $"insert into users values ('{id}, '{username}', '{email}', '{password}', '{creationDate}')")
```

<span>Lastly, you can get the sql reader object and manage the result as you please. Do not forget to close the connection!<br>Ex:</span>

```C#
var readerWithRawQuery = easyCrud.ExecuteAndGetReader(null, "select * from files");
List<UserFiles> filesWithRawQuery = new List<UserFiles>();

while (readerWithRawQuery.Read())
{
    filesWithRawQuery.Add(new UserFiles()
    {
        Id = readerWithRawQuery[0].ToString(),
        UserId = readerWithRawQuery[1].ToString(),
        Filename = readerWithRawQuery[2].ToString(),
        Path = readerWithRawQuery[3].ToString(),
        Extension = readerWithRawQuery[4].ToString(),
        Size = int.Parse(readerWithRawQuery[5].ToString()),
        CreatedAt = DateTime.Parse(readerWithRawQuery[6].ToString()),
    });
}
readerWithRawQuery.Close();
```

<br>
<h3><strong>Class Mapper</strong></h3>
<span>When executing a select query, easy crud takes care of mapping the response from the database to a data type T. To do this, the properties of your class must match with the name of the table column or use the <strong>ColumnMapper</strong> attribute in order to set the table column name for that class property.</span><br>

<br>
<h3><strong>Class Mapper usage</strong></h3><br>

<span>Image you have the following table:</span>
<h4>Table Users</h4>
<table>
    <tr>
        <td>Id</td>
        <td>NVARCHAR</td>
    </tr>
    <tr>
        <td>Username</td>
        <td>NVARCHAR</td>         
    </tr>    
    <tr>
        <td>Email</td>
        <td>NVARCHAR</td>
    </tr>    
    <tr>
        <td>Password</td>
        <td>DATETIME</td>         
    </tr>    
</table><br>

<span>And your class looks like something like this</span>

```c#
public class User
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string SecretPassword { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

<span>The properties of your class doesnt match table column name so when you execute a select query it'll fail because the mapping. To avoid that error we can:</span>

<span>1- Change the property name to the table column name</span>

```c#
public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
}
```
  
<span>2- Use <strong>ColumnMapper</strong> attribute and set the table column name</span>

```c#
public class User
{
    //As second parameter it receives a ColumnAction. ColumnAction.Map (Map property) or ColumnAction.Ignore (Ignore property from mapping)
    [ColumnMapper("Id")]
    public string UserId { get; set; }

    public string Username { get; set; }
    public string Email { get; set; }
        
    [ColumnMapper("Password")]
    public string SecretPassword { get; set; }

    public DateTime CreatedAt { get; set; }
}
```    

<span>Map all the properties or not is totally optional and depends on your needs</span>