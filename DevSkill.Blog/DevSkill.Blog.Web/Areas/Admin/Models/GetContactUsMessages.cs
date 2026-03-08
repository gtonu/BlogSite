using DevSkill.Blog.Domain.Utilities.DataTable;

namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class GetContactUsMessages : DataTables
    {
        public AdvancedContactUsSearchModel SearchItem { get; set; }
    }
}
