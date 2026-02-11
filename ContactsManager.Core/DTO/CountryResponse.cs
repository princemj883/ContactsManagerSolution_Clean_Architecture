using Entities;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO Class for returning Country details
/// </summary>
public class CountryResponse
{
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if(obj.GetType() != typeof(CountryResponse))
            return false;
        CountryResponse countryResponse = (CountryResponse)obj;
        return CountryId == countryResponse.CountryId && CountryName == countryResponse.CountryName;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public static class CountryExtensions
{
    public static CountryResponse ToCountryResponse(this Country country)
    {
        return new CountryResponse()
        {
            CountryId = country.CountryId,
            CountryName = country.CountryName
        };
    }
}