using System;
using System.IO;
using Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DataTests;

public class DatabaseFixture : IDisposable
{
    private const string ConnectionString = 
        "host=localhost:5433;database=test;username=dummy;password=dummy123";
    public DatabaseFixture()
    {
        Context = new DatabaseContext(ConnectionString);
        
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
        
        var testData = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory, 
                "../../../../../database/db_content.sql")
        );
        
        //Inserts all test data from db_content.sql
        Context.Database.ExecuteSqlRaw(File.ReadAllText(testData));
    }
    
    public void Dispose()
    {
        Context.Database.CloseConnection();
        Context.Dispose();
    }

    public DatabaseContext Context { get; }
}

[CollectionDefinition("DataTestCollection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> {}
