using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BankofdotNet.ConsoleClent
{
    class Program
    {

        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            string apiEndPoint = "http://localhost:58840/api/customers";
            string IdentityEndPoint = "http://localhost:5000";

            //controllo se ci sono client connessi all'authority di is4
            //resource owner grant-type
            var discoRO = await DiscoveryClient.GetAsync(IdentityEndPoint);
            if (discoRO.IsError)
            {
                Console.WriteLine(discoRO.Error);
                return;
            }

            //ricavo il token da ISvr4 comunicando le credenziali del client
            var tokenClientRO = new TokenClient(discoRO.TokenEndpoint, "ro.client", "secret");
            //vedere gli Users che possono accedere elencati in Config.cs del progetto IS4
            var tokenResponseRO = await tokenClientRO.RequestResourceOwnerPasswordAsync("Gaetano", "password", "BankofdotNetAPI");
            if (tokenResponseRO.IsError)
            {
                Console.WriteLine(tokenResponseRO.Error);
                return;
            }
            Console.WriteLine(tokenResponseRO.Json);
            Console.WriteLine("\n\n");


            //controllo se ci sono client connessi all'authority di is4
            var disco = await DiscoveryClient.GetAsync(IdentityEndPoint);
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            //ricavo il token da ISvr4 comunicando le credenziali del client
            var tokenClient = new TokenClient(disco.TokenEndpoint,"client","secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("BankofdotNetAPI");
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return; 
            }
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            //ottenuto il token posso interrogare le mie API
            var client = new HttpClient();
            client.SetBearerToken(tokenResponseRO.AccessToken);

            //creo un content di tipo json=>string da inviare nel body del post verso la API di creazione customer
            var customerInfo = new StringContent(
                JsonConvert.SerializeObject(
                    new { Id = 12, FirstName = "gaetanos", LastName = "Scrimieri" }), Encoding.UTF8, "application/json");

            //post a web api
            var createcustomerResponse = await client.PostAsync(apiEndPoint, customerInfo);

            if(!createcustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(createcustomerResponse.StatusCode);
            }

            //leggo tutti gli utenti
            var getcustomerResponse = await client.GetAsync(apiEndPoint);
            if (!getcustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(getcustomerResponse.StatusCode);
            }
            else
            {
                var content = await getcustomerResponse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.Read();
        }
    }
}
