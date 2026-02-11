using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts;

public interface IPersonsPdfGenerator
{
    byte[] GeneratePersonsPdf(List<PersonResponse> persons);
}