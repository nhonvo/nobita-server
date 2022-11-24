using HDBank.Core.Aggregate;
using HDBank.Core.Aggregate.ChangePassword;
using HDBank.Core.Aggregate.GetKey;
using HDBank.Core.Aggregate.Login;
using HDBank.Core.Aggregate.RefreshToken;
using HDBank.Core.Aggregate.Register;
using HDBank.Core.Interfaces;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
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
        public async Task<BankResponse<LoginResponseData>> Login(BankRequest<LoginRequestData> request)
        {
            var client = _httpClientFactory.CreateClient("HDBank");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("access-token", "eyJraWQiOiJXcDRGMndiQVpMa1d2WWgyNDhnYjNtUHBLRzZTdDRNcG85Tmc3U2diZ2E0PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiI0MGM1OGU1ZC05ZjMxLTRmOGQtOGZmMC0xZDVkZTZhMzQxM2YiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiaXNzIjoiaHR0cHM6XC9cL2NvZ25pdG8taWRwLmFwLXNvdXRoZWFzdC0xLmFtYXpvbmF3cy5jb21cL2FwLXNvdXRoZWFzdC0xX1FiMVE4VFBzVSIsImNvZ25pdG86dXNlcm5hbWUiOiI0MGM1OGU1ZC05ZjMxLTRmOGQtOGZmMC0xZDVkZTZhMzQxM2YiLCJvcmlnaW5fanRpIjoiNjA1OGEyZjQtZjcxZS00MzgyLTlhMjMtM2I5MTczNzg3YTQxIiwiYXVkIjoic2lrY25laTR0MmgzbnRrcWo1ZDQ5bHR2ciIsImV2ZW50X2lkIjoiMTkzYzM4M2MtYzcyMy00MjQ3LWIyM2MtMGE3YzM4MTgyNzYzIiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE2NjkyMTUxNzMsIm5hbWUiOiJUcnVvbmdOaG9uIiwiZXhwIjoxNjY5Mzc5ODk1LCJpYXQiOjE2NjkyOTM0OTUsImp0aSI6ImIwODgxZmI0LTViYzEtNDVkMS1hZTI4LTllMzYwMjFiOGJlNCIsImVtYWlsIjoidm90aHVvbmd0cnVvbmduaG9uMjAwMkBnbWFpbC5jb20ifQ.uR0MULrVjCTXXfu9vvMuD-_vOxxNoCnqbcqGhu_rF3CCR0VlGs1heBaViJHaQ9qKp8b2wGf1wUgE19bt99XFWJVx0CoMdWcISX7tZFX0jcm6OUoQrioR19Le9YWuYjrp8xIcqdcb_-vUNyrPD4hj8qMNdbC7pbYQzkLCkQnvghnGBWndCEO09UkgQryOPjTHBk-CE5-JhmrakO7Gx_Dsg8s4nTJ6ZYUexbYAnyOwHGFbmNdkBNu9ACYBS4RyT0D6ukTK9CNjrQkBczj7QwR04DotHf9Tw8AN5E2RKnPoDQajpgsnhx0jAUDVuqh8cap54EME2kDHa_TqPOseDqSWqA");
            var response = await client.PostAsync($"login", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync()!;
                var obj = JsonConvert.DeserializeObject<BankResponse<LoginResponseData>>(content);
                return obj;
            }
            return new BankResponse<LoginResponseData>();

        }

        public async Task<BankResponse<RegisterResponseData>> Register(BankRequest<RegisterRequestData> request)
        {
            var client = _httpClientFactory.CreateClient("HDBank");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("access-token", "eyJraWQiOiJXcDRGMndiQVpMa1d2WWgyNDhnYjNtUHBLRzZTdDRNcG85Tmc3U2diZ2E0PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiI0MGM1OGU1ZC05ZjMxLTRmOGQtOGZmMC0xZDVkZTZhMzQxM2YiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiaXNzIjoiaHR0cHM6XC9cL2NvZ25pdG8taWRwLmFwLXNvdXRoZWFzdC0xLmFtYXpvbmF3cy5jb21cL2FwLXNvdXRoZWFzdC0xX1FiMVE4VFBzVSIsImNvZ25pdG86dXNlcm5hbWUiOiI0MGM1OGU1ZC05ZjMxLTRmOGQtOGZmMC0xZDVkZTZhMzQxM2YiLCJvcmlnaW5fanRpIjoiNjA1OGEyZjQtZjcxZS00MzgyLTlhMjMtM2I5MTczNzg3YTQxIiwiYXVkIjoic2lrY25laTR0MmgzbnRrcWo1ZDQ5bHR2ciIsImV2ZW50X2lkIjoiMTkzYzM4M2MtYzcyMy00MjQ3LWIyM2MtMGE3YzM4MTgyNzYzIiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE2NjkyMTUxNzMsIm5hbWUiOiJUcnVvbmdOaG9uIiwiZXhwIjoxNjY5Mzc5ODk1LCJpYXQiOjE2NjkyOTM0OTUsImp0aSI6ImIwODgxZmI0LTViYzEtNDVkMS1hZTI4LTllMzYwMjFiOGJlNCIsImVtYWlsIjoidm90aHVvbmd0cnVvbmduaG9uMjAwMkBnbWFpbC5jb20ifQ.uR0MULrVjCTXXfu9vvMuD-_vOxxNoCnqbcqGhu_rF3CCR0VlGs1heBaViJHaQ9qKp8b2wGf1wUgE19bt99XFWJVx0CoMdWcISX7tZFX0jcm6OUoQrioR19Le9YWuYjrp8xIcqdcb_-vUNyrPD4hj8qMNdbC7pbYQzkLCkQnvghnGBWndCEO09UkgQryOPjTHBk-CE5-JhmrakO7Gx_Dsg8s4nTJ6ZYUexbYAnyOwHGFbmNdkBNu9ACYBS4RyT0D6ukTK9CNjrQkBczj7QwR04DotHf9Tw8AN5E2RKnPoDQajpgsnhx0jAUDVuqh8cap54EME2kDHa_TqPOseDqSWqA");
            var response = await client.PostAsync($"register", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync()!;
                var obj = JsonConvert.DeserializeObject<BankResponse<RegisterResponseData>>(content);
                return obj;
            }
            return new BankResponse<RegisterResponseData>();
        }

        public async Task<RefreshTokenResponse> RefeshToken()
        {
            RefreshTokenRequest request = new RefreshTokenRequest() { 
                ClientId= "sikcnei4t2h3ntkqj5d49ltvr",
                GrantType = "refresh_token",
                RefreshToken= "eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ.K3SSviSa9APrwYEdwyFI6DPFXavdCN4Ckzynu91yyS3mDLSH0yGr2FCkTzooCTWCo4r9LDzHDYgJj5ie0OgF9cXNPq6y1iDudyJnHLeTSQjsN91JxZaZwc846f3IgsJ6GrZkpkXUoUG3dcQCAKXAWqQ8N9PEI-Y_EmotJgwL8JuprSiEUhOOohB3sK2BFwaD7N_eIvYvmWcnEb_u_l8L6Jansy07x0FG2bcCcIJ2TpmFRXAKIEtoOcg5ocBuycHSidMCL6CBkqessAaExk_OAifYh3E3A41O6pqKWztkvIgG-QT39_bLZfV0Xp2r_CRCxGmSP9m8ghyaO3nwTJdlRA.gbbHZ-pWhYzG7GBG.eWvBOCM3xeZSSP2KMWSt6NDGTN2g0AJN-I_-VhoQncm7FMU65io4HoV-uywNzOiYTBfmV5AE-RtFyK911KcHwVeNCHGAogNO9MoC79F9KrK14GYZYYZmBCbhQTmkGmqgjBXc3Ax-WWFaiOL1f6euW_8E0bLydJg3Aj1t7Rx_F166f9gUShJEBUsUKksOSQivzja2G797qZ4dJZnL0OlzBMu0RzIFtdeybD03HhBafV4DObl5SbxaD8EUBVEqs-CGzQtLAUFTv5eUrj7tkDr9HsK1iauIVcqGLlXX7l7ZrNK9kCaMDo2lnwmUX_jevVN4R0Ba9bi-q6g8afJU3DWj_-YfFeLubk4Ct-x1ZcS32s3GvMMwv-1Z1qxW1MdiB6hL7vTpIp3wGuTBShbfJcf7Y1DwkL8mUal73uqcPPeI2bTAieT8Knx0m6JsFuDLMr-aBBBQQBguThQxeew5n3gEuC0xtbsSvGB-fwneWXlTPGYWHCS8Kla6kK1IJ6nXSIgcMpWe_BerKeh0a7lcelaM4VqCsqXF8fCGi2GeVYe9ViXt1HtNuad7snNZBiXPJmMSZw7a1RqWQSH6HpqfTwKwOk_BKoLZl79hxvpjrE8a6e8kvBXV1RwwxEnReQp3o4GxJCkdHdWzMSSviOPDi23hnxSlIKeGaI0R11A1eU8MnwZ0FwHIK8D1PSVK4kgzgIJNcANDSIqUMo1L-n-BNa_5AEMoekDCVz3S0CPOE-x6DzM8183C29GY0SB8lJVNCLz6d-HhJu3YispNdzZ7LLdH7AaNQ4RXVLGbQpaUII8D_xhrQ9bn_7l4HlUd3rhPIWFwnrRv33uzJ3WpXVjYuI8ENMGwNBOECe64y6ZwTeny5xupLgmy53qOOja0am1h6a228RwVnUU8Mf7xFgcrLLazGLljqlII8fW5_MUTcBrb6qETyqJdGbzd4-9cY7KlrrkXZ8hXzipn8rVhfAbNG_UWbDYwv5b8OeQizokKnMuPFdnZS-4t0xDzvwcJKfxof4wHkGEwOWBUVDel1hB-32cRk6H8UWYA9ctAUxxJVf_gqqjc4n5ok-MPofb5MpzKRUCpamHTCSejzA1XD8BB-Zfb_AgVkrLVZo3m5qCirXBOk5pZ1O7oTkdQcZr4ezbL93FnkKSWzJ4HTExmbOZH6H8rh8Nda2f1fvz77A45neJ4I45Yw8eu5SWRuQ6ZZKO6mLuWQyqWm78fK4DNn1vxbI5ksG-wGnt-1QUSElruw-IUSxA6nLmagGcPrPlSj0D5WAfAubxXZTY9KDSwVu9TcYjkI3BjDK-s4W80gvNuEuigf5qeUlRJUE8pfTPYskFwmYn-s_cQrTkLVQ.Ug5ANUilgF_mQHqhTiGnxw"
            };

            var client = _httpClientFactory.CreateClient("Refresh");
            var str = $"client_id={request.ClientId}&grant_type={request.GrantType}&refresh_token={request.RefreshToken}";
            var httpContent = new StringContent(str, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync($"token", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync()!;
                var obj = JsonConvert.DeserializeObject<RefreshTokenResponse>(content);
                return obj;
            }
            return new RefreshTokenResponse();

        }

        public async Task<BankResponse<ChangePasswordResponseData>> ChangePassword(BankRequest<ChangePasswordRequestData> request)
        {
            var client = _httpClientFactory.CreateClient("HDBank");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("access-token", "eyJraWQiOiJXcDRGMndiQVpMa1d2WWgyNDhnYjNtUHBLRzZTdDRNcG85Tmc3U2diZ2E0PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiI0MGM1OGU1ZC05ZjMxLTRmOGQtOGZmMC0xZDVkZTZhMzQxM2YiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiaXNzIjoiaHR0cHM6XC9cL2NvZ25pdG8taWRwLmFwLXNvdXRoZWFzdC0xLmFtYXpvbmF3cy5jb21cL2FwLXNvdXRoZWFzdC0xX1FiMVE4VFBzVSIsImNvZ25pdG86dXNlcm5hbWUiOiI0MGM1OGU1ZC05ZjMxLTRmOGQtOGZmMC0xZDVkZTZhMzQxM2YiLCJvcmlnaW5fanRpIjoiNjA1OGEyZjQtZjcxZS00MzgyLTlhMjMtM2I5MTczNzg3YTQxIiwiYXVkIjoic2lrY25laTR0MmgzbnRrcWo1ZDQ5bHR2ciIsImV2ZW50X2lkIjoiMTkzYzM4M2MtYzcyMy00MjQ3LWIyM2MtMGE3YzM4MTgyNzYzIiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE2NjkyMTUxNzMsIm5hbWUiOiJUcnVvbmdOaG9uIiwiZXhwIjoxNjY5Mzc5ODk1LCJpYXQiOjE2NjkyOTM0OTUsImp0aSI6ImIwODgxZmI0LTViYzEtNDVkMS1hZTI4LTllMzYwMjFiOGJlNCIsImVtYWlsIjoidm90aHVvbmd0cnVvbmduaG9uMjAwMkBnbWFpbC5jb20ifQ.uR0MULrVjCTXXfu9vvMuD-_vOxxNoCnqbcqGhu_rF3CCR0VlGs1heBaViJHaQ9qKp8b2wGf1wUgE19bt99XFWJVx0CoMdWcISX7tZFX0jcm6OUoQrioR19Le9YWuYjrp8xIcqdcb_-vUNyrPD4hj8qMNdbC7pbYQzkLCkQnvghnGBWndCEO09UkgQryOPjTHBk-CE5-JhmrakO7Gx_Dsg8s4nTJ6ZYUexbYAnyOwHGFbmNdkBNu9ACYBS4RyT0D6ukTK9CNjrQkBczj7QwR04DotHf9Tw8AN5E2RKnPoDQajpgsnhx0jAUDVuqh8cap54EME2kDHa_TqPOseDqSWqA");
            var response = await client.PostAsync($"change_password", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync()!;
                var obj = JsonConvert.DeserializeObject<BankResponse<ChangePasswordResponseData>>(content);
                return obj;
            }
            return new BankResponse<ChangePasswordResponseData>();
        }
    }
}