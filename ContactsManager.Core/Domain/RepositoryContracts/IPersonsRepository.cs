using System.Linq.Expressions;
using ContactsManager.Core.Domain.Entities;

namespace ContactsManager.Core.Domain.RepositoryContracts;

/// <summary>
/// Represents data access layer for managing Person entities
/// </summary>
public interface IPersonsRepository
{
    /// <summary>
    /// Adds a person object to the data store
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    Task<Person> AddPerson(Person? person);
    
    /// <summary>
    /// Returns all persons from the data store
    /// </summary>
    /// <returns></returns>
    Task<List<Person>> GetPersonsList();
    
    /// <summary>
    /// Returns person object based on personId
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    Task<Person?> GetPersonById(Guid personId);

    /// <summary>
    /// Returns all persons object based on the experession predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);
    
    /// <summary>
    /// Deletes the person object based on personId
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    Task<bool> DeletePersonByPersonId(Guid? personId);
    
    /// <summary>
    ///Updates the person object in the data store
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    Task<Person?> UpdatePerson(Person? person);
}