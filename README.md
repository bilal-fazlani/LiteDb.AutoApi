# LiteDb.AutoApi 

Want to create an api without writing code?

[![Build status](https://ci.appveyor.com/api/projects/status/q99s5ef9xko6k8y0?svg=true)](https://ci.appveyor.com/project/bilal-fazlani/litedb-autoapi)
[![tests](ttps://img.shields.io/appveyor/tests/bilal-fazlani/litedb-autoapi.svg)](https://ci.appveyor.com/project/bilal-fazlani/litedb-autoapi/build/tests)
[![nuget release](https://img.shields.io/nuget/vpre/LiteDB.AutoApi.svg)](https://www.nuget.org/packages/LiteDB.AutoApi/)

This library let's you create CRUD apis in dotnet core without writing any controller, service or database code. 
All you have to do is define your model... and that's it. This library will then expose 

- CREATE
- UPDATE
- DELETE
- LIST
- GET

These endpoints will persinst your data using a litedb. If you don't what litedb is, please check it out here -> http://www.litedb.org/

# Here's how it works

First you install this nuget package `LiteDB.AutoApi` in your aspnet core application

Then create a model class you want to persist and expose as endpoint. It needs to inherit from LiteDbModel class. For example :

```c#
public class Vehicle : LiteDbModel
{
    public string Model { get; set; }
    
    public string Number { get; set; }
}
```

Now, in the Startup class, make the following change :

```c#
services.AddMvc()
    .AddAutoApi<Vehicle>("/vehicles");
```

And we are done. Note that you can add any number of AutoApis.

You have successfully created an api which can 

- create/update a vehicle at `HTTPPOST` `/vehicles`
- retrieve a vehicle by id at `HTTPGET` `/vehicles/{vehicleId}`
- delete a vehicle at `HTTPDELETE` `/vehicles/{vehicleId}`
- list all vehicles at `HTTPGET` `/vehicles`

