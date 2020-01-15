using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace BankingApplication
{
    //Custom JsonConverter for handling account types
    class AccountConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Account);
        }
        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Use default serialization.");
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var accounts = AccountFactory.CreateAccount(jsonObject.Value<int>("AccountNumber"),
                                                        jsonObject.Value<char>("AccountType"),
                                                        jsonObject.Value<int>("CustomerID"),
                                                        jsonObject.Value<decimal>("Balance"));
            
            serializer.Populate(jsonObject.CreateReader(), accounts);
            return accounts;
        }
    }
}
