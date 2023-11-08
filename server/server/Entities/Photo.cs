﻿namespace WebAPI.Entities
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string Url { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}