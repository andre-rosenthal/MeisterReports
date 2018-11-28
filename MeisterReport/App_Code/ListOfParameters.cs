using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class ParmBind
{
    public enum KindTypes
    {
        Parameter,
        Selection
    }
    public string ParameterName { get; set; }
    public string Kind { get; set; }
    public string Description { get; set; }
    public string Operation { get; set; }
    public string From { get; set; }
    public string To { get; set; }
}

[JsonObject("PARMSET")]
public partial class ParmSet
{
    [JsonProperty("SELECTIONS")]
    public Parameters Parameters { get; set; }
}

[JsonObject("SELECTIONS")]
public partial class Parameters
{
    [JsonProperty("METADATA")]
    public List<Metadatum> Metadata { get; set; }

    [JsonProperty("MESSAGES")]
    public List<Messages> Messages { get; set; }
}

[JsonObject("METADATA")]
public partial class Metadatum
{
    [JsonProperty("PARAMETERS")]
    public List<ParameterElement> Parameter { get; set; }

    [JsonProperty("VARIANTS")]
    public object[] Variants { get; set; }

    [JsonProperty("LAYOUTS")]
    public object[] Layouts { get; set; }
}

[JsonObject("PARAMETERS")]
public partial class ParameterElement
{
    [JsonProperty("PARAMETER")]
    public ParameterBody Parameter { get; set; }

    [JsonProperty("DEFAULT")]
    public Default Default { get; set; }

    [JsonProperty("SEARCH_HELP")]
    public string SearchHelp { get; set; }
    [JsonProperty("DESCRIPTION")]
    public string Description { get; set; }
}
[JsonObject("DEFAULT")]
public partial class Default
{
    [JsonProperty("SELNAME")]
    public string Selname { get; set; }

    [JsonProperty("KIND")]
    public string Kind { get; set; }

    [JsonProperty("SIGN")]
    public string Sign { get; set; }

    [JsonProperty("OPTION")]
    public string Option { get; set; }

    [JsonProperty("LOW")]
    public string Low { get; set; }

    [JsonProperty("HIGH")]
    public string High { get; set; }
}
[JsonObject("PARAMETER")]
public partial class ParameterBody
{
    [JsonProperty("NAME")]
    public string Name { get; set; }

    [JsonProperty("KIND")]
    public string Kind { get; set; }

    [JsonProperty("TYPE")]
    public string Type { get; set; }

    [JsonProperty("DTYP")]
    public string Dtyp { get; set; }

    [JsonProperty("DBFIELD")]
    public string Dbfield { get; set; }

    [JsonProperty("NOSELSET")]
    public string Noselset { get; set; }

    [JsonProperty("OBLIGATORY")]
    public string Obligatory { get; set; }

    [JsonProperty("DYNNR")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long Dynnr { get; set; }
}

public partial class ParmSet
{
    public static List<ParmSet> FromJson(string json) => JsonConvert.DeserializeObject<List<ParmSet>>(json);
}

public static class Serialize
{
    public static string ToJson<T>(this T self) => JsonConvert.SerializeObject(self);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
}

internal class ParseStringConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        long l;
        if (Int64.TryParse(value, out l))
        {
            return l;
        }
        throw new Exception("Cannot unmarshal type long");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (long)untypedValue;
        serializer.Serialize(writer, value.ToString());
        return;
    }

    public static readonly ParseStringConverter Singleton = new ParseStringConverter();
}

