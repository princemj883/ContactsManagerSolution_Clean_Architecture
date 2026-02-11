using System.Globalization;
using ClosedXML.Excel;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using ContactsManager.Core.Exceptions;
using ContactsManager.Core.Helpers;
using ContactsManager.Core.ServiceContracts;
using CsvHelper;
using CsvHelper.Configuration;

namespace ContactsManager.Core.Services;

public class PersonService(IPersonsRepository personsRepository) : IPersonService
{
    public async Task<PersonResponse> AddPerson(PersonAddRequest personAddRequest)
    {
        //check if PersonAddRequest is not null
        if (personAddRequest == null)
            throw new ArgumentNullException(nameof(personAddRequest));

        //Model Validation
        ValidationHelper.ModelValidation(personAddRequest);

        //convert PersonAddRequest to Person entity
        Person person = personAddRequest.ToPerson();

        //Generate new Guid for PersonId
        person.PersonId = Guid.NewGuid();

        //add the person to the list
        await personsRepository.AddPerson(person);

        //convert the Person object into PersonResponse type
        return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetPersonsList()
    {
        var persons = await personsRepository.GetPersonsList();
        return persons.Select(temp => temp.ToPersonResponse()).ToList();
    }

    public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
    {
        if (personId == null)
            return null;

        Person? person = await personsRepository.GetPersonById(personId.Value);
        if (person == null)
            return null;
        return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
    {
        List<Person> persons = searchBy switch
        {
            nameof(PersonResponse.PersonName) =>
                await personsRepository.GetFilteredPersons(
                    x => x.PersonName!.Contains(searchString!)),

            nameof(PersonResponse.Email) =>
                await personsRepository.GetFilteredPersons(
                    x => x.Email!.Contains(searchString!)),

            nameof(PersonResponse.DateOfBirth) =>
                await personsRepository.GetFilteredPersons(
                    x => x.DateOfBirth.ToString().Contains(searchString!)),

            nameof(PersonResponse.Gender) =>
                await personsRepository.GetFilteredPersons(
                    x => x.Gender!.Contains(searchString!)),

            nameof(PersonResponse.CountryId) =>
                await personsRepository.GetFilteredPersons(
                    x => x.Country!.CountryName!.Contains(searchString!)),


            nameof(PersonResponse.Address) =>
                await personsRepository.GetFilteredPersons(
                    x => x.Address!.Contains(searchString!)),

            _ => await personsRepository.GetPersonsList()
        };
        return persons.Select(temp => temp.ToPersonResponse()).ToList();
    }

    public Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allpersons, string sortBy,
        SortOrderOptions sortOrder)
    {
        if (string.IsNullOrEmpty(sortBy))
            return Task.FromResult(allpersons);

        List<PersonResponse> sortedPersons = (sortBy, sortOrder)
            switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.ReceiveNewsLetter).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.ReceiveNewsLetter).ToList(),

                _ => allpersons
            };
        return Task.FromResult(sortedPersons);
    }

    public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        if (personUpdateRequest == null)
            throw new ArgumentNullException(nameof(personUpdateRequest));

        //Validation
        ValidationHelper.ModelValidation(personUpdateRequest);

        //get matching person object from the list
        Person? matchingPerson = await personsRepository.GetPersonById(personUpdateRequest.PersonId);
        if (matchingPerson == null)
            throw new InvalidPersonIdException($"Given Person ID does not exists");

        //Update the details 
        matchingPerson.PersonName = personUpdateRequest.PersonName;
        matchingPerson.Email = personUpdateRequest.Email;
        matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
        matchingPerson.Gender = personUpdateRequest.Gender.ToString();
        matchingPerson.CountryId = personUpdateRequest.CountryId;
        matchingPerson.Address = personUpdateRequest.Address;
        matchingPerson.ReceiveNewsLetter = personUpdateRequest.ReceiveNewsLetter;
        
        await personsRepository.UpdatePerson(matchingPerson);
        
        return matchingPerson.ToPersonResponse();
    }

    public async Task<bool> DeletePerson(Guid? personId)
    {
        if (personId == null)
            throw new ArgumentNullException(nameof(personId));
        Person? person = await personsRepository.GetPersonById(personId.Value);
        if (person == null)
            return false;

        await personsRepository.DeletePersonByPersonId(personId.Value);

        return true;
    }

    public async Task<MemoryStream> GetPersonsCsv()
    {
        MemoryStream memoryStream = new MemoryStream();
        StreamWriter streamWriter = new StreamWriter(memoryStream);

        CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);
        
        //PersonName, Email etx
        csvWriter.WriteField(nameof(PersonResponse.PersonName));
        csvWriter.WriteField(nameof(PersonResponse.Email));
        csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
        csvWriter.WriteField(nameof(PersonResponse.Age));
        csvWriter.WriteField(nameof(PersonResponse.Gender));
        csvWriter.WriteField(nameof(PersonResponse.CountryName));
        csvWriter.WriteField(nameof(PersonResponse.Address));
        csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetter));
        csvWriter.NextRecord();
        
        List<PersonResponse> persons = await GetPersonsList();

        foreach (PersonResponse person in persons)
        {
            csvWriter.WriteField(person.PersonName);
            csvWriter.WriteField(person.Email);
            if(person.DateOfBirth.HasValue)
                csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MM-dd"));
            else
                csvWriter.WriteField("");
            csvWriter.WriteField(person.Age);
            csvWriter.WriteField(person.Gender);
            csvWriter.WriteField(person.CountryName);
            csvWriter.WriteField(person.Address);
            csvWriter.WriteField(person.ReceiveNewsLetter);
            csvWriter.NextRecord();
            csvWriter.Flush();
        }

        //await streamWriter.FlushAsync();
        memoryStream.Position = 0;
        return memoryStream;

    }

    public async Task<MemoryStream> GetPersonsExcel()
    {
        var persons = await GetPersonsList();

        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Persons");

        //Header
        worksheet.Cell(1, 1).Value = "PersonId";
        worksheet.Cell(1, 2).Value = "PersonName";
        worksheet.Cell(1, 3).Value = "Email";
        worksheet.Cell(1, 4).Value = "DateOfBirth";
        worksheet.Cell(1, 5).Value = "Gender";
        worksheet.Cell(1, 6).Value = "CountryName";
        worksheet.Cell(1, 7).Value = "Address";
        worksheet.Cell(1, 8).Value = "ReceiveNewsLetter";
        worksheet.Cell(1, 8).Value = "Age";

        //Style Header
        worksheet.Row(1).Style.Font.Bold = true;

        //Data
        int row = 2;
        foreach (var person in persons)
        {
            worksheet.Cell(row, 1).Value = person.PersonId.ToString();
            worksheet.Cell(row, 2).Value = person.PersonName;
            worksheet.Cell(row, 3).Value = person.Email;
            worksheet.Cell(row, 4).Value = person.DateOfBirth?.ToString("yyyy-MM-dd") ?? "-";
            worksheet.Cell(row, 5).Value = person.Gender;
            worksheet.Cell(row, 6).Value = person.CountryName;
            worksheet.Cell(row, 7).Value = person.Address;
            worksheet.Cell(row, 8).Value = person.ReceiveNewsLetter;
            worksheet.Cell(row, 9).Value = person.Age;
            row++;
        }

        worksheet.Columns().AdjustToContents();
         var stream = new MemoryStream();
         workbook.SaveAs(stream);
         stream.Position = 0;

         return stream;
    }
}