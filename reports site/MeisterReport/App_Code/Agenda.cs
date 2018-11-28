using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class User
{
    public String Userid { get; set; }
}
/// <summary>
/// Query ....
/// </summary>
public class AgendaQuery
{
    public List<Agenda> AGENDA { get; set; }
}

public class AgendaBind
{
    public string Schedule_Type { get; set; }
    public string WeekDay { get; set; }
    public int TimeSlot { get; set; }
    public string NickName { get; set; }
    public string UUID { get; set; }
    public string UserName { get; set; }
}
public class Agenda
{
    public string AGENDA_TYPE { get; set; }
    public string DOW { get; set; }
    public int SLOT { get; set; }
    public string NICKNAME { get; set; }
    public string PKY { get; set; }
    public string USERNAME { get; set; }
    public string DATESTAMP { get; set; }
    public string TIMESTAMP { get; set; }
    public ReportDatum REPORT { get; set; }
    public string STATUS { get; set; }
    public string EMAIL { get; set; }
    public string VIA_EMAIL { get; set; }
    public string WITH_METADATA { get; set; }
    public string COLUMNS_NAMED { get; set; }
}

public class AddAgenda
{
    public string PKY { get; set; }
    public string DOW { get; set; }
    public string NICKNAME { get; set; }
    public string SCHEDULE_TYPE { get; set; }
    public string SLOT { get; set; }
    public ReportDatumA REPORT { get; set; }
    public string DELETE { get; set; }
}

public partial class ReportDatumA
{
    [JsonProperty("PKY")]
    public string Pky { get; set; }

    [JsonProperty("DATESTAMP")]
    public string Date { get; set; }
    [JsonProperty("TIMESTAMP")]
    public string Time { get; set; }

    [JsonProperty("USERNAME")]
    public string Username { get; set; }

    [JsonProperty("REPORT")]
    public Report Report { get; set; }

    [JsonProperty("STATUS")]
    public string Status { get; set; }

    [JsonProperty("EMAIL")]
    public string Email { get; set; }

    [JsonProperty("VIA_EMAIL")]
    public string ViaEmail { get; set; }

    [JsonProperty("WITH_METADATA")]
    public string WithMetadata { get; set; }

    [JsonProperty("COLUMNS_NAMED")]
    public string ColumnsNamed { get; set; }
}
