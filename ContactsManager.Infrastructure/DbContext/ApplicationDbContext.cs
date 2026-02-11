using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactsManager.Infrastructure.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Person> Persons { get; set; }
    


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().ToTable("Countries");
        modelBuilder.Entity<Person>().ToTable("Persons").Property(p => p.DateOfBirth)
            .HasColumnType("date");
        
        //Seed Data for Countries
        string countriesJson = File.ReadAllText("countries.json");
        List<Country>? countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
        
        foreach (Country country in countries)  
            modelBuilder.Entity<Country>().HasData(country);
        
        //Seed Data for Countries
        string personsJson = File.ReadAllText("persons.json");
        List<Person>? persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);
        
        foreach (Person person in persons)  
            modelBuilder.Entity<Person>().HasData(person);
        
        //Fluent API
        modelBuilder.Entity<Person>().Property(p => p.TIN)
            .HasColumnName("TaxIdentificationNumber")
            .HasColumnType("varchar(8)")
            .HasDefaultValue("ABCD1234");

        //modelBuilder.Entity<Person>().HasIndex(p => p.TIN).IsUnique();
        
    }
    
    public List<Person> sp_GetAllPersons()
    {
        return Persons
            .FromSqlRaw(@"SELECT * FROM get_all_persons()")
            .AsNoTracking()
            .ToList();
    }
}