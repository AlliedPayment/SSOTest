using Newtonsoft.Json;

namespace SSOTest
{
    public class Account
    {
        [JsonProperty("AccountCIF")]
        public string AccountCif { get; set; }

        [JsonProperty("AccountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty("AccountName")]
        public string AccountName { get; set; }

        [JsonProperty("AvailableAccountBalance")]
        public long AvailableAccountBalance { get; set; }

        [JsonProperty("CollectedAccountBalance")]
        public long CollectedAccountBalance { get; set; }

        [JsonProperty("RoutingNumber")]
        public string RoutingNumber { get; set; }

        [JsonProperty("AccountOwnerType")]
        public string AccountOwnerType { get; set; }

        [JsonProperty("AccountType")]
        public string AccountType { get; set; }
    }
}