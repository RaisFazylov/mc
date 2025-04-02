using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

public class Record
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }  // Название

    public DateTime Date { get; set; }  // Дата

    public string Category { get; set; }  // Категория

    public string Description { get; set; }  // Описание
}

public class AppDbContext : DbContext
{
    public AppDbContext() : base("name=SQLiteConnection") { }

    public DbSet<Record> Records { get; set; }
}