using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Warsmiths.Common.Domain;

namespace Assets.Scripts
{
    public class WarsmithsClientConverter : JsonConverter
    {

        private Type[] Types { get; set; }

        public WarsmithsClientConverter()
        {
            Types = Assembly.Load("Warsmiths.Common").GetTypes();
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IEntity));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var values = jo["_t"].Values<JToken>();
            var lastValue = values.Last();
            if (lastValue != null)
            {
                var val = lastValue.Value<string>(lastValue);

                var specificType = Types.FirstOrDefault(t => t.Name == val);

                var firedObject = jo.ToObject(specificType);
                return firedObject;
            }
            return null;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        } 

    }
}
