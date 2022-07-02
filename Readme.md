<h1> EasyCrudNET </h1>

<p>EasyCrudNET is a SQL builder and wrapper for ADO.NET that allows you to build queries declaratively and makes it easy to manipulate sql server databases.</p>

<hr>

<h3>Demo project: https://github.com/Nicoconte/OpenPersonalBudget.API/tree/easycrudnet-demo </h3>
<p>I replaced entity framework for EasyCrudNET without any problems! Check it out.</p>

<hr>

<h3>Example and usage</h3>

<br>
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
</table>

<h4>Table Operations</h4>
<table>
    <tr>
        <td>Id</td>
        <td>NVARCHAR</td>
    </tr>
    <tr>
        <td>UserId</td>
        <td>NVARCHAR</td>
    </tr>    
    <tr>
        <td>Description</td>
        <td>NVARCHAR</td>
    </tr>    
    <tr>
        <td>Amount</td>
        <td>INT</td>         
    </tr>    
    <tr>
        <td>OperationType</td>  
        <td>INT</td>  
    </tr>    
    <tr>
        <td>PaymentType</td>  
        <td>INT</td>                  
    </tr>    
    <tr>
        <td>Category</td>                  
        <td>INT</td>         
    </tr>    
    <tr>
        <td>CreatedAt DATETIME</td>         
        <td>DATETIME</td>        
    </tr>    
</table>

```c#
using EasyCrudConsole.Entities;
using EasyCrudConsole.Utils;
using EasyCrudNET;
using EasyCrudNET.Configuration;

string connectionString = "Data Source={Your host};Initial Catalog={Your database};Trusted_Connection=True;MultipleActiveResultSets=true;";


var easyCrud = new EasyCrud();

easyCrud.SetSqlConnection(connectionString);

easyCrud
  .Select("Id", "UserId", "Description", "Amount")
  .From("Operations")
  .Where("Id", "@id")
  .Execute<Operations>(new
  {
    id = "7bd328dd-c9f8-4f4a-ba65-e3608e2731dd"
  }); //Return a list of operations by id

easyCrud
  .Select("*")
  .From("Operations")
  .Where()
  .Like("Description", "%dev%")
  .Execute<Operations>(); // return a list of operations where description contains 'dev'

var op = easyCrud
    .Select("*")
    .From("Operations O")
    .InnerJoin("Users U")
    .On("O.UserId", "U.Id")
    .Where("O.UserId", "@id")
    .DebugQuery()
    .Execute<Operations>(new
    {
        id = "439200c8-df14-481c-935c-fd82f9c4a538"
    })
    .FirstOrDefault(); // return the first operation from operation list

//----------------------------------

easyCrud
    .Insert()
    .Into("Users")
    .Values("@id", "@username", "@email","@password", "@date")
    .DebugQuery("Custom message before showing the query")
    .Execute(new
    {
        id = Guid.NewGuid().ToString(),
        username = "foo",
        email = "foo@gmail.com",
        password = PasswordUtil.Hash("superSecret"),
        date = DateTime.Now
    }); //Insert user and return rows affected

var user = easyCrud
    .Select("*")
    .From("Users")
    .Where()
    .Like("Username", "@username")
    .Execute<User>(new
    {
        username = "foo"
    })
    .FirstOrDefault(); // return the user created before

easyCrud
    .Update("Users")
    .Set("Username", "@newUser")
    .Set("Email", "@newEmail")
    .Where("Id", "@id")
    .Execute(new
    {
        newUser = "foobar",
        newEmail = "foobar@gmail.com",
        id = user?.Id
    }); // Update the user created before. It returns the rows affected

easyCrud
    .Delete()
    .From("Users")
    .Where()
    .Like("Id", "@id")
    .Execute(new 
    {
        id = user?.Id    
    }); //Delete user by ID

```

<h3>Another example </h3>
<br>
<h4>Table Files</h4>
<table>
    <tr>
        <td>Id</td>
        <td>NVARCHAR</td>
    </tr>
    <tr>
        <td>UserId</td>
        <td>NVARCHAR</td>
    </tr>    
    <tr>
        <td>Filename</td>
        <td>NVARCHAR</td>
    </tr>    
    <tr>
        <td>Path</td>
        <td>NVARCHAR</td>         
    </tr>    
    <tr>
        <td>Extension</td>  
        <td>NVARCHAR</td>  
    </tr>    
    <tr>
        <td>Size</td>  
        <td>INT</td>                  
    </tr>    
    <tr>
        <td>CreatedAt DATETIME</td>         
        <td>DATETIME</td>        
    </tr>    
</table>

