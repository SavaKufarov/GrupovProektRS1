using BusinessLayer;
using DataLayer;
using DataLayer.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System;

namespace TestLayer;

[TestFixture]
public class TestManager
{
    internal static ExpenseTrackerDbContext dbContext;

    static TestManager()
    {
        DbContextOptionsBuilder<ExpenseTrackerDbContext> builder = new DbContextOptionsBuilder<ExpenseTrackerDbContext>();
        builder.UseInMemoryDatabase("TestingDb");
        dbContext = new ExpenseTrackerDbContext(builder.Options);
    }

    [OneTimeTearDown]
    public void Dispose()
    {
        dbContext.Dispose();
    }
}
