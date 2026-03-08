using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Blog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetUsersSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE [dbo].[GetUsers]  
                	@PageIndex int,
                	@PageSize int,
                	@OrderBy nvarchar(100),
                	@UserName nvarchar(250) = '%',
                	@Email nvarchar(250) = '%',
                	@EmailConfirmed bit = null,
                	@RegistrationDateFrom datetime2(7) = null,
                	@RegistrationDateTo datetime2(7) = null,
                	@Total int output,
                	@TotalDisplay int output


                AS
                BEGIN

                --declare main query variable
                	Declare @sql nvarchar(2000);
                	Declare @paramList nvarchar(MAX);

                	--declare count query variable
                	Declare @countSql nvarchar(2000);
                	Declare @countParamList nvarchar(MAX);

                	--set total data count
                	SELECT @Total = count(*) from AspNetUsers;

                	--preparing count query
                	SET @countSql = 'select @xTotalDisplay = count(*) from AspNetUsers 
                	                 where 1=1 ';

                	IF @UserName IS NOT NULL
                	SET @countSql = @countSql + ' AND UserName LIKE ''%'' + @xUserName + ''%'''

                	IF @Email IS NOT NULL
                	SET @countSql = @countSql + ' AND Email LIKE ''%'' + @xEmail + ''%'''

                	IF @EmailConfirmed IS NOT NULL
                	SET @countSql = @countSql + ' AND EmailConfirmed = @xEmailConfirmed'

                	IF @RegistrationDateFrom IS NOT NULL
                	SET @countSql = @countSql + ' AND RegistrationDate >= @xRegistrationDateFrom'

                	IF @RegistrationDateTo IS NOT NULL
                	SET @countSql = @countSql + ' AND RegistrationDate <= @xRegistrationDateTo'

                	--preparing main query
                	SET @sql = 'select * from AspNetUsers 
                	            where 1=1 ';

                	IF @UserName IS NOT NULL
                	SET @sql = @sql + ' AND UserName LIKE ''%'' + @xUserName + ''%'''

                	IF @Email IS NOT NULL
                	SET @sql = @sql + ' AND Email LIKE ''%'' + @xEmail + ''%'''

                	IF @EmailConfirmed IS NOT NULL
                	SET @sql = @sql + ' AND EmailConfirmed = @xEmailConfirmed'

                	IF @RegistrationDateFrom IS NOT NULL
                	SET @sql = @sql + ' AND RegistrationDate >= @xRegistrationDateFrom'

                	IF @RegistrationDateTo IS NOT NULL
                	SET @sql = @sql + ' AND RegistrationDate <= @xRegistrationDateTo'

                	SET @sql = @sql + ' ORDER BY ' + @OrderBy + ' OFFSET @xPageSize * (@xPageIndex - 1)
                	ROWS FETCH NEXT @xPageSize ROWS ONLY';

                	--preparing count sql parameters
                	SELECT @countParamList = '@xUserName nvarchar(250),
                	                          @xEmail nvarchar(250),
                							  @xEmailConfirmed bit,
                							  @xRegistrationDateFrom datetime2(7),
                							  @xRegistrationDateTo datetime2(7),
                							  @xTotalDisplay int output';

                    --executing count sql
                	exec sp_executesql @countSql,@countParamList,
                	     @UserName,
                		 @Email,
                		 @EmailConfirmed,
                		 @RegistrationDateFrom,
                		 @RegistrationDateTo,
                		 @xTotalDisplay = @TotalDisplay output;

                    --preparing main sql parameters
                	SELECT @paramList = '@xUserName nvarchar(250),
                	                     @xEmail nvarchar(250),
                						 @xEmailConfirmed bit,
                						 @xRegistrationDateFrom datetime2(7),
                						 @xRegistrationDateTo datetime2(7),
                						 @xPageIndex int,
                						 @xPageSize int';

                	--executing main sql
                	exec sp_executesql @sql,@paramList,
                	     @UserName,
                		 @Email,
                		 @EmailConfirmed,
                		 @RegistrationDateFrom,
                		 @RegistrationDateTo,
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
            var sql = "DROP PROCEDURE [dbo].[GetUsers]";

            migrationBuilder.Sql(sql);
        }
    }
}
