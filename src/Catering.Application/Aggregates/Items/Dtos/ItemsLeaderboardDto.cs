using Catering.Domain;

namespace Catering.Application.Aggregates.Items.Dtos;

public record ItemsLeaderboardDto(Guid Id, string Name, double EvaluatedValue) : IdAndName<Guid>(Id, Name);