```c#

string connectionString = "Data Source={Your host};Initial Catalog={Your database};Trusted_Connection=True;MultipleActiveResultSets=true;";

var easyCrud = new EasyCrud();

easyCrud.SetSqlConnection(connectionString);


//Execute only a query
var filesWithoutScalarAndRawQuery = easyCrud.Execute<UserFiles>(null, "select * from files");

//Execute query with scalar variables inside. We should pass the values as first parameter
var filesWithtScalarAndRawQuery = easyCrud.Execute<UserFiles>(new
{
    id = "390d6d03-d1af-4c9f-8c3e-94e256730e0e"
}, "select * from files where UserId=@id");


//We also could get the sqlreader object in order to manipulate the data
//based on our needs
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

easyCrud
    .Insert()
    .Into("Files")
    .Values("@id", "@userId", "@filename", "@path", "@extension", "@size", "@date")
    .Execute(new
    {
        id = Guid.NewGuid().ToString(),
        userId = Guid.NewGuid().ToString(),
        filename = "test.pdf",
        path = "C:/bar/foo/test.pdf",
        extension = ".pdf",
        size = 120,
        date = DateTime.Now
    });

//In case we pass a query as second parameter,
//it wont be able to execute because we already have a query built with the sql builder
easyCrud
    .Insert()
    .Into("Files")
    .Values("@id", "@userId", "@filename", "@path", "@extension", "@size", "@date")
    .Execute(new
    {
        id = Guid.NewGuid().ToString(),
        userId = Guid.NewGuid().ToString(),
        filename = "test.pdf",
        path = "C:/bar/foo/test.pdf",
        extension = ".pdf",
        size = 120,
        date = DateTime.Now
    }, "insert into Files values ('a', 'b', 'c', 'd', 'f', 1, GETDATE())");

//Null type checking
easyCrud
    .Select("*")
    .From("Files")
    .Where()
    .IsNull("Filename")
    .DebugQuery()
    .Execute<UserFiles>()
    .FirstOrDefault();

easyCrud
    .Select("*")
    .From("Files")
    .Where()
    .IsNotNull("Filename")
    .DebugQuery()
    .Execute<UserFiles>()
    .Count(); //return 3 files. 4 in total, one of them has a null value
```

<h3>Mapping example</h3>
<p>We also have a basic mapping that allows us to map a specific class against the columns of the Table</p>

```c#

//UserFiles.cs
//We only map the properties we need
public class UserFiles
{
    public string Id { get; set; }
    
    [ColumnMapper("UserId")] //By default it contains ColumnAction.Map after the column name
    public string OwnerId { get; set; }
    public string Filename { get; set; }
    public string Path { get; set; }
    public string Extension { get; set; }
    public int Size { get; set; }
    public DateTime CreatedAt { get; set; }
}

//Program.cs
var easyCrud = new EasyCrud(connection);

var res = easyCrud
    .Select("*")
    .From("Files")
    .DebugQuery()
    .Execute<UserFiles>();


//User.cs
//We can map all the properties and set what we should ignore
public class Users
{
    [ColumnMapper("Id")]
    public string UserId { get; set; }

    [ColumnMapper(ColumnAction.Ignore)]
    public string Username { get; set; }

    [ColumnMapper("Password")]
    public string SecretPassword { get; set; }

    [ColumnMapper("CreatedAt")]
    public DateTime FechaDeCreacion { get; set; }
}

//Program.cs
string connectionString = "Data Source={Your host};Initial Catalog={Your database};Trusted_Connection=True;MultipleActiveResultSets=true;";

var easyCrud = new EasyCrud();

easyCrud.SetSqlConnection(connectionString);

var users = easyCrud
    .Select("*")
    .From("users")
    .Execute<Users>();

//Clientes.cs
//Complete map
public class Clientes
{
    [ColumnMapper("cliente_num")]
    public int Id { get; set; }

    [ColumnMapper("nombre")]
    public string Nombre { get; set; }

    [ColumnMapper("Apellido")]
    public string Apellido { get; set; }

    [ColumnMapper("empresa")]
    public string Empresa { get; set; }

    [ColumnMapper("domicilio")]
    public string Domicilio { get; set; }

    [ColumnMapper("ciudad")]
    public string Ciudad { get; set; }

    [ColumnMapper("provincia_cod")]
    public string CodigoProvincia { get; set; }

    [ColumnMapper("codPostal")]
    public string CodigoPostal { get; set; }

    [ColumnMapper("telefono")]
    public string Telefono { get; set; }

    [ColumnMapper("cliente_ref")]
    public int ReferenciaCliente { get; set; }

    [ColumnMapper("estado")]
    public string Estado { get; set; }
}

//Program.cs
var easyCrud = new EasyCrud(connection);

var clientes = easyCrud
    .Select("*")
    .From("clientes")
    .DebugQuery()
    .Execute<Clientes>();
```