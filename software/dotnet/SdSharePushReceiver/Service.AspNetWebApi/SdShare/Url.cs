using System;

namespace SdShare.Service.AspNetWebApi.SdShare
{
    public class Url
    {
        public int UrlId { get; set; }

        public string Address { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
