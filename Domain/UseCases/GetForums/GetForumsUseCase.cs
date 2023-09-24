using Microsoft.EntityFrameworkCore;
using Storage;
using Forum = Domain.Models.Forum;

namespace Domain.UseCases.GetForums
{
  public class GetForumsUseCase : IGetForumsUseCase
  {
    private readonly ForumDbContext forumDbContext;

    public GetForumsUseCase(ForumDbContext dbContext)
    {
      this.forumDbContext = dbContext;
    }

    public async Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken)
    {
      return await forumDbContext.Forums.Select(f => new Forum() {
        Id = f.ForumId,
        Title = f.Title,
      }).ToArrayAsync(cancellationToken);
    }
  }
}
