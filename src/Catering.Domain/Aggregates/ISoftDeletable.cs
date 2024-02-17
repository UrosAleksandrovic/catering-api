namespace Catering.Domain.Aggregates;

public interface ISoftDeletable
{
    bool IsDeleted { get; }

    void MarkAsDeleted();
}
