namespace server.Entities
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string Url { get; set; }

        //Linked to post
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
