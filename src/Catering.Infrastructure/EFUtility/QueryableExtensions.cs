using Catering.Application.Filtering;

namespace Catering.Infrastructure.EFUtility;

internal static class QueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, FilterBase filterBase)
        => query.Skip((filterBase.PageIndex - 1) * filterBase.PageSize).Take(filterBase.PageSize);
}
