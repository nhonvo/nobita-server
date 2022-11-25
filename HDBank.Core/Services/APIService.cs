using HDBank.Core.Aggregate;
using HDBank.Core.Aggregate.Balance;
using HDBank.Core.Aggregate.ChangePassword;
using HDBank.Core.Aggregate.GetKey;
using HDBank.Core.Aggregate.Login;
using HDBank.Core.Aggregate.RefreshToken;
using HDBank.Core.Aggregate.Register;
using HDBank.Core.Aggregate.Tranfer;
using HDBank.Core.Aggregate.TranferHistory;
using HDBank.Core.Interfaces;
using Newtonsoft.Json;
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
        public String AccessToken = "eyJraWQiOiJXcDRGMndiQVpMa1d2WWgyNDhnYjNtUHBLRzZTdDRNcG85Tmc3U2diZ2E0PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiI0MGM1OGU1ZC05ZjMxLTRmOGQtOGZmMC0xZDVkZTZhMzQxM2YiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiaXNzIjoiaHR0cHM6XC9cL2NvZ25pdG8taWRwLmFwLXNvdXRoZWFzdC0xLmFtYXpvbmF3cy5jb21cL2FwLXNvdXRoZWFzdC0xX1FiMVE4VFBzVSIsImNvZ25pdG86dXNlcm5hbWUiOiI0MGM1OGU1ZC05ZjMxLTRmOGQtOGZmMC0xZDVkZTZhMzQxM2YiLCJvcmlnaW5fanRpIjoiNjA1OGEyZjQtZjcxZS00MzgyLTlhMjMtM2I5MTczNzg3YTQxIiwiYXVkIjoic2lrY25laTR0MmgzbnRrcWo1ZDQ5bHR2ciIsImV2ZW50X2lkIjoiMTkzYzM4M2MtYzcyMy00MjQ3LWIyM2MtMGE3YzM4MTgyNzYzIiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE2NjkyMTUxNzMsIm5hbWUiOiJUcnVvbmdOaG9uIiwiZXhwIjoxNjY5MzYzNDAwLCJpYXQiOjE2NjkyNzcwMDEsImp0aSI6ImVmZGVmOWI5LTk0NDAtNDExNi1iMmJmLTJhZDQ0ZWZlNjIxMyIsImVtYWlsIjoidm90aHVvbmd0cnVvbmduaG9uMjAwMkBnbWFpbC5jb20ifQ.2nKS5naPP99v-82xpVJn5T0UTqKzU0ql5CBA54AWvQTxiPk1ehs405OwNtRnoBUN7muDIhL59FVlQ3DcniRTHqNQUfYUy458bPYtlVoAxhNTmYrr6L5N8N5FP8EE9kIk1cK9t4gsLhKCurHKG7y-3h5q39QCPmoL_sqrDbMzl3T2izgfuJP-nbU19TbQWS28s7oqBMmOHytKm5e5ya1wMEos5lHUpL1p371LN3jztzW15cXOEnDPDZcxJHpz1pv7HKc5e7MO32fVKdtULNDfp6a1fCfh0hIRcNUkQ2aGINXdD5VimrOys0__V45sqOIbTUA7wIRjq8YZX5ErRllmUA";
        public String RefreshToken = "eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ.K3SSviSa9APrwYEdwyFI6DPFXavdCN4Ckzynu91yyS3mDLSH0yGr2FCkTzooCTWCo4r9LDzHDYgJj5ie0OgF9cXNPq6y1iDudyJnHLeTSQjsN91JxZaZwc846f3IgsJ6GrZkpkXUoUG3dcQCAKXAWqQ8N9PEI-Y_EmotJgwL8JuprSiEUhOOohB3sK2BFwaD7N_eIvYvmWcnEb_u_l8L6Jansy07x0FG2bcCcIJ2TpmFRXAKIEtoOcg5ocBuycHSidMCL6CBkqessAaExk_OAifYh3E3A41O6pqKWztkvIgG-QT39_bLZfV0Xp2r_CRCxGmSP9m8ghyaO3nwTJdlRA.gbbHZ-pWhYzG7GBG.eWvBOCM3xeZSSP2KMWSt6NDGTN2g0AJN-I_-VhoQncm7FMU65io4HoV-uywNzOiYTBfmV5AE-RtFyK911KcHwVeNCHGAogNO9MoC79F9KrK14GYZYYZmBCbhQTmkGmqgjBXc3Ax-WWFaiOL1f6euW_8E0bLydJg3Aj1t7Rx_F166f9gUShJEBUsUKksOSQivzja2G797qZ4dJZnL0OlzBMu0RzIFtdeybD03HhBafV4DObl5SbxaD8EUBVEqs-CGzQtLAUFTv5eUrj7tkDr9HsK1iauIVcqGLlXX7l7ZrNK9kCaMDo2lnwmUX_jevVN4R0Ba9bi-q6g8afJU3DWj_-YfFeLubk4Ct-x1ZcS32s3GvMMwv-1Z1qxW1MdiB6hL7vTpIp3wGuTBShbfJcf7Y1DwkL8mUal73uqcPPeI2bTAieT8Knx0m6JsFuDLMr-aBBBQQBguThQxeew5n3gEuC0xtbsSvGB-fwneWXlTPGYWHCS8Kla6kK1IJ6nXSIgcMpWe_BerKeh0a7lcelaM4VqCsqXF8fCGi2GeVYe9ViXt1HtNuad7snNZBiXPJmMSZw7a1RqWQSH6HpqfTwKwOk_BKoLZl79hxvpjrE8a6e8kvBXV1RwwxEnReQp3o4GxJCkdHdWzMSSviOPDi23hnxSlIKeGaI0R11A1eU8MnwZ0FwHIK8D1PSVK4kgzgIJNcANDSIqUMo1L-n-BNa_5AEMoekDCVz3S0CPOE-x6DzM8183C29GY0SB8lJVNCLz6d-HhJu3YispNdzZ7LLdH7AaNQ4RXVLGbQpaUII8D_xhrQ9bn_7l4HlUd3rhPIWFwnrRv33uzJ3WpXVjYuI8ENMGwNBOECe64y6ZwTeny5xupLgmy53qOOja0am1h6a228RwVnUU8Mf7xFgcrLLazGLljqlII8fW5_MUTcBrb6qETyqJdGbzd4-9cY7KlrrkXZ8hXzipn8rVhfAbNG_UWbDYwv5b8OeQizokKnMuPFdnZS-4t0xDzvwcJKfxof4wHkGEwOWBUVDel1hB-32cRk6H8UWYA9ctAUxxJVf_gqqjc4n5ok-MPofb5MpzKRUCpamHTCSejzA1XD8BB-Zfb_AgVkrLVZo3m5qCirXBOk5pZ1O7oTkdQcZr4ezbL93FnkKSWzJ4HTExmbOZH6H8rh8Nda2f1fvz77A45neJ4I45Yw8eu5SWRuQ6ZZKO6mLuWQyqWm78fK4DNn1vxbI5ksG-wGnt-1QUSElruw-IUSxA6nLmagGcPrPlSj0D5WAfAubxXZTY9KDSwVu9TcYjkI3BjDK-s4W80gvNuEuigf5qeUlRJUE8pfTPYskFwmYn-s_cQrTkLVQ.Ug5ANUilgF_mQHqhTiGnxw";
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
                if (encryptedData == null)
                    throw new Exception(); 
                return Convert.ToBase64String(encryptedData);
            }
            catch (Exception)
            {
                //Catch this exception in case the encryption did
                //not succeed.
                return string.Empty;
            }
        }

        public async Task<BankResponse<GetKeyResponseData>> GetKey()
        {
            var client = _httpClientFactory.CreateClient("HDBank");
            client.DefaultRequestHeaders.Add("access-token", $"{AccessToken}");

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

            client.DefaultRequestHeaders.Add("access-token", $"{AccessToken}");
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

            client.DefaultRequestHeaders.Add("access-token", $"{AccessToken}");
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
            RefreshTokenRequest request = new RefreshTokenRequest()
            {
                ClientId = "sikcnei4t2h3ntkqj5d49ltvr",
                GrantType = "refresh_token",
                RefreshToken = $"{RefreshToken}"
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

            client.DefaultRequestHeaders.Add("access-token", $"{AccessToken}");
            var response = await client.PostAsync($"change_password", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync()!;
                var obj = JsonConvert.DeserializeObject<BankResponse<ChangePasswordResponseData>>(content);
                return obj;
            }
            return new BankResponse<ChangePasswordResponseData>();
        }

        public async Task<BankResponse<TransferResponseData>> Tranfer(BankRequest<TransferRequestData> request)
        {

            var client = _httpClientFactory.CreateClient("HDBank");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("access-token", $"{AccessToken}");

            var response = await client.PostAsync($"transfer", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync()!;
                var obj = JsonConvert.DeserializeObject<BankResponse<TransferResponseData>>(content);
                return obj;
            }
            return new BankResponse<TransferResponseData>();
        }

        public async Task<BankResponse<TranferHistoryResponseData>> TranferHistory(BankRequest<TranferHistoryRequestData> request)
        {


            var client = _httpClientFactory.CreateClient("HDBank");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("access-token", $"{AccessToken}");

            var response = await client.PostAsync($"tranhis", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync()!;
                var obj = JsonConvert.DeserializeObject<BankResponse<TranferHistoryResponseData>>(content);
                return obj;
            }
            return new BankResponse<TranferHistoryResponseData>();
        }

        public async Task<BankResponse<BalanceResponseData>> Balance(BankRequest<BalanceRequestData> request)
        {
            var client = _httpClientFactory.CreateClient("HDBank");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("access-token", $"{AccessToken}");
            
            var response = await client.PostAsync($"balance", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync()!;
                var obj = JsonConvert.DeserializeObject<BankResponse<BalanceResponseData>>(content);
                return obj;
            }
            return new BankResponse<BalanceResponseData>();
        }
    }
}