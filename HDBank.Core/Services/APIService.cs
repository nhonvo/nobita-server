using HDBank.Core.Aggregate;
using HDBank.Core.Interfaces;
using Newtonsoft.Json;
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
                byte[] decryptedData;

                //Create a new instance of RSACryptoServiceProvider to generate
                //public and private key data.
                using RSACryptoServiceProvider RSA = new();

                var bytePublicKey = ByteConverter.GetBytes(publicKey);
                RSAParameters parameters = RSA.ExportParameters(false);
                parameters.Modulus = RSAService.GetModulus(bytePublicKey);
                parameters.Exponent = RSAService.GetExponent(bytePublicKey);
                RSA.ImportParameters(parameters);

                //Pass the data to ENCRYPT, the public key information 
                //(using RSACryptoServiceProvider.ExportParameters(false),
                //and a boolean flag specifying no OAEP padding.
                encryptedData = RSAService.RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);

                //Pass the data to DECRYPT, the private key information 
                //(using RSACryptoServiceProvider.ExportParameters(true),
                //and a boolean flag specifying no OAEP padding.
                decryptedData = RSAService.RSADecrypt(encryptedData, RSA.ExportParameters(true), false);

                //Display the decrypted plaintext to the console. 
                Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));
                return ByteConverter.GetString(encryptedData);
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
            var response = await client.GetAsync($"/get_key");
            return await response.Content.ReadAsStringAsync()!; ;
        }
        public async Task<string> Login(BankRequest<LoginRequest> request)
        {
            var client = _httpClientFactory.CreateClient("HDBank");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/login", httpContent);
            return await response.Content.ReadAsStringAsync()!; ;
        }
    }
}