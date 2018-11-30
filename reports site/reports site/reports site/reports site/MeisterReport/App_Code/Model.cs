using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Model
/// </summary>
public class Model
{
    public enum Calls
    {
        unkown,
        GetTcodesOrProgramsByHint,
    }
    Dictionary<Calls, string> Dict { get; set; }
    Dictionary<string, string> Payload { get; set; }
    public List<ResultSet> ResultSet { get; set; }
    public TimeSpan TimeSpan { get; private set; }

    private Controller Controller;

    public Model()
    {
        this.Controller = new Controller();
        Dict = new Dictionary<Calls, string>();
        Dict.Add(Calls.unkown, @"");
        Dict.Add(Calls.GetTcodesOrProgramsByHint, @"Meister.SDK.Report.Finder");
        Payload = new Dictionary<string, string>();
    }

    
        
    
    public EndPoint GetEndPoint(Calls key)
    {
        EndPoint ep = new EndPoint();
        string h = string.Empty;
        Dict.TryGetValue(key, out h);
        ep.Handler = h;
        ep.Parm = new Parm();
        return ep;
    }

    public void ReportFinder(string hint)
    {
        Dictionary<string, string> jsoncall = new Dictionary<string, string>();
        jsoncall.Add("HINT", hint);
        string s = Controller.FromList<Dictionary<string, string>>(jsoncall);
        EndPoint endPoint = GetEndPoint(Calls.GetTcodesOrProgramsByHint);
        var sw = Stopwatch.StartNew();
        ResultSet = Controller.ToList<ResultSet>(Controller.ExecuteCall(endPoint, s, false));
        sw.Stop();
        TimeSpan = sw.Elapsed;
    }

    
}