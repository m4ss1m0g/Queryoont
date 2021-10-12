# Queryoont

The Queryoont is a _minimal_ framework to use SQL with ASP.NET Core API in a dynamic way, you can issue statement from the client using a POST request, specifying the SELECT and WHERE.

Use the `QueryoontAttribute` to decorate your controller action method to obtain the base [SqlKata](https://sqlkata.com/) Query, next the Queryoont apply the body request filter to the query.

## DISCLAIMER

This tool does not allow to specify joins and INSERT / UPDATE / DELETE statements for security reasons.
Actually, you can **ONLY** specify the `WHERE` and the fields on `SELECT`.

## Setup the Queries

Decorate the controller action method with `HttpPost` and  `QueryoontFilter`

``` csharp
[HttpPost]
[QueryoontFilter]
public Query GetCustomer()
{
    return new Query("Customers");
}
```

## Startup

On the startup configure the `QueryBuilder` class of SqlKata below is a configuration for the SqlServer.

``` csharp

// Configure SQL Server with SqlKata
var connectionString = Configuration.GetConnectionString("ConnectionStringName");
services.AddTransient<IDbConnection>((s) => new SqlConnection(connectionString));
services.AddTransient<QueryFactory>((s) =>
{
    var compiler = new SqlServerCompiler();
    return new QueryFactory(s.GetService<IDbConnection>(), compiler);
});

// Add Queryoont services
services.AddQueryoont();

```

## Request Body on POST

This is a map of all possible conditions.

``` jsonc
{
    "version": 1.0,
    // If you have more that one table use the prefix
    "select": [
        "table1.id",
        "table2.name",
        "table1.{title,note}" // SqlKata syntax
    ],
    "filter": [
        {
            "type": "WhereCondition", // This condition is translated to (Where prefix.Id > 2 OR prefix.Id < 5) between brakets
            "condition": [
                {
                    "type": "Where",
                    "field": "prefix.Id",
                    "oper": ">",
                    "value": "2"
                },
                {
                    "type": "OrWhere",
                    "field": "prefix.Id",
                    "oper": "<",
                    "value": "5"
                }
            ]
        },
        {
            "type": "Where",  // For AND condition repeat the Where
            "field": "prefix.Id",
            "oper": "<",
            "value": "2"
        },
        {
            "type": "OrWhere",
            "field": "prefix.Id",
            "oper": "<",
            "value": "2"
        },
        {
            "type": "WhereNot",
            "field": "prefix.Id",
            "oper": "<",
            "value": "2"
        },
        {
            "type": "OrWhereNot",
            "field": "prefix.Id",
            "oper": "<",
            "value": "2"
        },
        {
            "type": "WhereNull",
            "field": "prefix.Id"
        },
        {
            "type": "OrWhereNull",
            "field": "prefix.Id"
        },
        {
            "type": "WhereTrue",
            "field": "prefix.Id"
        },
        {
            "type": "WhereFalse",
            "field": "prefix.Id"
        },
        {
            "type": "WhereIn",
            "field": "prefix.Id",
            "values": [
                1,
                2,
                3,
                4
            ]
        },
        {
            "type": "WhereNotIn",
            "field": "prefix.Id",
            "values": [
                1,
                2,
                3,
                4
            ]
        }
    ]
}
```

## Example

A simple example

``` sql
CREATE TABLE Table1
(
    [ID] INT NOT NULL PRIMARY KEY,
    [NAME] VARCHAR(50),
    [EMAIL] VARCHAR(100)
)
```

``` csharp
[HttpPost]
[QueryoontFilter]
public Query GetCustomer()
{
    return new Query("table1");
}
```

``` jsonc
{
    "version": 1.0,
    "select": [
        "table1.Name"
    ],
    "filter": [
        {
            "type": "Where",  
            "field": "table.Id",
            "oper": "<",
            "value": "20"
        }
    ]
}
```

This is translated into this SQL

``` SQL
SELECT 
    table1.Name
FROM
    table1
WHERE 
    table.Id < 20
```

And the corresponding json

``` json
[
     {
        "name": "name 1"
     },
     {
        "name": "name 2"
     }
]
```

## Json Serialization

Adding `services.AddQueryoont()` configure the framework to use [Json.NET](https://www.newtonsoft.com/json) to take care of _Serialization_ and _Deserialization_. If you want to customize the process of Serialization/Deserialization you could implement the inserface `IJsonSerializer` of the framework and add it to the _serviceCollection_ using the overload of `services.AddQueryoont()` or by remove the call and adding it to the serviceCollections.

## Know issues with `System.Text.Json`

There are some issues when using System.Text.Json

### Writing

If you use the `System.Text.Json` the default is to serialize on PascalCase the `Dictionary` keys (the result of SqlKata is a `DapperRow` that is a `dynamic` object that is a `Dictionary<string,object>`), if you want the camelCase with `System.Text.Json` you should configure the JsonOptions like this

``` csharp
services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
    );
```

### Reading

When deserialize _object_ properties the `System.Text.Json` adds _ValueKind_ to value property, you must take care of it [See this GiHub issue](https://github.com/dotnet/runtime/issues/31408)
