using System.Numerics;
using Newtonsoft.Json;

namespace Module.Core.Converters;

public class BigIntegerConverter : JsonConverter<BigInteger>
{
    public override void WriteJson(JsonWriter writer, BigInteger value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override BigInteger ReadJson(
        JsonReader reader,
        Type objectType,
        BigInteger existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var valueStr = reader.ReadAsString() ?? string.Empty;
        return BigInteger.Parse(valueStr);
    }
}