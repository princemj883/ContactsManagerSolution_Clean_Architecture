using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace CRUD.Filters.ActionFilters;

public class PersonsListActionFilter(ILogger<PersonsListActionFilter> logger) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        logger.LogInformation("PersonsListActionFilter.OnActionExecuting method");
        if (context.ActionArguments.ContainsKey("searchBy"))
        {
            string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);
            if (!string.IsNullOrEmpty(searchBy))
            {
                var searchByOptions = new List<string>()
                {
                    nameof(PersonResponse.PersonName),
                    nameof(PersonResponse.Email),
                    nameof(PersonResponse.DateOfBirth),
                    nameof(PersonResponse.Address),
                    nameof(PersonResponse.Gender),
                    nameof(PersonResponse.CountryId)
                };
                if (searchByOptions.Any(x => x == searchBy) == false)
                {
                    logger.LogInformation($"searchby actual value: {searchBy}", searchBy);
                    context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                    logger.LogInformation($"searchby updated value: {searchBy}", searchBy);
                }
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        logger.LogInformation("PersonsListActionFilter.OnActionExecuted method");
    }
}