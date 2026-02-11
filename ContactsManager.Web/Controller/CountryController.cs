using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;

namespace ContactsManager.Web.Controller;

public class CountryController(ICountriesService countriesService) : Microsoft.AspNetCore.Mvc.Controller
{
    [HttpPost("countries/upload-excel")]
    public async Task<IActionResult> UploadCountriesExcel(IFormFile? formFile)
    {
        //File null / empty check
        if (formFile == null || formFile.Length == 0)
            return BadRequest("Please upload a valid Excel file.");

        //Extension validation
        var extension = Path.GetExtension(formFile.FileName);

        if (!string.Equals(extension, ".xlsx", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Only .xlsx Excel files are allowed.");
        
        ExcelUploadResponse result = await countriesService.UploadCountriesFromExcelFile(formFile);
        
        if(result.InsertedCount == 0)
            return Ok(new
            {
                Message ="Records already exist or no valid records found in the uploaded file", 
                result.InsertedCount,
                result.DuplicateCount,
            });
        
        return Ok(new {Message ="Uploaded Successfully",
            result.InsertedCount,
            result.DuplicateCount,
        });
    }
}