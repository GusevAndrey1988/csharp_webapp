using Domain.Exceptions;
using Domain.UseCases.CreateTopic;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Storage;

namespace Domain.Tests;

public class CreateTopicUseCaseShould
{
    private readonly CreateTopicUseCase sut;
    private readonly ForumDbContext forumDbContext;

    public CreateTopicUseCaseShould()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ForumDbContext>()
            .UseInMemoryDatabase(nameof(CreateTopicUseCaseShould));

        forumDbContext = new ForumDbContext(dbContextOptionsBuilder.Options);
        sut = new(forumDbContext); 
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
    {
        await forumDbContext.Forums.AddAsync(new Forum() {
            ForumId = Guid.Parse("a8fad9ff-6e52-46f6-95a4-a4631ff6b19a"),
            Title = "Basic forum",
        });
        await forumDbContext.SaveChangesAsync();

        var forumId = Guid.Parse("4f895ea8-355c-483b-be67-666c01468c57");
        var authorId = Guid.Parse("5fcef535-2d94-48a8-9e13-05524c13d85c");

        await sut.Invoking(s => s.Execute(forumId, "Some Title", authorId, CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();
    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic()
    {
        var forumId = Guid.Parse("53d146a6-21ee-48f5-8a25-6796d49b3889");
        await forumDbContext.Forums.AddAsync(new Forum() {
            ForumId = forumId, 
            Title = "Existing forum",
        });

        var authorId = Guid.Parse("56a85214-ff51-46de-9460-b180b0114f86");
        await forumDbContext.Users.AddAsync(new User() {
            UserId = authorId,
            Login = "Author",
        });

        await forumDbContext.SaveChangesAsync();

        string topicName = "Hello world";
        var actual = await sut.Execute(forumId, topicName, authorId, CancellationToken.None);
        var allTopics = await forumDbContext.Topics.ToArrayAsync();
        allTopics.Should().BeEquivalentTo(new[] {
            new Topic() {
                ForumId = forumId,
                Title = topicName,
                UserId = authorId,
            }
        }, cfg => cfg.Including(t => t.ForumId).Including(t => t.Title).Including(t => t.UserId));
    }
}