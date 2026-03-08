using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Tests;

public class EditBlogPostCommandHandlerTests
{
    private AutoMock _moq;
    private EditBlogPostCommandHandler _editBlogPostCommandHandler;
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
        _editBlogPostCommandHandler = _moq.Create<EditBlogPostCommandHandler>();
        _applicationUnitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        _blogPostRepositoryMock = _moq.Mock<IBlogPostRepository>();
        _mapperMock = _moq.Mock<IMapper>();
    }
    [TearDown]
    public void TearDown()
    {
        _applicationUnitOfWorkMock?.Reset();
        _blogPostRepositoryMock?.Reset();
        _mapperMock?.Reset();
    }

    [Test]
    public async Task Should_Edit_Specific_Post()
    {
        //Arrange
        var command = new EditBlogPostCommand
        {
            Title = "UnitTest",
            Body = "NUnit framework is used for unit testing"
        };

        var postToEdit = new BlogPost
        {
            Title = "UnitTest",
            Body = "NUnit framework is used for unit testing"
        };

        _mapperMock.Setup(x => x.Map<BlogPost>(command)).Returns(postToEdit).Verifiable();

        _applicationUnitOfWorkMock.Setup(x => x.BlogPostRepository)
                          .Returns(_blogPostRepositoryMock.Object)
                          .Verifiable();

        _blogPostRepositoryMock.Setup(x => x.EditAsync(
            It.Is<BlogPost>(y => y.Title == postToEdit.Title && y.Body == postToEdit.Body)
            )).Returns(Task.CompletedTask)
            .Verifiable();
        //Act
        var editedPost = await _editBlogPostCommandHandler.Handle(command, CancellationToken.None);

        //Assert

        this.ShouldSatisfyAllConditions(
            () => editedPost.ShouldNotBeNull(),
            () => editedPost.ShouldBeOfType<BlogPost>(),
            () => editedPost.Title.ShouldBe(postToEdit.Title),
            () => editedPost.Body.ShouldBe(postToEdit.Body),

            () => _mapperMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll(),
            () => _applicationUnitOfWorkMock.VerifyAll()
            );
    }
    [Test]
    public async Task Should_Return_Null_When_Command_IsNUll()
    {
        //Arrange
        EditBlogPostCommand command = null;
        //Act
        var editedPost = await _editBlogPostCommandHandler.Handle(command, CancellationToken.None);
        //Assert
        this.ShouldSatisfyAllConditions(
            () => editedPost.ShouldBeNull()
            );
    }
}
