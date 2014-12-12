using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Http;
using System.Xml;
using SdShare.Service.AspNetWebApi.SdShare;
using SyndicationFeedFormatter = System.ServiceModel.Syndication.SyndicationFeedFormatter;

//using SyndicationFeedFormatter = SdShare.Service.AspNetWebApi.SdShare.SyndicationFeedFormatter;

//using SyndicationFeedFormatter = System.ServiceModel.Syndication.SyndicationFeedFormatter;

namespace SdShare.Service.AspNetWebApi
{
    public class SdShareController : ApiController
    {
        private static readonly IUrlRepository _repo = new UrlRepository();

        [HttpGet]
        public IEnumerable<Url> Collections()
        {
            return _repo.GetAll();
        }

        //[HttpGet]
        //public HttpResponseMessage Collections()
        //{
        //    var feed = new SyndicationFeed(GetSyndicationItems())
        //    {
        //        Title = new TextSyndicationContent("Published Collections"),
        //        LastUpdatedTime = (DateTimeOffset)DateTime.UtcNow,
        //        Id = Guid.NewGuid().ToString()
        //    };

        //    var formatter = new Atom10FeedFormatter(feed);

        //    return Request.CreateResponse(HttpStatusCode.OK, feed, new SyndicationFeedFormatter(), "application/atom+xml");
        //}



        //[HttpGet]
        //public HttpResponseMessage Collections()
        //{
        //    //var feed = new SyndicationFeed(GetSyndicationItems())
        //    //{
        //    //    Title = new TextSyndicationContent("Published Collections"),
        //    //    LastUpdatedTime = (DateTimeOffset) DateTime.UtcNow,
        //    //    Id = Guid.NewGuid().ToString()
        //    //};

        //    //var formatter = new Atom10FeedFormatter(feed);
        //    //var sb = new StringBuilder();
        //    //using (var sw = new StringWriter(sb))
        //    //using (var xmlWriter = new XmlTextWriter(sw))
        //    //{
        //    //    formatter.WriteTo(xmlWriter);
        //    //}

        //    //return sb.ToString();
        //}

        private IEnumerable<SyndicationItem> GetSyndicationItems()
        {
            return new List<SyndicationItem>
            {
                GetExceptionsSyndicationItem()
            };
        }

        private SyndicationItem GetExceptionsSyndicationItem()
        {
            var item = new SyndicationItem
            {
                Title = new TextSyndicationContent("Exceptions"),
                Summary = new TextSyndicationContent("Details of exceptions"),
                LastUpdatedTime = DateTime.UtcNow
            };

            return item;
        }
    }
}
