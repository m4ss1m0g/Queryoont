# Queryoont

The Queryoont is a _minimal_ framework to use SQL with ASP.NET Core API in a dynamic way, you can issue statement from the client using a POST request, specifying the SELECT and WHERE.

The Queryoont uses the mediator pattern with [MediatR](https://github.com/jbogard/MediatR), you should create a request command and a request handler to obtain the base [SqlKata](https://sqlkata.com/) Query, next the middleware grab the query and apply the body request filter and select to the query.

## DISCLAIMER

This tool does not allow to specify joins and INSERT / UPDATE / DELETE statements for security reasons.
Actually, you can **ONLY** specify the `WHERE` and the fields on `SELECT`.

## Setup the Queries

Create a MediatR command request

``` csharp
public class MyQuery : IRequest<Query>
{
    
}
```

Create the corresponding handler

``` csharp
public class MyQueryHandler : IRequestHandler<MyQuery, Query>
{
    public async Task<Query> Handle(MyQuery request, CancellationToken cancellationToken)
    {
        var query = new Query("table").Where("Id", ">", 10);
        return await Task.FromResult(query);
    }
}
```

Decorate the controller action with the `Queryoont` attribute and setup the name of the command

```csharp
[HttpPost]
[Queryoont(typeof(MyQuery))]
public Query GetRecords()
{
    throw new NotImplementedException();
}
```

## Startup

On the startup configure the QueryBuilder and MediatR, below is a configuration for the SqlServer.
This setup is left to the user to permit the maximum level of flexibility.

``` csharp
public static IServiceCollection AddQueryoontServices(this IServiceCollection services, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("ConnectionStringName");
    
    services.AddTransient<IDbConnection>((s) => new SqlConnection(connectionString));
    services.AddTransient<QueryFactory>((s) =>
    {
        var compiler = new SqlServerCompiler();
        return new QueryFactory(s.GetService<IDbConnection>(), compiler);
    });

    // Configure the MediatR based on the assembly containing the Query
    services.AddMediatR(typeof(Startup));

    return services;
}
```

Next you should configure the app to use the Middleware and the services

``` csharp
public void ConfigureServices(IServiceCollection services)
{
    // See above: You should register the QueryFactory and MediatR queries
    services.AddQueryoontServices(Configuration);
    
    // This register the internal classes
    services.AddQueryoont(); 
}
```

``` csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // [..]

    app.UseRouting();

    app.UseAuthorization();

    app.UseQueryoont(); // Add the Queryoont middleware

    // [..]
}
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
    ID INT NOT NULL PRIMARY KEY,
    NAME VARCHAR(50)
)
```

``` csharp
// The query returned by the handler
var query = new Query("table1");
return query;
```

``` jsonc
{
    "version": 1.0,
    "select": [
        "table1.Id",
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
     table1.Id
    ,table1.Name
FROM
    table1
WHERE 
    table.Id < 20
```
