using Domain.Models;

namespace Domain.UseCases.GetForums
{
  public interface IGetForumsUseCase
  {
    Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken);
  }
}
