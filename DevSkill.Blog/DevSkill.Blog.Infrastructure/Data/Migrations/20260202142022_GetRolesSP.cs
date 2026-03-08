using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Blog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetRolesSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                                
                CREATE OR ALTER PROCEDURE [dbo].[GetRoles]
                    @PageIndex int,
                	@PageSize int,
                	@OrderBy nvarchar(250),
                	@Name nvarchar(250) = '%',
                	@Total int output,
                	@TotalDisplay int output

                AS
                BEGIN
                	--declaring main query and query parameters variable
                	DECLARE @sql nvarchar(2000);
                	DECLARE @sqlParamList nvarchar(MAX);

                	--declaring count query and count query parameters variable
                	DECLARE @countSql nvarchar(2000);
                	DECLARE @countSqlParamList nvarchar(MAX);

                	--query to count total data
                	SELECT @Total = count(*) from AspNetRoles;

                	--preparing count query
                	SET @countSql = 'SELECT @xTotalDisplay = count(*) from AspNetRoles
                	                 where 1=1 '

                	IF @Name IS NOT NULL
                	SET @countSql = @countSql + 'AND Name LIKE ''%'' + @xName + ''%'''

                	--preparing main query
                	SET @sql = 'SELECT * FROM AspNetRoles where 1=1 '

                	IF @Name IS NOT NULL
                	SET @sql = @sql + 'AND Name LIKE ''%'' + @xName + ''%'''

                	SET @sql = @sql + ' ORDER BY ' + @OrderBy + ' OFFSET @xPageSize * (@xPageIndex -1) 
                	           ROWS FETCH NEXT @xPageSize ROWS ONLY';

                    --preparing count query parameters placeholders
                	SELECT @countSqlParamList = '@xName nvarchar(250),
                	                             @xTotalDisplay int output';

                	--executing count query
                	exec sp_executesql @countSql,@countSqlParamList,
                	     @Name,
                		 @xTotalDisplay = @TotalDisplay output;

                	--preparing main query parameters placeholders
                	SELECT @sqlParamList = '@xName nvarchar(250),
                	                        @xPageIndex int,
                							@xPageSize int';

                	--executing main query
                	exec sp_executesql @sql,@sqlParamList,
                	     @Name,
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
            var sql = "DROP PROCEDURE [dbo].[GetRoles]";
            migrationBuilder.Sql(sql);
        }
    }
}
