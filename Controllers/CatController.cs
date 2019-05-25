using System.Threading.Tasks;
using catttiie.Models.Response;
using catttiie.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace catttiie.Controllers
{
    [Produces("application/json")]
    [Route("cats")]
    [ApiController]
    public class CatController
    {
        readonly ICatService _catService;

        public CatController(ICatService catService)
        {
            _catService = catService;
        }

        [HttpGet]
        [Route("cat")]
        public Task<Cat> GetCats(string id)
        {
            var result = _catService.GetCats(id);
            return result;
        }
    }
}
