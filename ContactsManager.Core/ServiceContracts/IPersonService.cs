using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts;

public interface IPersonService
{
   Task<PersonResponse> AddPerson(PersonAddRequest personAddRequest);
   Task<List<PersonResponse>> GetPersonsList();
   
   /// <summary>
   /// Return the PersonResponse based on personId
   /// </summary>
   /// <param name="personId"></param>
   /// <returns>Returns Matching person object</returns>
   Task<PersonResponse?> GetPersonByPersonId(Guid? personId);

   /// <summary>
   /// 
   /// </summary>
   /// <param name="searchBy"></param>
   /// <param name="searchString"></param>
   /// <returns></returns>
   Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);
   
   /// <summary>
   /// 
   /// </summary>
   /// <param name="allpersons"></param>
   /// <param name="sortBy"></param>
   /// <param name="sortOrder"></param>
   /// <returns></returns>
   Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allpersons, string sortBy, SortOrderOptions sortOrder);
   
   /// <summary>
   ///
   /// </summary>
   /// <param name="personUpdateRequest"></param>
   /// <returns></returns>
   Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
   
   /// <summary>
   /// 
   /// </summary>
   /// <param name="personId"></param>
   /// <returns></returns>
   Task<bool> DeletePerson(Guid? personId);

   /// <summary>
   /// Returns Persons as CSV
   /// </summary>
   /// <returns></returns>
   Task<MemoryStream> GetPersonsCsv();
   
   Task<MemoryStream> GetPersonsExcel();
}
