using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class User
{
    public String USERID { get; set; }
}
/// <summary>
/// Query ....
/// </summary>
public class AgendaQuery
{
    public List<Agenda> AGENDA { get; set; }
}


public class AgendaResult
{
    public string MESSAGE { get; set; }
    public string PKY { get; set; }
    public string REPORTNAME { get; set; }
    public string USERNAME { get; set; }
}

public class AgendaBind
{
    public string Schedule_Type { get; set; }
    public string WeekDay { get; set; }
    public string TimeSlot { get; set; }
    public string NickName { get; set; }
    public string UUID { get; set; }
    public string UserName { get; set; }
}
public class Agenda
{
    public string PKY { get; set; }
    public string AGENDA_TYPE { get; set; }
    public string DOW { get; set; }
    public string SLOT { get; set; }
    public string NICKNAME { get; set; }
    public string USERID { get; set; }
    public string DELETE { get; set; }
    [JsonProperty("NAME")]
    public string Name { get; set; }
    [JsonProperty("VARIANT")]
    public string Variant { get; set; }
    [JsonProperty("WITH_METADATA")]
    public string WithMetadata { get; set; }
    [JsonProperty("COLUMNS_NAMED")]
    public string ColumnsNamed { get; set; }
    [JsonProperty("PARAMETERS")]
    public List<Parameter> Parameters { get; set; }

}