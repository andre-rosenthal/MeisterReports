using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EndPoint
/// </summary>
public class EndPoint
{
    public EndPoint()
    {
        Parm = new Parm();
    }
    public string Handler { get; set; }
    public Parm Parm { get; set; }

}

[JsonObject("Parms")]
public class Parm
{
    [JsonProperty("COMPRESSION")]
    public string Compression { get; set; }
    [JsonProperty("TEST_RUN")]
    public string Testrun { get; set; }
    [JsonProperty("STYLE")]
    public string Style { get; set; }
    [JsonProperty("SDK_HINT")]
    public string SDKHint { get; set; }
    public Parm()
    {
        Style = "Default";
        Testrun = string.Empty;
        Compression = string.Empty;
        SDKHint = string.Empty;
    }
}