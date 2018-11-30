
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class SchedulerSet
    {
        [JsonProperty("REPORT_GUID")]
        public string ReportGuid { get; set; }
        [JsonProperty("USERID")]
        public string UserId { get; set; }
    }

    public partial class SchedulerSet
    {
        public static SchedulerSet[] FromJson(string json) => JsonConvert.DeserializeObject<SchedulerSet[]>(json);
    }

    public partial class RetrieveSet
    {
        [JsonProperty("REPORT_DATA")]
        public string ReportData { get; set; }
    }

    public partial class RetrieveSet
    {
        public static List<RetrieveSet> FromJson(string json) => JsonConvert.DeserializeObject<List<RetrieveSet>>(json);
    }



