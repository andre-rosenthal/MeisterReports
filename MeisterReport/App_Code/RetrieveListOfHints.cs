using Newtonsoft.Json;
using System.Collections.Generic;

public class BindResults
{
    public string type { get; set; }
    public string name { get; set; }
    public string value { get; set; }
}

[JsonObject("RESULTSET")]
public class ResultSet
{
    [JsonProperty("RESULTS")]
    public List<ResultBody> results { get; set; }

}

[JsonObject("RESULTBODY",Title = "Result from Body")]
public class ResultBody
{
    [JsonProperty("ENUM_ID")]
    public string Enum_id { get; set; }
    [JsonProperty("ENUMS")]
    public List<Enum> Enums { get; set; }
}

[JsonObject("ENUM")]
public class Enum
{
    [JsonProperty("NAME")]
    public string Name { get; set; }
    [JsonProperty("VALUE")]
    public string value { get; set; }
}

