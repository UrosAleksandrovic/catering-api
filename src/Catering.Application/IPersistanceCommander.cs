namespace Catering.Application;

public interface IPersistanceCommander
{
    void TrackMultipleCommands();
    Task CommitAsync();
}
