using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Blog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetContactUsSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                
                
                CREATE OR ALTER PROCEDURE [dbo].[GetContactUs]
                       @PageIndex int,
                	   @PageSize int,
                	   @OrderBy nvarchar(200),
                	   @SenderName nvarchar(250) = '%',
                	   @SenderEmail nvarchar(250) = '%',
                	   @ReceivedDateFrom datetime2(7) = null,
                	   @ReceivedDateTo datetime2(7) = null,
                	   @RepliedDateFrom datetime2(7) = null,
                	   @RepliedDateTo datetime2(7) = null,
                	   @MarkAsRead bit = null,
                	   @Total int output,
                	   @TotalDisplay int output

                AS
                BEGIN
                      --declaring main query and parameteres variable
                	  DECLARE @sql nvarchar(2000);
                	  DECLARE @sqlParamList nvarchar(MAX);

                	  --declaring count query and parameteres variable
                	  DECLARE @countSql nvarchar(2000);
                	  DECLARE @countSqlParamList nvarchar(MAX);

                	  --sql for total contactus messages count
                	  SELECT @Total = count(*) from ContactUs;

                	  --preparing count sql query
                	  SET @countSql = 'SELECT @xTotalDisplay = count(*) from ContactUs
                	                   where 1=1 '

                	  IF @SenderName IS NOT NULL
                	  SET @countSql = @countSql + ' AND SENDERNAME LIKE ''%'' + @xSenderName + ''%'''

                	  IF @SenderEmail IS NOT NULL
                	  SET @countSql = @countSql + ' AND SENDEREMAIL LIKE ''%'' + @xSenderEmail + ''%'''

                	  IF @MarkAsRead IS NOT NULL
                	  SET @countSql = @countSql + ' AND MARKASREAD = @xMarkAsRead'

                	  IF @ReceivedDateFrom IS NOT NULL
                	  SET @countSql = @countSql + ' AND RECEIVEDDATE >= @xReceivedDateFrom'

                	  IF @ReceivedDateTo IS NOT NULL
                	  SET @countSql = @countSql + ' AND RECEIVEDDATE <= @xReceivedDateTo'

                	  IF @RepliedDateFrom IS NOT NULL
                	  SET @countSql = @countSql + ' AND REPLIEDDDATE >= @xRepliedDateFrom'

                	  IF @RepliedDateTo IS NOT NULL
                	  SET @countSql = @countSql + ' AND REPLIEDDDATE <= @xRepliedDateTo'

                	  --preparing main query
                	  SET @sql = 'SELECT * from ContactUs
                	              where 1=1 '

                	  IF @SenderName IS NOT NULL
                	  SET @sql = @sql + ' AND SENDERNAME LIKE ''%'' + @xSenderName + ''%'''

                	  IF @SenderEmail IS NOT NULL
                	  SET @sql = @sql + ' AND SENDEREMAIL LIKE ''%'' + @xSenderEmail + ''%'''

                	  IF @MarkAsRead IS NOT NULL
                	  SET @sql = @sql + ' AND MARKASREAD = @xMarkAsRead'

                	  IF @ReceivedDateFrom IS NOT NULL
                	  SET @countSql = @countSql + ' AND RECEIVEDDATE >= @xReceivedDateFrom'

                	  IF @ReceivedDateTo IS NOT NULL
                	  SET @countSql = @countSql + ' AND RECEIVEDDATE <= @xReceivedDateTo'

                	  IF @RepliedDateFrom IS NOT NULL
                	  SET @countSql = @countSql + ' AND REPLIEDDDATE >= @xRepliedDateFrom'

                	  IF @RepliedDateTo IS NOT NULL
                	  SET @countSql = @countSql + ' AND REPLIEDDDATE <= @xRepliedDateTo'

                	  SET @sql = @sql + ' ORDER BY ' + @OrderBy + ' OFFSET @xPageSize * (@xPageIndex -1) 
                	                      ROWS FETCH NEXT @xPageSize ROWS ONLY';

                	  --preparing count sql query placeholders string
                	  SELECT @countSqlParamList = '@xSenderName nvarchar(250),
                	                               @xSenderEmail nvarchar(250),
                								   @xMarkAsRead bit,
                								   @xReceivedDateFrom datetime2(7),
                								   @xReceivedDateTo datetime2(7),
                								   @xRepliedDateFrom datetime2(7),
                								   @xRepliedDateTo datetime2(7),
                								   @xTotalDisplay int output';

                	  --executing count sql query
                	  exec sp_executesql @countSql,@countSqlParamList,
                	       @SenderName,
                		   @SenderEmail,
                		   @MarkAsRead,
                		   @ReceivedDateFrom,
                		   @ReceivedDateTo,
                		   @RepliedDateFrom,
                		   @RepliedDateTo,
                		   @xTotalDisplay = @TotalDisplay output;

                	 --preparing main query placeholders string
                	 SELECT @sqlParamList = '@xSenderName nvarchar(250),
                	                         @xSenderEmail nvarchar(250),
                							 @xMarkAsRead bit,
                							 @xReceivedDateFrom datetime2(7),
                							 @xReceivedDateTo datetime2(7),
                							 @xRepliedDateFrom datetime2(7),
                							 @xRepliedDateTo datetime2(7),
                							 @xPageIndex int,
                							 @xPageSize int';

                	 --executing main query
                	 exec sp_executesql @sql,@sqlParamList,
                	      @SenderName,
                		  @SenderEmail,
                		  @MarkAsRead,
                		  @ReceivedDateFrom,
                		  @ReceivedDateTo,
                		  @RepliedDateFrom,
                		  @RepliedDateTo,
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
            var sql = "DROP PROCEDURE [dbo].[GetContactus]";

            migrationBuilder.Sql(sql);
        }
    }
}
