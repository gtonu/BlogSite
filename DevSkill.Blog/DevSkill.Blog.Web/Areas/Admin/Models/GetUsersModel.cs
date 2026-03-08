using DevSkill.Blog.Domain.Utilities.DataTable;

namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class GetUsersModel : DataTables
    {
        public AdvancedUsersSearchModel? SearchItem { get; set; }
    }
}
