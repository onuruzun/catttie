using System;
using System.Threading.Tasks;
using catttiie.Models.Response;

namespace catttiie.Services.Abstract
{
    public interface ICatService
    {
        Task<Cat> GetCatsAsync(string id);
    }
}
