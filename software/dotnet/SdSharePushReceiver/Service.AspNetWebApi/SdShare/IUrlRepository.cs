using System.Linq;

namespace SdShare.Service.AspNetWebApi.SdShare
{
    public interface IUrlRepository
    {
        IQueryable<Url> GetAll();
        Url Get(int id);
        Url Add(Url url);
    }
}
