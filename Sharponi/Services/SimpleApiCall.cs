using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sharponi.Services
{
    public class SimpleApiCall
    {
        public static async Task<T> Call<T>(string endpoint, IServiceProvider provider)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept
                              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (var response = await httpClient.GetAsync(endpoint))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<T>(apiResponse);
                        }
                        else
                        {
                            throw new Exception(response.ReasonPhrase);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                provider.GetRequiredService<ILogger<SimpleApiCall>>().LogError($"Error calling endpoint \"{endpoint}\"", e);
            }

            return default;
        }
        
        
        public static async Task<JObject> Call(string endpoint, IServiceProvider provider)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept
                              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (var response = await httpClient.GetAsync(endpoint))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            return JObject.Parse(apiResponse);
                        }
                        else
                        {
                            throw new Exception(response.ReasonPhrase);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                provider.GetRequiredService<ILogger<SimpleApiCall>>().LogError($"Error calling endpoint \"{endpoint}\"", e);
            }

            return default;
        }
    }
}