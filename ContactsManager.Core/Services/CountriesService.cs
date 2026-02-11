using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;

namespace ContactsManager.Core.Services;

public class CountriesService(ICountriesRepository countriesRepository) : ICountriesService
{
    // Private Field

    // Constructor
    public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
    {
        //Validation: countryAddRequest should not be null
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }

        // Validation: CountryName should not be null or empty
        if (countryAddRequest.CountryName == null)
        {
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        }

        // Validation: CountryName cannot be duplicate
        if (await countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
        {
            throw new ArgumentException("CountryName already exists");
        }

        Country country = countryAddRequest.ToCountry();
        country.CountryId = Guid.NewGuid();
        await countriesRepository.AddCountry(country);
        return country.ToCountryResponse();
    }

    public async Task<List<CountryResponse>> GetAllCountries()
    {
        return (await countriesRepository.GetAllCountries())
            .Select(country => country.ToCountryResponse()).ToList();
    }

    public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
    {
        if (countryId == null)
            return null;
        Country? countryResponse = await countriesRepository.GetCountryByCountryId(countryId.Value);
        if (countryResponse == null)
            return null;
        return countryResponse.ToCountryResponse();
    }

    public async Task<ExcelUploadResponse> UploadCountriesFromExcelFile(IFormFile fromFile)
    {
        if (fromFile == null || fromFile.Length == 0)
            throw new ArgumentException("File is required");
        using var memoryStream = new MemoryStream();
        await fromFile.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        using var workbook = new ClosedXML.Excel.XLWorkbook(memoryStream);
        var worksheet = workbook.Worksheets.First();
        var rows = worksheet.RowsUsed().Skip(1); // Skip header row

        var existingCountryNames = countriesRepository.GetAllCountries().ToString();

        var countries = new List<Country>();
        int duplicateCount = 0;

        foreach (var row in rows)
        {
            var countryName = row.Cell(1).GetString().Trim();
            if (string.IsNullOrEmpty(countryName))
                continue;
            //Skip duplicates (Excel + DB)
            if (existingCountryNames.Contains(countryName.ToLower()))
            {
                duplicateCount++;
                continue;
            }

            // Prevent duplicates within same Excel file
            if (countries.Any(c =>
                    c.CountryName.Equals(countryName, StringComparison.OrdinalIgnoreCase)))
            {
                duplicateCount++;
                continue;
            }

            countries.Add(new Country
            {
                CountryId = Guid.NewGuid(),
                CountryName = countryName
            });
        }

        if (countries.Any())
        {
            await countriesRepository.AddCountries(countries);
        }

        return new ExcelUploadResponse
        {
            InsertedCount = countries.Count,
            DuplicateCount = duplicateCount
        };
    }
}