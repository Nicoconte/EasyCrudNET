<h1> EasyCrudNET (Current version 2.1.0)</h1>

<p>EasyCrudNET is a SQL Builder that allows you to interact with sql server database by creating declarative queries. It also includes a simple entity mapper as complement.</p>

<hr>

<h2><strong>Getting started</strong></h2><br>

<span>Get it from <a href='https://www.nuget.org/packages/EasyCrudNET' target="_blank">Nuget Packages</a> or execute any of the following command:</span><br><br>

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

public IDatabase BindValues(object values);

public List<List<(string FieldName, object FieldValue)>> GetResult();

public IEnumerable<T> MapResultTo<T>();        
public IEnumerable<T> MapResultTo<T>(Func<List<(string FieldName, object FieldValue)>, T> map);

public IDatabase ExecuteQuery();

public int SaveChanges();

public void SetSqlConnection(string connectionString);
```

<h3><strong>Entity mapper:</strong></h3>

```C#
public class User 
{
    public int Id { get; set; }
    
    public string Username { get; set; }

    public string Password { get; set; }
 
    [Field("CreatedAt")] 
    public DateTime CreationDate { get; set; }
}
```

<hr>

<h3><strong>Examples:</strong></h3>

<h4><strong>Select data from database using sql builder:</strong></h4>

```c# 

easyCrud
    .Select("*") //SELECT *
    .From("Users") //FROM Users
    .Where("username", "@username") //WHERE username=@username
    .And("password", "@password") // AND password=@password
    .BindValues(new 
    {
        username = "foo", //property name should match with scalar variable name
        password = "bar"
    })
    .ExecuteQuery() //Require method
    .GetResult(); //Require output method

//The GetResult method will return an list of lists where each list represents a row/record
//and the tuples will represent the field name with its value
```

<h4><strong>EasyCrudNET provide a feature to map the query result to T.</strong></h4>

```c#
easyCrud
    .Select("*")
    .From("Users")
    .Where("username", "@username")
    .And("password", "@password")
    .BindValues(new 
    {
        username = "foo",
        password = "bar"
    })
    .ExecuteQuery()
    .MapResultTo<Users>(); //Return a IEnumerable of Users

```

<h4><strong>You can map the result to T by yourself.</strong></h4>

```c#
easyCrud
    .Select("*")
    .From("Users")
    .Where("username", "@username")
    .And("password", "@password")
    .BindValues(new 
    {
        username = "foo",
        password = "bar"
    })
    .ExecuteQuery()
    .MapResultTo<Users>(res => 
    {
        return new Users()
        {
            Id = int.Parse(c[0].FieldValue.ToString()),
            Name = c[1].FieldValue.ToString(),
            Password = c[2].FieldValue.ToString(),
            CreationDate = DateTime.Parse(c[3].FieldValue.ToString()),
        };        
    });

//Notes:
//We can pass into MapResultTo<T> method a callback where the  
//res is a list of tuples(FieldName, FieldValue).
```

<h4><strong>Notes:</strong></h4>
<h4>In order to map an entity, the property name should match with the fieldname or use the 'Field' attribute. Ex:</h4>

```c#

public class Users
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }

    [Field("CreatedAt")] //Map the property 'CreationDate' with field 'CreatedAt'
    public DateTime CreationDate { get; set; }
}

```

<h4><strong>Select data from database using raw queries</strong></h4>

```C#

easyCrud
    .FromSql("select * from Users")
    .ExecuteQuery()
    .GetResult();

easyCrud
    .FromSql("select * from Users")
    .ExecuteRawQuery()
    .MapResultTo<Users>();

easyCrud
    .FromSql("select * from Users where username=@username and password=@password")
    .BindValues(new 
    {
        username = "foo",
        password = "bar"        
    })
    .ExecuteQuery()
    .GetResult();

easyCrud
    .FromSql("select * from Users where username=@username and password=@password")
    .BindValues(new 
    {
        username = "foo",
        password = "bar"        
    })
    .ExecuteQuery()
    .MapResultTo<Users>();    

```

<h4><strong>Modify database using sql builder</strong></h4>

``` c#

easyCrud
    .Insert()
    .Into("Users")
    .Values("@username", "@password", "@date")
    .BindValues(new 
    {
        username = "foo",
        password = "bar",
        date = DateTime.Now
    })
    .SaveChanges(); //Return rows affected

easyCrud
    .Update("Users")
    .Set("username", "@username")
    .Set("password", "@password")
    .BindValues(new 
    {
        username = "foo",
        password = "bar"
    })
    .SaveChanges();     

easyCrud
    .Delete()
    .From("Users")
    .Where("Id", "@id")
    .BindValues(new 
    {
        id = 1,
    })
    .SaveChanges();        

```

<h4><strong>Modify database using raw queries</strong></h4>

```C#

easyCrud
    .FromSql("delete from Users")
    .SaveChanges(); //lol

easyCrud
    .FromSql("insert into Users values (@username, @password, @date)")
    .BindValues(new 
    {
        username = "foo",
        password = "bar",
        date = DateTime.Now
    })
    .SaveChanges();

easyCrud
    .FromSql($"update users set username='{"foodoeee"}' where id=@id")
    .BindValues(new
    {
        id = 1
    })
    .SaveChanges();

```

<h4><strong>Execute transactions</strong></h4>

```c#
var success = easyCrud
    .BeginTransaction((queries) =>
    {
        queries.Add(("insert into Users values (@username1, @password1, @date1)", new
        {
            username1 = "foor",
            password1 = "barrr",
            date1 = DateTime.Now,
        }));
        
        queries.Add(("insert into Users values (@username2, @password2, @date2)", new
        {
            username2 = "faaas",
            password2 = "buuur",
            date2 = DateTime.Now,
        }));    

        queries.Add(("insert into facturas values ('fooss', 'sass', GETDATE())", null));
    })
    .Commit(); //Return a boolean

```

<a href='https://github.com/Nicoconte/EasyCrudNET.Examples.git' target="_blank">More examples</a>
