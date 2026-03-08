using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Blog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetPostsWithCategoriesSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                                
                
                CREATE OR ALTER   PROCEDURE [dbo].[GetPostsWithCategories] 
                	@CategoryName nvarchar(250) = '%',
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN

                    DECLARE @sql nvarchar(MAX);
                	DECLARE @sqlParamList nvarchar(MAX);

                	DECLARE @countSql nvarchar(MAX);
                	DECLARE @countSqlParamList nvarchar(MAX);

                	--query for counting total posts with @CategoryName
                	SELECT 
                	@Total = COUNT(BPC.BlogPostId)
                	FROM Categories AS cat
                	INNER JOIN BlogPostCategories AS BPC ON BPC.CategoryId = cat.Id
                	WHERE cat.CategoryName LIKE @CategoryName + '%'

                	SET @TotalDisplay = @Total;

                	--query to find out all the posts with @CategoryName
                	SELECT 
                	post.Id,post.Title,post.ThumbnailName,post.Url,post.PublishedAt,post.UserId,Users.UserName
                	FROM BlogPosts AS post
                	INNER JOIN BLOGPOSTCATEGORIES AS BPC ON post.Id = BPC.BlogPostId
                	INNER JOIN Categories ON Categories.Id = BPC.CategoryId
                	INNER JOIN AspNetUsers AS Users ON post.UserId = Users.Id
                	WHERE Categories.CategoryName LIKE @CategoryName + '%'

                END
                GO
                
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetPostsWithCategories]";
            migrationBuilder.Sql(sql);
        }
    }
}
