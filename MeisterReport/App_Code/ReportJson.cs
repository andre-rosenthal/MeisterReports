using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ReportJson
/// </summary>
public class ReportsJson
{
   public List<string> report { get; set; }
}

public class ReportJsonEDM
{
    public string EDM { get; set; }
    public string CONTENT { get; set; }
}

public class ReportJsonNamed
{
    public string CONTENT { get; set; }
}