using System.Linq.Expressions;
using ContactsManager.Infrastructure.DbContext;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories;

public class PersonsRepository(ApplicationDbContext dbContext) : IPersonsRepository
{
    public async Task<Person> AddPerson(Person? person)
    {
        await dbContext.Persons.AddAsync(person);
        await dbContext.SaveChangesAsync();
        return person;
    }

    public async Task<List<Person>> GetPersonsList()
    {
         return await dbContext.Persons.Include("Country")
            .ToListAsync();
    }

    public async Task<Person?> GetPersonById(Guid personId)
    {
        return await dbContext.Persons.Include("Country")
            .Where(x => x.PersonId == personId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
    {
         return await dbContext.Persons.Include("Country")
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<bool> DeletePersonByPersonId(Guid? personId)
    {
        dbContext.Persons.RemoveRange(dbContext.Persons.Where(x => x.PersonId == personId));
        int rowDeleted = await dbContext.SaveChangesAsync();
        return rowDeleted > 0;
    }

    public async Task<Person?> UpdatePerson(Person? person)
    {
        Person? machingPerson = await dbContext.Persons
            .FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
        if (machingPerson != null)
            return person;
        machingPerson.PersonName = person.PersonName;
        machingPerson.Country = person.Country;
        machingPerson.Email = person.Email;
        machingPerson.DateOfBirth = person.DateOfBirth;
        machingPerson.Address = person.Address;
        machingPerson.Gender = person.Gender;
        machingPerson.ReceiveNewsLetter = person.ReceiveNewsLetter;
        
        await dbContext.SaveChangesAsync();
        return machingPerson;
        
    }
}