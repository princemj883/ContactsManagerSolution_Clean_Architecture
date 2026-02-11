using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;

namespace ContactsManager.Core.ServiceContracts;

public interface ICountriesService
{
    Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
    
    /// <summary>
    /// Retruns all countries from the list 
    /// </summary>
    /// <returns>All Countries from the list as list of CountryResponse</returns>
    Task<List<CountryResponse>> GetAllCountries();
    
    Task<CountryResponse>? GetCountryByCountryId(Guid? countryId);
    
    /// <summary>
    /// Upload countries from excel file into database
    /// </summary>
    /// <param name="fromFile"></param>
    /// <returns>Number of countries added</returns>
    Task<ExcelUploadResponse> UploadCountriesFromExcelFile(IFormFile fromFile);
}