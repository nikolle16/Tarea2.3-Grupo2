using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAudioPluginRecordingSample.Controllers
{
    public class AudioController
    {
        //Crud
        //Create
        public async static Task<int> Create(Models.Audio audio)
        {
            try
            {
                String jsonObject = JsonConvert.SerializeObject(audio);
                System.Net.Http.StringContent contenido = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = null;

                    response = await client.PostAsync(Config.Config.EndPointCreate, contenido);

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                        }
                        else
                        {
                            Console.WriteLine($"Ha Ocurrido un Error: {response.ReasonPhrase}");
                            return -1;
                        }
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ha Ocurrido un Error: {ex.ToString()}");
                return -1;
            }
        }

        //Read
        public async static Task<List<Models.Audio>> Get()
        {
            List<Models.Audio> audiolist = new List<Models.Audio>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = null;
                    response = await client.GetAsync(Config.Config.EndPointList);
                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            try
                            {
                                audiolist = JsonConvert.DeserializeObject<List<Models.Audio>>(result);
                            }
                            catch (JsonException jex)
                            {

                            }
                        }
                    }
                    return audiolist;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
