using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class CountriesRepository(ApplicationDbContext dbContext) : ICountriesRepository
{
    public async Task<Country> AddCountries(IEnumerable<Country> countries)
    {
        dbContext.Countries.AddRange(countries);
        await dbContext.SaveChangesAsync();
        return countries.FirstOrDefault();
    }
    
    public async Task<Country> AddCountry(Country country)
    {
        dbContext.Countries.Add(country);
        await dbContext.SaveChangesAsync();
        return country;
    }

    public async Task<List<Country>> GetAllCountries()
    {
        return await dbContext.Countries.ToListAsync();
    }

    public async Task<Country?> GetCountryByCountryId(Guid? countryId)
    {
         return await dbContext.Countries.FirstOrDefaultAsync(x => x.CountryId == countryId);
    }

    public async Task<Country?> GetCountryByCountryName(string countryName)
    {
        return await dbContext.Countries.FirstOrDefaultAsync(x => x.CountryName == countryName);
    }
}