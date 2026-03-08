using DevSkill.Blog.Domain.Utilities.DataTable;

namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class GetRolesModel : DataTables
    {
        public AdvancedRoleSearchModel SearchItem { get; set; }
    }
}
