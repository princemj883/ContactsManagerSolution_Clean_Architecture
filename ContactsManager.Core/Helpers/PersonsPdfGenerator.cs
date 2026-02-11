using ContactsManager.Core.Helpers;
using QuestPDF.Fluent;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services.PDF;

public class PersonsPdfGenerator : IPersonsPdfGenerator
{
    public byte[] GeneratePersonsPdf(List<PersonResponse> persons)
    {
        var document = new PersonsPdfDocument(persons);
        return document.GeneratePdf();
    }
}