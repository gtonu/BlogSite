using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Blog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetCategoriesSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE [dbo].[GetCategories]
                       @PageIndex int,
                	   @PageSize int,
                	   @OrderBy nvarchar(200),
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
                	SET @countSql = 'SELECT @xTotalDisplay = count(*) from Categories
                	                 where 1=1 ';

                	IF @CategoryName IS NOT NULL
                	SET @countSql = @countSql + ' AND CategoryName LIKE ''%'' + @xCategoryName + ''%'''

                	--preparing main sql
                	SET @sql = 'SELECT * from Categories
                	            where 1=1 '

                	IF @CategoryName IS NOT NULL
                	SET @sql = @sql + ' AND CategoryName LIKE ''%'' + @xCategoryName + ''%'''

                	SET @sql = @sql + ' ORDER BY ' + @OrderBy + ' OFFSET @xPageSize * (@xPageIndex-1)
                	                   ROWS FETCH NEXT @xPageSize ROWS ONLY';

                	--preparing count sql parameters
                	SELECT @countSqlParamList = '@xCategoryName nvarchar(250),
                	                             @xTotalDisplay int output';

                	--executing count sql
                	exec sp_executesql @countSql,@countSqlParamList,
                	    @CategoryName,
                		@xTotalDisplay = @TotalDisplay output;

                    --preparing main sql parameters
                	SELECT @sqlParamList = '@xCategoryName nvarchar(250),
                	                        @xPageIndex int,
                							@xPageSize int';

                	--executing main sql
                	exec sp_executesql @sql,@sqlParamList,
                	     @CategoryName,
                		 @PageIndex,
                		 @PageSize;

                		 print @countSql;
                		 print @sql;
                END
                GO
                
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetCategories]";

            migrationBuilder.Sql(sql);
        }
    }
}
