using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

/// <summary>
/// Summary description for Model
/// </summary>
public class Model
{
    public enum Calls
    {
        unkown,
        GetTcodesOrProgramsByHint,
        GetParameters,
        Scheduler,
        Retriever,
        Variants,
        NewVariant
    }
    Dictionary<Calls, string> Dict { get; set; }
    Dictionary<string, string> Payload { get; set; }
    public List<ResultSet> ResultSet { get; set; }
    public List<ParmSet> parmSet { get; set; }
    public List<SchedulerResponse> schedulerResponses { get; set; }
    public List<ReportData> ReportData { get; set; }
    public TimeSpan TimeSpan { get; private set; }
    public string RawReport { get; set; }
    public List<VariantsSet> Variants { get; set; }
    public List<Messages> Messages { get; set; }
    public List<VariantValuesSet> VariantValuesSet { get; set; }

    private Controller Controller;

    public Model()
    {
        this.Controller = new Controller();
        Dict = new Dictionary<Calls, string>();
        Dict.Add(Calls.unkown, @"");
        Dict.Add(Calls.GetTcodesOrProgramsByHint, @"Meister.SDK.Report.Finder");
        Dict.Add(Calls.GetParameters, @"Meister.SDK.Report.Parameters");
        Dict.Add(Calls.Scheduler, @"Meister.SDK.Report.Scheduler");
        Dict.Add(Calls.Retriever, @"Meister.SDK.Report.Retrieval");
        Dict.Add(Calls.Variants, @"Meister.SDK.Report.Read.Variant");
        Dict.Add(Calls.NewVariant, @"Meister.SDK.Report.Create.Variant");
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

    public void Parameters(string rep)
    {
        Dictionary<string, string> jsoncall = new Dictionary<string, string>();
        jsoncall.Add("REP_TCODE", rep);
        jsoncall.Add("rep_tcode_type", "T");
        string s = Controller.FromList<Dictionary<string, string>>(jsoncall);
        EndPoint endPoint = GetEndPoint(Calls.GetParameters);
        var sw = Stopwatch.StartNew();
        parmSet = Controller.ToList<ParmSet>(Controller.ExecuteCall(endPoint, s, false));
        sw.Stop();
        TimeSpan = sw.Elapsed;
    }

    public void ScheduleReport(Scheduler s)
    {
        string st = Serialize.ToJson<Scheduler>(s);
        EndPoint endPoint = GetEndPoint(Calls.Scheduler);
        var sw = Stopwatch.StartNew();
        schedulerResponses = Controller.ToList<SchedulerResponse>(Controller.ExecuteCall(endPoint, st, false));
        sw.Stop();
        TimeSpan = sw.Elapsed;
    }

    public void ReportRetriever(string s, bool hit = false, bool metadata = false)
    {
        if (hit == true)
            ReadReport(s, metadata);
        else
            MyReports(s);
    }

    private void ReadReport(string guid, bool metadata)
    {
        SchedulerSet sc = new SchedulerSet();
        sc.ReportGuid = guid;
        string st = Serialize.ToJson<SchedulerSet>(sc);
        EndPoint endPoint = GetEndPoint(Calls.Retriever);
        endPoint.Parm.Compression = "";
        var sw = Stopwatch.StartNew();
        RawReport = Controller.ToString(Controller.ExecuteCall(endPoint, st, false));
        sw.Stop();
        TimeSpan = sw.Elapsed;
    }

    public void MyReports(string user)
    {
        SchedulerSet sc = new SchedulerSet();
        sc.UserId = user;
        string st = Serialize.ToJson<SchedulerSet>(sc);
        EndPoint endPoint = GetEndPoint(Calls.Retriever);
        var sw = Stopwatch.StartNew();
        string s = Controller.ToString(Controller.ExecuteCall(endPoint, st, false));
        ReportData = Controller.FromString<ReportData>(s);
        sw.Stop();
        TimeSpan = sw.Elapsed;
    }

    public void VariantContents(string program, string variant)
    {
        VariantQuery vq = new VariantQuery();
        vq.ReportName = program;
        vq.VariantName = variant;
        string st = Serialize.ToJson<VariantQuery>(vq);
        EndPoint endPoint = GetEndPoint(Calls.Variants);
        endPoint.Parm.Compression = "";
        var sw = Stopwatch.StartNew();
        VariantValuesSet = Controller.ToList<VariantValuesSet>(Controller.ExecuteCall(endPoint, st, false));
        sw.Stop();
        TimeSpan = sw.Elapsed;
    }
    public void FindVariants(string program)
    {
        VariantQuery vq = new VariantQuery();
        vq.ReportName = program;
        string st = Serialize.ToJson<VariantQuery>(vq);
        EndPoint endPoint = GetEndPoint(Calls.Variants);
        endPoint.Parm.Compression = "";
        var sw = Stopwatch.StartNew();
        Variants = Controller.ToList<VariantsSet>(Controller.ExecuteCall(endPoint, st, false));
        sw.Stop();
        TimeSpan = sw.Elapsed;
    }

    public void SaveVariant(NewVariant nv)
    {
        string st = Serialize.ToJson<NewVariant>(nv);
        EndPoint endPoint = GetEndPoint(Calls.NewVariant);
        endPoint.Parm.Compression = "";
        var sw = Stopwatch.StartNew();
        Messages = Controller.ToList<Messages>(Controller.ExecuteCall(endPoint, st, false));
        sw.Stop();
        TimeSpan = sw.Elapsed;
    }
}