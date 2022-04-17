namespace Catering.Application;

public interface IUnitOfWork : IPersistanceCommander
{
    IUnitOfWork WrapRepo<T>(IBaseRepository<T> repo);
    void Reset();

    bool IsTrackingCommandsEnabled { get; }
}
