namespace Catering.Application.Mailing.Emails;

public class EmailTemplate
{
    public const string OrderPlacedTemplateName = "OrderPlaced_Restourant";

    public string Name { get; set; }
    public string HtmlTemplate { get; set; }
}
