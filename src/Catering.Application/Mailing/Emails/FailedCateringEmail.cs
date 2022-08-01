namespace Catering.Application.Mailing.Emails;

public class FailedCateringEmail : IMail
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<string> Recepiants { get; set; }
    public string Content { get; set; }
    public DateTime GeneratedOn { get; set; }
}
