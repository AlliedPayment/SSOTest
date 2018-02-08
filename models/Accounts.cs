using System.Collections.Generic;
using Newtonsoft.Json;

namespace SSOTest
{
    public class Accounts
    {
        [JsonProperty("$values")]
        public List<Account> Values { get; set; }
    }
}