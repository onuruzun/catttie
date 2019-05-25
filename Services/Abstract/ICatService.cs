using System.Threading.Tasks;
using catttiie.Models.Response;

namespace catttiie.Services.Abstract
{
    public interface ICatService
    {
        Task<Cat> GetCats(string id);
    }
}
