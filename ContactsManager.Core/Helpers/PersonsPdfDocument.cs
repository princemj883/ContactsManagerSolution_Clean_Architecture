using ContactsManager.Core.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ContactsManager.Core.Helpers

{
    public class PersonsPdfDocument(IReadOnlyList<PersonResponse> persons) : IDocument
    {
        private readonly IReadOnlyList<PersonResponse> _persons = persons;

        public DocumentMetadata GetMetadata()
            => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(25);
                page.DefaultTextStyle(x => x.FontSize(11));

                // ---------- HEADER ----------
                page.Header()
                    .Text("Persons Report")
                    .FontSize(20)
                    .SemiBold()
                    .AlignCenter();

                // ---------- CONTENT ----------
                page.Content()
                    .PaddingVertical(10)
                    .Element(ComposeTable);

                // ---------- FOOTER ----------
                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Page ");
                        text.CurrentPageNumber();
                        text.Span(" of ");
                        text.TotalPages();
                    });
            });
        }

        private void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3); // Name
                    columns.RelativeColumn(4); // Email
                    columns.RelativeColumn(3); // DOB
                    columns.RelativeColumn(2); // Gender
                });

                // ----- TABLE HEADER -----
                table.Header(header =>
                {
                    header.Cell().Element(CellHeaderStyle).Text("Name");
                    header.Cell().Element(CellHeaderStyle).Text("Email");
                    header.Cell().Element(CellHeaderStyle).Text("Date of Birth");
                    header.Cell().Element(CellHeaderStyle).Text("Gender");
                });

                // ----- TABLE BODY -----
                foreach (var person in _persons)
                {
                    table.Cell().Element(CellBodyStyle).Text(person.PersonName);
                    table.Cell().Element(CellBodyStyle).Text(person.Email);
                    table.Cell().Element(CellBodyStyle)
                        .Text(person.DateOfBirth?.ToString("yyyy-MM-dd") ?? "-");
                    table.Cell().Element(CellBodyStyle).Text(person.Gender);
                }
            });
        }

        // ---------- STYLES ----------

        private static IContainer CellHeaderStyle(IContainer container)
        {
            return container
                .Background(Colors.Grey.Lighten3)
                .Border(1)
                .BorderColor(Colors.Grey.Lighten1)
                .Padding(6)
                .DefaultTextStyle(x => x.SemiBold());
        }

        private static IContainer CellBodyStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten1)
                .Padding(6);
        }
    }
}