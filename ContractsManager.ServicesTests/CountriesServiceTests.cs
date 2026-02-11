using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using ContactsManager.Infrastructure.DbContext;
using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;

namespace ContractsManager.ServicesTests;

public class CountriesServiceTests
{
    private readonly ICountriesService _countriesService;
    
    public CountriesServiceTests()
    {
        var countriesInitialData = new List<Country>();
        DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
            new DbContextOptionsBuilder<ApplicationDbContext>().Options);
        

        ApplicationDbContext dbContext = dbContextMock.Object;
        dbContextMock.CreateDbSetMock(x => x.Countries, countriesInitialData);
        _countriesService = new CountriesService(null);
    }

    #region AddCountry Tests
    // When CountryAddRequest is null, AddCountry should throw ArgumentNullException
    [Fact]
    public async Task AddCountry_CountryAddRequestIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        CountryAddRequest request = null;
        
        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            //Act
             await _countriesService.AddCountry(request);
        });
    }
    
    //When CountryName is null or empty, AddCountry should throw ArgumentException
    [Fact]
    public async Task AddCountry_CountryNameIsNull_ThrowsArgumentException()
    {
        // Arrange
        CountryAddRequest request = new CountryAddRequest()
        {
            CountryName = null
        };
        
        //Assert
         await Assert.ThrowsAsync<ArgumentException>(async () =>        
        {
            //Act
             await _countriesService.AddCountry(request);
        });
    }
    
    //When the CountryName is duplicate, AddCountry should throw ArgumentException
    [Fact]
    public async Task AddCountry_DuplicateCountryName_ThrowsArgumentException()
    {
        // Arrange
        CountryAddRequest request1 = new CountryAddRequest()
        {
            CountryName = "USA"
        };
        CountryAddRequest request2 = new CountryAddRequest()
        {
            CountryName = "USA"
        };
        
        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async() =>        
        {
            //Act
            await _countriesService.AddCountry(request1);
            await _countriesService.AddCountry(request2);
        });
    }
    
    //When  you supply valid CountryName, it should insert(add) the country to the existing list of countries
    [Fact]
    public async Task AddCountry_ProperCountryDetails()
    {
        // Arrange
        CountryAddRequest request = new CountryAddRequest()
        {
            CountryName = "Japan"
        };
        
        //Act
        CountryResponse response = await _countriesService.AddCountry(request);
        List<CountryResponse> countriesFromGetAllCountries = await _countriesService.GetAllCountries();
        
        //Assert
        Assert.True(response.CountryId != Guid.Empty);
        Assert.Contains(response, countriesFromGetAllCountries);
    }
    
    #endregion

    #region GetAllCountries
    //List of countries should be empty by default
    [Fact]
    public async Task GetAllCountries_EmptyList()
    {
        //Act 
        List<CountryResponse> actualCountriesResponseList = await _countriesService.GetAllCountries();
        
        //Assert
        Assert.Empty(actualCountriesResponseList);
        
    }

    [Fact]
    public async Task GetAllCountries_AddFewCountries()
    {
        //Arrange
        List<CountryAddRequest> countryAddRequests = new List<CountryAddRequest>()
        {
            new CountryAddRequest(){ CountryName = "India"},
            new CountryAddRequest(){ CountryName = "USA"},
            new CountryAddRequest(){ CountryName = "UK"}
        };
        //Act
        List<CountryResponse> actualCountriesReponseList = new List<CountryResponse>();
        foreach (CountryAddRequest countryAddRequest in countryAddRequests)
        {
            actualCountriesReponseList.Add(await _countriesService.AddCountry(countryAddRequest));
        }
        //read each element in actualCountriesReponseList
        foreach (CountryResponse expectedCountryResponse in actualCountriesReponseList)
        {
            Assert.Contains(expectedCountryResponse, actualCountriesReponseList);
        }
    }
    #endregion
}