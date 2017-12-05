using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace SSOTest
{
    public class SsoResponse
    {
        public string Uri { get; set; }
    }
    public class SSoTest
    {
        const string PrivateKey = "PRIVATE_KEY_HERE";
        const string PublicKey = "PUBLIC_KEY_HERE";
        const string Domain = "FI_DOMAIN_HERE";
        const string Root = "https://api.alliedpayment.com";
        const string Endpoint = "sso";
        const string JsonMime = "application/json";
        
        [Fact]
        public async void SSO_Request()
        {
            var ssoToken = new
            {
                Application = "BILLPAY",
                CustomerId = "test",
                FinancialInstitutionId = "ALLIED",
                FirstName = "Support",
                LastName = "User",
                Email = "support.user@alliedpayment.com",
                Timeout = 50,
                Accounts = new List<object>
                {
                    new
                    {
                        AccountCIF = "12345",
                        AccountNumber = "1234",
                        AccountName = "My Bank Account",
                        AvailableAccountBalance = 1000,
                        CollectedAccountBalance = 1000,
                        RoutingNumber = "074000010",
                        AccountOwnerType = "Personal",
                        AccountType = "Checking"
                    }
                },
                Address1 = "3201 Stellhorn Road",
                City = "Fort Wayne",
                State = "IN",
                Zip = "46815",
                DailyLimit = 1000,
                UserName = "support",
                ForeignKey = "support"
            };
            
            var client = new HttpClient() { BaseAddress = new Uri(Root) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMime));
            var authHeader = GetAuthorizationHeader($"{Root}/{Endpoint}", Domain);
            Console.WriteLine($"Authorization Header: {authHeader}");
            client.DefaultRequestHeaders.Remove("Authorization");
            client.DefaultRequestHeaders.Add("Authorization", authHeader);
            var json = JsonConvert.SerializeObject(ssoToken);
            var response = await client.PostAsync(Endpoint, new StringContent(json, Encoding.UTF8, JsonMime));
            Assert.True((int)response.StatusCode == 200);
            json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SsoResponse>(json);
            Assert.True(result.Uri.Length > 0);
            Console.WriteLine(result.Uri);
        }


        private string CreateSignature(string url, string timestamp)
        {
        var message = new StringBuilder();
        message.Append(url + "\r\n");
        if (!string.IsNullOrEmpty(timestamp)) message.Append(timestamp + "\r\n");
        var bytes = Encoding.UTF8.GetBytes(message.ToString());
        var key = Encoding.UTF8.GetBytes(PrivateKey);
        var hmac = new HMACSHA1(key);
        var hash = hmac.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
        }

        public string GetAuthorizationHeader(string url, string domain = null, string username = null)
        {
        var timestamp = DateTime.UtcNow.ToString("o");
        var signature = CreateSignature(url, timestamp);
        domain = (domain != null) ? domain : Domain;
        var header = new StringBuilder("TIMESTAMP ");
        if (!string.IsNullOrEmpty(username)) header.Append(string.Format("username={0};", username));
        if (!string.IsNullOrEmpty(domain)) header.Append(string.Format("domain={0};", domain));
        header.Append(string.Format("timestamp={0};signature={1};publickey={2}", timestamp, signature, PublicKey));
        return header.ToString();
        }
    }
}
