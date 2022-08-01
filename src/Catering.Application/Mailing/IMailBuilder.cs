using Catering.Domain.Builders;

namespace Catering.Application.Mailing;

public interface IMailBuilder<T> : IBuilder<T> where T : IMail
{
}
