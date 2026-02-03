namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class Post : EntityBase
    {
        public string Name { get; set; }
        public int PostStatus { get; set; }
        public string Description { get; set; }
        public DateTime? PostDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool Liked { get; set; }
        public int? Likes { get; set; }
        public string WrittenByName { get; set; }
        public Guid? WrittenBy { get; set; }
        public int Type { get; set; }
        public bool AdminArticle { get; set; }
        public int PostType { get; set; }
        public string VideoUrl { get; set; }
       // public bool Pinned { get; set; }

        private Post() { }
    }
}