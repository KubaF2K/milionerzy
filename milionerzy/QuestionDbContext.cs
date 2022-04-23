using System;
using Microsoft.EntityFrameworkCore;

namespace milionerzy;

public class QuestionDbContext : DbContext
{
    public DbSet<Question> Questions { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies().UseSqlite("Data Source=./questionDb.db");
    }
}