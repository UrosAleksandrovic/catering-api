namespace Catering.Domain.Entities;

public interface ISoftDeletable
{
    bool IsDeleted { get; }

    void MarkAsDeleted();
}
