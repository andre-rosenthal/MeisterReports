using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Messages
/// </summary>
/// 
[JsonObject("Messages")]
public class Messages
{
    public Messages()
    {
        
    }

    [JsonProperty("TYPE")]
    public string Type { get; set; }
    [JsonProperty("ID")]
    public string Id { get; set; }
    [JsonProperty("NUMBER")]
    public string Number { get; set; }
    [JsonProperty("MESSAGE")]
    public string Message { get; set; }
    [JsonProperty("LOG_NO")]
    public string LogNo { get; set; }
    [JsonProperty("LOG_MSG_NO")]
    public string LogMsgNo { get; set; }
    [JsonProperty("MESSAGE_V1")]
    public string Message_V1 { get; set; }
    [JsonProperty("MESSAGE_V2")]
    public string Message_V2 { get; set; }
    [JsonProperty("MESSAGE_V3")]
    public string Message_V3 { get; set; }
    [JsonProperty("MESSAGE_V4")]
    public string Message_V4 { get; set; }
    [JsonProperty("PARAMETER")]
    public string Parameter { get; set; }
    [JsonProperty("ROW")]
    public string Row { get; set; }
    [JsonProperty("FIELD")]
    public string Field { get; set; }
    [JsonProperty("SYSTEM")]
    public string System { get; set; }
}
