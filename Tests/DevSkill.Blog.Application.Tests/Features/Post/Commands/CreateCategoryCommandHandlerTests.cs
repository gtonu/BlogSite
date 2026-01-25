using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;

namespace DevSkill.Blog.Application.Tests;

public class CreateCategoryCommandHandlerTests
{
    private AutoMock _moq;
    private CreateCategoryCommandHandler _createCategoryCommandHandler;
    private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
    private Mock<ICategoryRepository> _categoryRepositoryMock;
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
        _createCategoryCommandHandler = _moq.Create<CreateCategoryCommandHandler>();
        _applicationUnitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        _categoryRepositoryMock = _moq.Mock<ICategoryRepository>();
        _mapperMock = _moq.Mock<IMapper>();
    }
    [TearDown]
    public void TearDown()
    {
        _applicationUnitOfWorkMock?.Reset();
        _categoryRepositoryMock?.Reset();
        _mapperMock?.Reset();
    }

    [Test]
    public async Task Category_IsValid()
    {
        //Arrange
        var command = new CreateCategoryCommand
        {
            CategoryName = "Machine Learning"
        };

        var category = new Category
        {
            CategoryName = "Machine Learning"
        };

        _mapperMock.Setup(x => x.Map<Category>(command)).Returns(category);

        _applicationUnitOfWorkMock.Setup(x => x.CategoryRepository)
            .Returns(_categoryRepositoryMock.Object)
            .Verifiable();

        _categoryRepositoryMock.Setup(x => x.AddAsync(
            It.Is<Category>(y =>
             y.Id != Guid.Empty
             && y.CategoryName == command.CategoryName
            )))
            .Returns(Task.CompletedTask)
            .Verifiable();

        
        //Act
        var result = await _createCategoryCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.ShouldBeOfType<Category>(),
            () => result.Id.ShouldNotBe(Guid.Empty),
            () => result.CategoryName.ShouldBe(command.CategoryName),

            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _categoryRepositoryMock.VerifyAll()
         );
    }
    
}
