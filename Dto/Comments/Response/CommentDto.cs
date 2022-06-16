namespace _0sechill.Dto.Comments.Response
{
    public class CommentDto
    {
        public string ID { get; set; }
        public string content { get; set; }
        public string authorName { get; set; }
        public List<CommentDto> childComments { get; set; }

        public CommentDto()
        {
            childComments = new List<CommentDto>();
        }
    }
}
