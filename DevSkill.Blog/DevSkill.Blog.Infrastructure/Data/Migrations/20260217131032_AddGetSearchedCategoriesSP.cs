using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Blog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGetSearchedCategoriesSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                                
                CREATE OR ALTER PROCEDURE [dbo].[GetSearchedCategories]
                    @CategoryName nvarchar(250) = '%',
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN
                    --declare main query variables
                	DECLARE @sql nvarchar(2000);  --this query actually outputs the table
                	DECLARE @sqlParamList nvarchar(MAX);

                	--declare count query variables
                	DECLARE @countSql nvarchar(2000);  --this query just counts the number of rows
                	DECLARE @countSqlParamList nvarchar(MAX);

                	--sql for total categories count
                	SELECT @Total = count(*) from Categories;

                	--preparing count sql
                	 SELECT @TotalDisplay = count(*) from Categories
                	                 where CategoryName LIKE @CategoryName + '%' ;

                    SELECT * FROM Categories WHERE CategoryName LIKE @CategoryName + '%';
                END
                GO
                
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetSearchedCategories]";
            migrationBuilder.Sql(sql);
        }
    }
}
