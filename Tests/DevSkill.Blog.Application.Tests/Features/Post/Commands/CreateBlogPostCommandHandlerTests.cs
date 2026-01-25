using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;

namespace DevSkill.Blog.Application.Tests;

public class CreateBlogPostCommandHandlerTests
{
    private AutoMock _moq;
    private CreateBlogPostCommandHandler _createBlogPostCommandHandler;
    private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
    private Mock<IBlogPostRepository> _blogPostRepositoryMock;
    private Mock<IMapper> _mapperMock;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _moq = AutoMock.GetLoose();
    }
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _moq?.Dispose();
    }

    [SetUp]
    public void Setup()
    {
        _applicationUnitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        _blogPostRepositoryMock = _moq.Mock<IBlogPostRepository>();
        _mapperMock = _moq.Mock<IMapper>();
        _createBlogPostCommandHandler = _moq.Create<CreateBlogPostCommandHandler>();
    }
    [TearDown]
    public void TearDown()
    {
        _applicationUnitOfWorkMock?.Reset();
        _blogPostRepositoryMock?.Reset();
        _mapperMock?.Reset();
    }

    [Test]
    public async Task Create_BlogPost()
    {
        //Arrange
        var command = new CreateBlogPostCommand
        {
            Title = "Unit Test",
            Body = "Applying Unit test following proper implementations and conventions"
        };

        var blog = new BlogPost
        {
            Title = command.Title,
            Body = command.Body
        };
        //these _blogPostRepositoryMock.Setup() methods are same but applies different techniques for bypassing the AddAsync()
        //method.first one just passes object to check by creating the blog object above and second one checks without creating
        //a blog object..

        /* //technique 1..
        _blogPostRepositoryMock.Setup(x => x.AddAsync(
            blog))
            .Returns(Task.CompletedTask)
            .Verifiable();
        */

        //technique 2..
        _blogPostRepositoryMock.Setup(x => x.AddAsync(
            It.Is<BlogPost>(y => y.Id != Guid.Empty
                  && y.Title == command.Title
                  && y.Body == command.Body
            )))
            .Returns(Task.CompletedTask)
            .Verifiable();


        _applicationUnitOfWorkMock.Setup(x => x.BlogPostRepository)
                      .Returns(_blogPostRepositoryMock.Object)
                      .Verifiable();

        _mapperMock.Setup(x => x.Map<BlogPost>(command)).Returns(blog);
        //Act
        var result = await _createBlogPostCommandHandler.Handle(command,CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.ShouldBeOfType<BlogPost>(),
            () => result.Id.ShouldNotBe(Guid.Empty),
            () => result.Title.ShouldBe(command.Title),
            () => result.Body.ShouldBe(command.Body),

            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll()
            );

    }
}
