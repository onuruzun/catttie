using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using catttiie.Helper;
using catttiie.Models.Request;
using catttiie.Models.Response;
using catttiie.Services.Abstract;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;

namespace catttiie.Services
{
    public class CatService : ICatService
    {
        private readonly IHostingEnvironment _hostingEvn;

        public CatService(
            IHostingEnvironment hostingEvn)
        {
            _hostingEvn = hostingEvn;
        }

        public Task<Cat> GetCats(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    Random random = new Random();

                    //server have 1677 cats
                    //TODO : you should auto check this count
                    int randomNumber = random.Next(0, 1677);

                    id = randomNumber.ToString();
                }

                string webRootPath = _hostingEvn.WebRootPath;

                var json = System.IO.File.ReadAllText(System.IO.Path.Combine(_hostingEvn.WebRootPath, "requesturi.json").ToString());

                RequestUri requestUri =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<RequestUri>(json);

                string uri = $"{requestUri.Uri}{id}";

                var catModel = GetNodes(uri);

                return catModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<Cat> GetNodes(string uri)
        {
            HttpClient client = new HttpClient();
            Cat catModel = new Cat();

            using (var response = await client.GetAsync(uri))
            {
                using (var content = response.Content)
                {
                    var result = await content.ReadAsStringAsync();
                    var doc = new HtmlDocument();

                    doc.LoadHtml(result);
                    var nodes = doc.DocumentNode.SelectNodes("//body/a/img/@src").ToList();

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
                        catModel = new Cat
                        {
                            Uri = item
                        };
                    }
                }
            }

            return catModel;
        }

    }
}
