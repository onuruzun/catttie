using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace catttiie.Helper
{
    public class HttpRequestManager
    {
        private readonly HttpClient _httpClient;
        private HttpRequestManager()
        {
            _httpClient = new HttpClient();
        }

        private static HttpRequestManager _helper;

        public static HttpRequestManager Instance
        {
            get
            {
                if (_helper == null)
                {
                    return new HttpRequestManager();
                }
                return _helper;
            }
        }

        public async Task<T> Get<T>(string uri) where T : new()
        {
            try
            {
                var httpContent = new StringContent(uri, Encoding.UTF8, "application/json");

                var result = await _httpClient.GetAsync(uri);

                string resultContent = await result.Content.ReadAsStringAsync();

                var returnModel = JsonConvert.DeserializeObject<T>(resultContent);

                return returnModel;
            }

            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
