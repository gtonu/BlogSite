using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Blog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetTagsSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                                
                CREATE OR ALTER PROCEDURE [dbo].[GetTags]
                    @PageIndex int,
                	@PageSize int,
                	@OrderBy nvarchar(250),
                	@TagName nvarchar(250) = '%',
                	@Total int output,
                	@TotalDisplay int output

                AS
                BEGIN
                    --declaring main query and parameters list variables
                	DECLARE @sql nvarchar(2000);
                	DECLARE @sqlParamList nvarchar(MAX);

                	--declaring count query and count parameters list variables
                	DECLARE @countSql nvarchar(2000);
                	DECLARE @countSqlParamList nvarchar(MAX);

                	--setting total data count
                	SELECT @Total = count(*) from Tags;

                	--preparing count query
                	SET @countSql = 'SELECT @xTotalDisplay = count(*) from Tags
                	                 where 1=1 '

                	IF @TagName IS NOT NULL
                	SET @countSql = @countSql + 'AND TagName LIKE ''%'' + @xTagName + ''%'''

                	--preparing main query
                	SET @sql = 'SELECT * FROM Tags where 1=1 '

                	IF @TagName IS NOT NULL
                	SET @sql = @sql + 'AND TagName LIKE ''%'' + @xTagName + ''%'''

                	SET @sql = @sql + ' ORDER BY ' + @OrderBy + ' OFFSET @xPageSize * (@xPageIndex -1) 
                	           ROWS FETCH NEXT @xPageSize ROWS ONLY';

                    --preparing count query parameters placeholders
                	SELECT @countSqlParamList = '@xTagName nvarchar(250),
                	                             @xTotalDisplay int output';

                	--executing count query
                	exec sp_executesql @countSql,@countSqlParamList,
                	     @TagName,
                		 @xTotalDisplay = @TotalDisplay output;

                	--preparing main query parameters placeholders
                	SELECT @sqlParamList = '@xTagName nvarchar(250),
                	                        @xPageIndex int,
                							@xPageSize int';

                	--executing main query
                	exec sp_executesql @sql,@sqlParamList,
                	     @TagName,
                		 @PageIndex,
                		 @PageSize;

                		 print @sql;
                		 print @countSql;
                END
                GO
                
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetTags]";

            migrationBuilder.Sql(sql);
        }
    }
}
