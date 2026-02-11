using Entities;

namespace RepositoryContracts;

/// <summary>
/// Represents data access layer for managing Country entities
/// </summary>
public interface ICountriesRepository
{
    /// <summary>
    /// Adds new Country object to the data store
    /// </summary>
    /// <param name="countries"></param>
    /// <returns></returns>
    Task<Country> AddCountries(IEnumerable<Country> countries);
    
    /// <summary>
    /// Adds new Country object to the data store
    /// </summary>
    /// <param name="country"></param>
    /// <returns></returns>
    Task<Country> AddCountry(Country country);
    
    /// <summary>
    /// Retruns all countries from the data store
    /// </summary>
    /// <returns></returns>
    Task<List<Country>> GetAllCountries();
    
    /// <summary>
    /// Return country object based on countryId
    /// </summary>
    /// <param name="countryId"></param>
    /// <returns></returns>
    Task<Country?> GetCountryByCountryId(Guid? countryId);
    
    /// <summary>
    ///Returns country object based on countryName
    /// </summary>
    /// <param name="countryName"></param>
    /// <returns></returns>
    Task<Country?> GetCountryByCountryName(string countryName);
}