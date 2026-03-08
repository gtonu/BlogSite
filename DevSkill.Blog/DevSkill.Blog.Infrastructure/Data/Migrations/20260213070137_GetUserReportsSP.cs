using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Blog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetUserReportsSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE  [dbo].[GetReports]
                	@PageIndex int,
                	@PageSize int,
                	@OrderBy nvarchar(250),
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
                	SELECT @Total = count(*) from UserReports;

                	--preparing count query
                	SET @countSql = 'SELECT @xTotalDisplay = count(*) from UserReports
                					 where 1=1 '


                	--preparing main query
                	SET @sql = 'SELECT * FROM UserReports where 1=1 '

                	SET @sql = @sql + ' ORDER BY ' + @OrderBy + ' OFFSET @xPageSize * (@xPageIndex -1) 
                			   ROWS FETCH NEXT @xPageSize ROWS ONLY';

                	--preparing count query parameters placeholders
                	SELECT @countSqlParamList = '@xTotalDisplay int output';

                	--executing count query
                	exec sp_executesql @countSql,@countSqlParamList,
                  		   @xTotalDisplay = @TotalDisplay output;

                	--preparing main query parameters placeholders
                	SELECT @sqlParamList = '@xPageIndex int,
                              				@xPageSize int';

                	--executing main query
                	exec sp_executesql @sql,@sqlParamList,
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
            var sql = "DROP PROCEDURE [dbo].[GetReports]";
            migrationBuilder.Sql(sql);
        }
    }
}
