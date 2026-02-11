using CRUD.Filters.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUD.Controller;

[ApiController]
[Route("api/[controller]")]
//[TypeFilter(typeof(HandleExceptionFilter))]
public class PersonController(IPersonService personService, IPersonsPdfGenerator pdfGenerator,ILogger<PersonController> logger) : ControllerBase
{
    
    [HttpGet]
    [TypeFilter(typeof(PersonsListActionFilter))]
    public async Task<IActionResult> GetAllPersons()
    {
        List<PersonResponse> persons = await personService.GetPersonsList();
        logger.LogInformation($"{persons.Count} people returned");
        return Ok(persons);
    }

    [HttpGet("{personId:guid}")]
    public async Task<IActionResult> GetPersonById(Guid personId)
    {
        PersonResponse? person =await personService.GetPersonByPersonId(personId);

        return Ok(person);
    }

    [HttpPost]
    public async Task<IActionResult> AddPerson([FromBody] PersonAddRequest request)
    {
        if (request == null)
            return BadRequest("Person data is required");
        PersonResponse response = await personService.AddPerson(request);
        
        return CreatedAtAction(nameof(GetPersonById), new { personId = response.PersonId }, response);
    }
    
    [HttpGet("filter")]
    [TypeFilter(typeof(PersonsListActionFilter))]
    [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = ["X-Custom-Key", "Custom-Value"])]
    public async Task<IActionResult> GetFilteredPersons([FromQuery] string searchBy, [FromQuery] string? searchString)
    {
        List<PersonResponse> persons = await personService.GetFilteredPersons(searchBy, searchString);

        return Ok(persons);
    }
    
    [HttpGet("sort")]
    public async Task<IActionResult> GetSortedPersons([FromQuery] string sortBy, [FromQuery] SortOrderOptions sortOrder)
    {
        List<PersonResponse> allPersons = await personService.GetPersonsList();
        List<PersonResponse> sortedPersons = await personService.GetSortedPersons(allPersons, sortBy, sortOrder);

        return Ok(sortedPersons);
    }

    [HttpPut("{personId:guid}")]
    public async Task<IActionResult> UpdatePerson(Guid personId, [FromBody] PersonUpdateRequest? request)
    {
        if(request == null)
            return BadRequest("Person data is required to update");
        
        request.PersonId = personId;
        
        PersonResponse updatedPerson = await personService.UpdatePerson(request);
        
        return Ok(updatedPerson);
    }
    
    [HttpDelete("{personId:guid}")]
    public IActionResult DeletePerson(Guid personId)
    {
        personService.DeletePerson(personId);
        return NoContent();
    }
    
    [HttpGet("pdf")]
    public async Task<IActionResult> PersonsPdf()
    {
        var persons = await personService.GetPersonsList();

        byte[] pdf = pdfGenerator.GeneratePersonsPdf(persons);

        return File(pdf, "application/pdf", "Persons.pdf");
    }
    
    [Route("PersonsCsv")]
    [HttpGet]
    public async Task<IActionResult> PersonsCsv()
    {
        MemoryStream personsCsv = await personService.GetPersonsCsv();

        return File(personsCsv, "text/csv", "Persons.csv");
    }
    
    [Route("PersonsExcel")]
    [HttpGet]
    public async Task<IActionResult> PersonsExcel()
    {
        var stream = await personService.GetPersonsExcel();

        return File(stream, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Persons.xlsx");
    }

}
