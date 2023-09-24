using Domain.Models;

namespace Domain.UseCases.CreateTopic
{
    public interface ICreateTopicUseCase
    {
        public Task<Topic> Execute(Guid forumId, string Title, Guid authorId, CancellationToken cancellationToken);
    }
}

