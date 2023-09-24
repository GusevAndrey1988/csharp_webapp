using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Storage;
using Topic = Domain.Models.Topic;

namespace Domain.UseCases.CreateTopic
{
    public class CreateTopicUseCase : ICreateTopicUseCase
    {
        private readonly ForumDbContext dbContext;

        public CreateTopicUseCase(ForumDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Topic> Execute(Guid forumId, string title, Guid authorId, CancellationToken cancellationToken)
        {
            var forumExists = await dbContext.Forums.AnyAsync(f => f.ForumId == forumId, cancellationToken);
            if (!forumExists) {
                throw new ForumNotFoundException(forumId);
            }

            await dbContext.Topics.AddAsync(new Storage.Topic() {
                ForumId = forumId,
                Title = title,
                UserId = authorId,
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new Topic();
        }
    }
}