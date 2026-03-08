using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Blog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGetblogsSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                                
                CREATE OR ALTER PROCEDURE [dbo].[Getblogs]
                    @Total int output,
                	@TotalDisplay int output
                AS
                BEGIN
                    DECLARE @sql nvarchar(MAX);
                	DECLARE @sqlParamList nvarchar(MAX);

                	DECLARE @countSql nvarchar(MAX);
                	DECLARE @countSqlParamList nvarchar(MAX);

                	--query for counting total posts associated with userId
                	SELECT DISTINCT
                	@Total = COUNT(users.Id)
                	FROM AspNetUsers AS users
                	LEFT JOIN BlogPosts AS posts ON posts.UserId = users.Id

                	SET @TotalDisplay = @Total;

                	--query to count all blogs and their posts
                	SELECT
                	users.Id AS UserId,users.UserName AS UserName,Count(posts.Id) AS PostCount
                	FROM AspNetUsers AS users
                	LEFT JOIN BlogPosts AS posts ON posts.UserId = users.Id
                	GROUP BY users.Id,users.UserName
                END
                GO
                
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[Getblogs]";
            migrationBuilder.Sql(sql);
        }
    }
}
