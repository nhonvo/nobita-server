using HDBank.Core.Aggregate;
using HDBank.Core.Aggregate.GetKey;
using HDBank.Core.Aggregate.Login;
using HDBank.Core.Interfaces;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HDBank.Core.Services
{
    public class APIService : IAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APIService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string GenerateCredential(IData data, string publicKey)
        {
            try
            {
                var serializedData = JsonConvert.SerializeObject(data);
                Console.WriteLine(serializedData);

                //Create byte arrays to hold original, encrypted.
                
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(serializedData);
                byte[] encryptedData;

                //Create a new instance of RSACryptoServiceProvider to generate
                //public and private key data.
                using RSACryptoServiceProvider rsa = new();
                {
                    var bytePublicKey = Convert.FromBase64String(publicKey);
                    rsa.ImportSubjectPublicKeyInfo(bytePublicKey, out _);

                }
                //Pass the data to ENCRYPT, the public key information 
                //(using RSACryptoServiceProvider.ExportParameters(false),
                //and a boolean flag specifying no OAEP padding.
                encryptedData = RSAService.RSAEncrypt(dataToEncrypt, rsa.ExportParameters(false), false);

                return Convert.ToBase64String(encryptedData);
            }
            catch (ArgumentNullException)
            {
                //Catch this exception in case the encryption did
                //not succeed.
                return string.Empty;
            }
        }

        public async Task<BankResponse<GetKeyResponseData>> GetKey()
        {
            var client = _httpClientFactory.CreateClient("HDBank");
            client.DefaultRequestHeaders.Add("access-token", "eyJraWQiOiJXcDRGMndiQVpMa1d2WWgyNDhnYjNtUHBLRzZTdDRNcG85Tmc3U2diZ2E0PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiI0Y2Y5MmRlNy05ZmQ0LTQyMjgtYTg2Mi1kYmM4YWUyYTM2MzEiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiaXNzIjoiaHR0cHM6XC9cL2NvZ25pdG8taWRwLmFwLXNvdXRoZWFzdC0xLmFtYXpvbmF3cy5jb21cL2FwLXNvdXRoZWFzdC0xX1FiMVE4VFBzVSIsImNvZ25pdG86dXNlcm5hbWUiOiI0Y2Y5MmRlNy05ZmQ0LTQyMjgtYTg2Mi1kYmM4YWUyYTM2MzEiLCJvcmlnaW5fanRpIjoiZjI0ODRiZGMtOWVlYi00YjYyLWI1MDktOGYxY2IzMmFhZGYwIiwiYXVkIjoic2lrY25laTR0MmgzbnRrcWo1ZDQ5bHR2ciIsImV2ZW50X2lkIjoiMzUyOTYzZTQtZjY5MS00OGZiLTk2MWUtMGZlZTczODc4YzdmIiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE2NjkxNzIwNDIsIm5hbWUiOiJOR1VZRU4gSE9ORyBUSEFJIiwiZXhwIjoxNjY5MjgwMjgzLCJpYXQiOjE2NjkxOTM4ODMsImp0aSI6IjEwY2E1MDM1LWFlNGItNDU0NC05OWJlLWZjY2NlOWQyY2Q3NSIsImVtYWlsIjoibmd1eWVuaG9uZ3RoYWkyODA0MjAwMkBnbWFpbC5jb20ifQ.eiJ8dJ3oaBnxTUdvjlR7WbfrW1eJZyI67WccgJlfnbpMZ2nsxTn3FoiWUCQZ01y9HSYHUf9qIbvlLdIKnpkuBzzRpgTajWveYuR4vZsua1WW-1NyeR-SFr8s9I2KxWFU-0CfStdGij_NeGBHhnl1b1IVzKtdaQixnO74JKQ8WNSZXsdobB1ATCyhB74V8TSeEHyFIaXeyclA0CDb-b93Y_KXso_n_JHcdhQ9mOuknR1V6SXqFDQjzwhIV7Ns84EheEOnxtaKv_ENDZo489dhviJAaCqQlXVS9HguXNHbB8xuqM7pp8sTMEvsAlgsgL-Wyl-WeIgeAD0PYqXxSIIR1A");
            
            var response = await client.GetAsync($"get_key");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync()!;
                return JsonConvert.DeserializeObject<BankResponse<GetKeyResponseData>>(content);
            }

            return new BankResponse<GetKeyResponseData>();
        }
        public async Task<BankResponse<LoginResponseData>> Login(BankRequest<LoginRequest> request)
        {
            var client = _httpClientFactory.CreateClient("HDBank");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("access-token", "eyJraWQiOiJXcDRGMndiQVpMa1d2WWgyNDhnYjNtUHBLRzZTdDRNcG85Tmc3U2diZ2E0PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiI0Y2Y5MmRlNy05ZmQ0LTQyMjgtYTg2Mi1kYmM4YWUyYTM2MzEiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiaXNzIjoiaHR0cHM6XC9cL2NvZ25pdG8taWRwLmFwLXNvdXRoZWFzdC0xLmFtYXpvbmF3cy5jb21cL2FwLXNvdXRoZWFzdC0xX1FiMVE4VFBzVSIsImNvZ25pdG86dXNlcm5hbWUiOiI0Y2Y5MmRlNy05ZmQ0LTQyMjgtYTg2Mi1kYmM4YWUyYTM2MzEiLCJvcmlnaW5fanRpIjoiZjI0ODRiZGMtOWVlYi00YjYyLWI1MDktOGYxY2IzMmFhZGYwIiwiYXVkIjoic2lrY25laTR0MmgzbnRrcWo1ZDQ5bHR2ciIsImV2ZW50X2lkIjoiMzUyOTYzZTQtZjY5MS00OGZiLTk2MWUtMGZlZTczODc4YzdmIiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE2NjkxNzIwNDIsIm5hbWUiOiJOR1VZRU4gSE9ORyBUSEFJIiwiZXhwIjoxNjY5MjgwMjgzLCJpYXQiOjE2NjkxOTM4ODMsImp0aSI6IjEwY2E1MDM1LWFlNGItNDU0NC05OWJlLWZjY2NlOWQyY2Q3NSIsImVtYWlsIjoibmd1eWVuaG9uZ3RoYWkyODA0MjAwMkBnbWFpbC5jb20ifQ.eiJ8dJ3oaBnxTUdvjlR7WbfrW1eJZyI67WccgJlfnbpMZ2nsxTn3FoiWUCQZ01y9HSYHUf9qIbvlLdIKnpkuBzzRpgTajWveYuR4vZsua1WW-1NyeR-SFr8s9I2KxWFU-0CfStdGij_NeGBHhnl1b1IVzKtdaQixnO74JKQ8WNSZXsdobB1ATCyhB74V8TSeEHyFIaXeyclA0CDb-b93Y_KXso_n_JHcdhQ9mOuknR1V6SXqFDQjzwhIV7Ns84EheEOnxtaKv_ENDZo489dhviJAaCqQlXVS9HguXNHbB8xuqM7pp8sTMEvsAlgsgL-Wyl-WeIgeAD0PYqXxSIIR1A");
            var response = await client.PostAsync($"login", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync()!;
                var obj = JsonConvert.DeserializeObject<BankResponse<LoginResponseData>>(content);
                return obj;
            }
            return new BankResponse<LoginResponseData>();

        }
    }
}