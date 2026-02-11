using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts;
using QuestPDF.Fluent;

namespace ContactsManager.Core.Helpers;

public class PersonsPdfGenerator : IPersonsPdfGenerator
{
    public byte[] GeneratePersonsPdf(List<PersonResponse> persons)
    {
        var document = new PersonsPdfDocument(persons);
        return document.GeneratePdf();
    }
}