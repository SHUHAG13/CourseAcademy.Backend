using CourseAcademy.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;

namespace CourseAcademy.Tests;

/// <summary>An in-memory AppDbContext with the real interceptor and a controllable clock.</summary>
internal static class TestDb
{
    public static readonly DateTimeOffset Start = new(2026, 11, 14, 8, 0, 0, TimeSpan.Zero);

    public static (AppDbContext Db, FakeTimeProvider Clock) Create()
    {
        var clock = new FakeTimeProvider(Start);
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .AddInterceptors(new AuditInterceptor(clock))
            .Options;
        var db = new AppDbContext(options);
        db.Database.EnsureCreated(); // inserts the HasData seed
        return (db, clock);
    }
}
