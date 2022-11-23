using HDBank.Core.Aggregate;
using HDBank.Core.Interfaces;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HDBank.Core.Services
{
    public class APIService : IAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APIService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string GenerateCredential(LoginData data, string publicKey)
        {
            try
            {
                var serializedData = JsonConvert.SerializeObject(data);
                Console.WriteLine(serializedData);
                //Create a UnicodeEncoder to convert between byte array and string.
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                //Create byte arrays to hold original, encrypted, and decrypted data.
                byte[] dataToEncrypt = ByteConverter.GetBytes(serializedData);
                byte[] encryptedData;

                //Create a new instance of RSACryptoServiceProvider to generate
                //public and private key data.
                using RSACryptoServiceProvider RSA = new();
                
                /*
                {
                    publicKey = "<RSAKeyValue><Modulus>g2NQ826qGrz/FFT2AER6cvsz5WhYgXX94pq+ZuEXWxwsDLnnr5PjaFldnai/ywZ5PKFjKlpIDA505Xl3v474+s0BLrgqAVQ4mauJNCV2x3U5l4NP3x+3fWA9AbEpKUiR/APUQUp47yMyizR1yk1MsRtUCCCm1NpYVQAf4ytlLXc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
                    RSA.FromXmlString(publicKey);
                    var pk = RSA.ExportParameters(false);
                    var pubKey = RSA.ExportRSAPublicKey();
                    Console.WriteLine(Convert.ToBase64String(pubKey));

                }
                */
                {
                    //var pk = "-----BEGIN PUBLIC KEY-----" + publicKey + "-----END PUBLIC KEY-----";
                    //var next = pk.TrimStart("-----BEGIN PUBLIC KEY-----".ToCharArray()).TrimEnd("-----END PUBLIC KEY-----".ToCharArray());
                    var bytePublicKey = Convert.FromBase64String(publicKey);
                    Console.WriteLine("Acctual key: " + publicKey);
                    RSA.ImportSubjectPublicKeyInfo(bytePublicKey, out _);
                    var infoPk = RSA.ExportSubjectPublicKeyInfo();
                    //RSA.ImportRSAPublicKey(infoPk, out _);
                    //var rsaPk = RSA.ExportRSAPublicKey();

                    //Console.WriteLine("Public RSA key: " + Convert.ToBase64String(rsaPk));
                    Console.WriteLine("Public Info key: " + Convert.ToBase64String(infoPk));
                }
                /*
                {
                    var bytePublicKey = Convert.FromBase64String(publicKey);

                    RsaKeyParameters param = (RsaKeyParameters) PublicKeyFactory.CreateKey(bytePublicKey);
                    RSAParameters rsaParam = new();
                    rsaParam.Modulus = param.Modulus.ToByteArrayUnsigned();
                    rsaParam.Exponent = param.Exponent.ToByteArrayUnsigned();
                    RSA.ImportParameters(rsaParam);
                    var pubKey = RSA.ExportRSAPublicKey();
                    Console.WriteLine("Public key: " + Convert.ToBase64String(pubKey));
                }
                */
                //Pass the data to ENCRYPT, the public key information 
                //(using RSACryptoServiceProvider.ExportParameters(false),
                //and a boolean flag specifying no OAEP padding.
                encryptedData = RSAService.RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);

                //Pass the data to DECRYPT, the private key information 
                //(using RSACryptoServiceProvider.ExportParameters(true),
                //and a boolean flag specifying no OAEP padding.
                //decryptedData = RSAService.RSADecrypt(encryptedData, RSA.ExportParameters(true), false);

                //Display the decrypted plaintext to the console. 
                //Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));
                return Convert.ToBase64String(encryptedData);
            }
            catch (ArgumentNullException)
            {
                //Catch this exception in case the encryption did
                //not succeed.
                return "Encryption failed.";
            }
        }

        public async Task<string> GetKey()
        {
            var client = _httpClientFactory.CreateClient("HDBank");
            client.DefaultRequestHeaders.Add("x-api-key", "hutech_hackathon@123456");
            client.DefaultRequestHeaders.Add("access-token", "eyJraWQiOiJXcDRGMndiQVpMa1d2WWgyNDhnYjNtUHBLRzZTdDRNcG85Tmc3U2diZ2E0PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiI0Y2Y5MmRlNy05ZmQ0LTQyMjgtYTg2Mi1kYmM4YWUyYTM2MzEiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiaXNzIjoiaHR0cHM6XC9cL2NvZ25pdG8taWRwLmFwLXNvdXRoZWFzdC0xLmFtYXpvbmF3cy5jb21cL2FwLXNvdXRoZWFzdC0xX1FiMVE4VFBzVSIsImNvZ25pdG86dXNlcm5hbWUiOiI0Y2Y5MmRlNy05ZmQ0LTQyMjgtYTg2Mi1kYmM4YWUyYTM2MzEiLCJvcmlnaW5fanRpIjoiZjI0ODRiZGMtOWVlYi00YjYyLWI1MDktOGYxY2IzMmFhZGYwIiwiYXVkIjoic2lrY25laTR0MmgzbnRrcWo1ZDQ5bHR2ciIsImV2ZW50X2lkIjoiMzUyOTYzZTQtZjY5MS00OGZiLTk2MWUtMGZlZTczODc4YzdmIiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE2NjkxNzIwNDIsIm5hbWUiOiJOR1VZRU4gSE9ORyBUSEFJIiwiZXhwIjoxNjY5MjgwMjgzLCJpYXQiOjE2NjkxOTM4ODMsImp0aSI6IjEwY2E1MDM1LWFlNGItNDU0NC05OWJlLWZjY2NlOWQyY2Q3NSIsImVtYWlsIjoibmd1eWVuaG9uZ3RoYWkyODA0MjAwMkBnbWFpbC5jb20ifQ.eiJ8dJ3oaBnxTUdvjlR7WbfrW1eJZyI67WccgJlfnbpMZ2nsxTn3FoiWUCQZ01y9HSYHUf9qIbvlLdIKnpkuBzzRpgTajWveYuR4vZsua1WW-1NyeR-SFr8s9I2KxWFU-0CfStdGij_NeGBHhnl1b1IVzKtdaQixnO74JKQ8WNSZXsdobB1ATCyhB74V8TSeEHyFIaXeyclA0CDb-b93Y_KXso_n_JHcdhQ9mOuknR1V6SXqFDQjzwhIV7Ns84EheEOnxtaKv_ENDZo489dhviJAaCqQlXVS9HguXNHbB8xuqM7pp8sTMEvsAlgsgL-Wyl-WeIgeAD0PYqXxSIIR1A");
            

            var response = await client.GetAsync($"get_key");
            return await response.Content.ReadAsStringAsync()!; ;
        }
        public async Task<string> Login(BankRequest<LoginRequest> request)
        {
            var client = _httpClientFactory.CreateClient("HDBank");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            Console.WriteLine(await httpContent.ReadAsStringAsync());
            client.DefaultRequestHeaders.Add("x-api-key", "hutech_hackathon@123456");

            client.DefaultRequestHeaders.Add("access-token", "eyJraWQiOiJXcDRGMndiQVpMa1d2WWgyNDhnYjNtUHBLRzZTdDRNcG85Tmc3U2diZ2E0PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiI0Y2Y5MmRlNy05ZmQ0LTQyMjgtYTg2Mi1kYmM4YWUyYTM2MzEiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiaXNzIjoiaHR0cHM6XC9cL2NvZ25pdG8taWRwLmFwLXNvdXRoZWFzdC0xLmFtYXpvbmF3cy5jb21cL2FwLXNvdXRoZWFzdC0xX1FiMVE4VFBzVSIsImNvZ25pdG86dXNlcm5hbWUiOiI0Y2Y5MmRlNy05ZmQ0LTQyMjgtYTg2Mi1kYmM4YWUyYTM2MzEiLCJvcmlnaW5fanRpIjoiZjI0ODRiZGMtOWVlYi00YjYyLWI1MDktOGYxY2IzMmFhZGYwIiwiYXVkIjoic2lrY25laTR0MmgzbnRrcWo1ZDQ5bHR2ciIsImV2ZW50X2lkIjoiMzUyOTYzZTQtZjY5MS00OGZiLTk2MWUtMGZlZTczODc4YzdmIiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE2NjkxNzIwNDIsIm5hbWUiOiJOR1VZRU4gSE9ORyBUSEFJIiwiZXhwIjoxNjY5MjgwMjgzLCJpYXQiOjE2NjkxOTM4ODMsImp0aSI6IjEwY2E1MDM1LWFlNGItNDU0NC05OWJlLWZjY2NlOWQyY2Q3NSIsImVtYWlsIjoibmd1eWVuaG9uZ3RoYWkyODA0MjAwMkBnbWFpbC5jb20ifQ.eiJ8dJ3oaBnxTUdvjlR7WbfrW1eJZyI67WccgJlfnbpMZ2nsxTn3FoiWUCQZ01y9HSYHUf9qIbvlLdIKnpkuBzzRpgTajWveYuR4vZsua1WW-1NyeR-SFr8s9I2KxWFU-0CfStdGij_NeGBHhnl1b1IVzKtdaQixnO74JKQ8WNSZXsdobB1ATCyhB74V8TSeEHyFIaXeyclA0CDb-b93Y_KXso_n_JHcdhQ9mOuknR1V6SXqFDQjzwhIV7Ns84EheEOnxtaKv_ENDZo489dhviJAaCqQlXVS9HguXNHbB8xuqM7pp8sTMEvsAlgsgL-Wyl-WeIgeAD0PYqXxSIIR1A");
            Console.WriteLine(client.DefaultRequestHeaders.Authorization);
            var response = await client.PostAsync($"login", httpContent);
            Console.WriteLine(response.StatusCode);
            return await response.Content.ReadAsStringAsync()!; ;
        }
    }
}