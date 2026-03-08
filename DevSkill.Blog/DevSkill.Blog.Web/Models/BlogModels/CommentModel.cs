namespace DevSkill.Blog.Web.Models.BlogModels
{
    public class CommentModel
    {
        public Guid PostId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
