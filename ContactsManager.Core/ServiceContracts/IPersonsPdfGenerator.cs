using ServiceContracts.DTO;

namespace ServiceContracts;

public interface IPersonsPdfGenerator
{
    byte[] GeneratePersonsPdf(List<PersonResponse> persons);
}