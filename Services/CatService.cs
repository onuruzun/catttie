using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using catttiie.Models.Request;
using catttiie.Models.Response;
using catttiie.Services.Abstract;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;

namespace catttiie.Services
{
    public class CatService : ICatService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHostingEnvironment _hostingEvn;

        public CatService(IHttpClientFactory httpClientFactory,
                          IHostingEnvironment hostingEvn)
        {
            _httpClientFactory = httpClientFactory;
            _hostingEvn = hostingEvn;
        }

        public async Task<Cat> GetCatsAsync(string id)
        {
            try
            {

                Cat model = new Cat();

                string webRootPath = _hostingEvn.WebRootPath;

                var json = System.IO.File.ReadAllText(System.IO.Path.Combine(_hostingEvn.WebRootPath, "requesturi.json").ToString());

                RequestUri requestUri =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<RequestUri>(json);

                string uri = $"{requestUri.Uri}{id}";

                HttpClient client = new HttpClient();
                using (var response = await client.GetAsync(uri))
                {
                    using (var content = response.Content)
                    {
                        var result = await content.ReadAsStringAsync();
                        var document = new HtmlDocument();

                        document.LoadHtml(result);
                        var nodes = document.DocumentNode.SelectNodes("//body/a/img/@src").ToList();

                        List<string> list = new List<string>();

                        foreach (var item in nodes)
                        {
                            for (int i = 0; i < nodes.Count; i++)
                            {
                                foreach (var itemAttr in item.Attributes)
                                {
                                    if (itemAttr.Name == "src")
                                    {
                                        list.Add(itemAttr.Value);
                                    }
                                }
                            }
                        }
                        foreach (var item in list)
                        {
                            model = new Cat
                            {
                                File = item
                            };
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
