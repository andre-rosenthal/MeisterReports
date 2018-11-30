    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

public partial class Scheduler
{
    [JsonProperty("columns_named")]
    public string ColumnsNamed { get; set; }

    [JsonProperty("parameters")]
    public List<ParameterSch> Parameters { get; set; }

    [JsonProperty("report")]
    public string Report { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("variant")]
    public string Variant { get; set; }

    [JsonProperty("with_metadata")]
    public string WithMetadata { get; set; }
}

public partial class ParameterSch
{
    [JsonProperty("high")]
    public string High { get; set; }

    [JsonProperty("kind")]
    public string Kind { get; set; }

    [JsonProperty("low")]
    public string Low { get; set; }

    [JsonProperty("option")]
    public string Option { get; set; }

    [JsonProperty("selname")]
    public string Selname { get; set; }

    [JsonProperty("sign")]
    public string Sign { get; set; }
}

public partial class Scheduler
{
    public Scheduler()
    {
        Parameters = new List<ParameterSch>();
    }
    public static List<Scheduler> FromJson(string json) => JsonConvert.DeserializeObject<List<Scheduler>>(json);
}

public partial class SchedulerResponse
{
    [JsonProperty("PKY")]
    public string Pky { get; set; }

    [JsonProperty("REPORT_NAME")]
    public string ReportName { get; set; }

    [JsonProperty("STATUS")]
    public string Status { get; set; }

    [JsonProperty("USER_NAME")]
    public string UserName { get; set; }

    [JsonProperty("MESSAGE")]
    public string Message { get; set; }
}

public partial class SchedulerResponse
{
    public static List<SchedulerResponse> FromJson(string json) => JsonConvert.DeserializeObject<List<SchedulerResponse>>(json);
}


