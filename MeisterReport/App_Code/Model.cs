using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using MeisterCore;
using Newtonsoft.Json;
using static MeisterCore.Support.MeisterSupport;

/// <summary>
/// Summary description for Model
/// </summary>
public class Model
{
    public enum Calls
    {
        unknown,
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
    public ReportData ReportData { get; set; }
    public TimeSpan TimeSpan { get; private set; }
    public string RawReport { get; set; }
    public List<VariantsSet> Variants { get; set; }
    public List<Messages> Messages { get; set; }
    public List<VariantValuesSet> VariantValuesSet { get; set; }
    public Uri od2 = new Uri("Http://MeisterAzure.Dyndns.org:8001");


    private Controller Controller;

    public Model()
    {
        this.Controller = new Controller();
        Dict = new Dictionary<Calls, string>();
        Dict.Add(Calls.unknown, @"");
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
        var byteArray = Encoding.ASCII.GetBytes("DEMOUSER:DemoUser.");
        SchedulerSet sc = new SchedulerSet();
        sc.UserId = user;
        AuthenticationHeaderValue headerValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        Resource<SchedulerSet, ReportDatum> resource = new Resource<SchedulerSet, ReportDatum>(od2, headerValue,
                                        MeisterExtensions.RemoveNullsAndEmptyArrays,
                                        MeisterOptions.None,
                                        AuthenticationModes.Basic,
                                        RuntimeOptions.ExecuteSync);
        ReportData = new ReportData();
        try
        {
            if (resource.Authenticate())
            {
                var response = resource.Execute("Meister.SDK.Report.Retrieval", sc);
                ReportData.ReportDatum = new List<ReportDatum>();
                ReportData.ReportDatum.AddRange(response);
            }
            else
            {
                var response = resource.Execute("Meister.SDK.Report.Retrieval", sc);
                ReportData.ReportDatum = new List<ReportDatum>();
                ReportData.ReportDatum.AddRange(response);
            }
        }
        catch (Exception ex)
        {
           
        }
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

    public dynamic UserAgenda(string us)
    {
        User u = new User();
        u.USERID = us;
        var byteArray = Encoding.ASCII.GetBytes("DEMOUSER:DemoUser.");
        AuthenticationHeaderValue headerValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        Resource<User, AgendaQuery> resource = new Resource<User, AgendaQuery>(od2, headerValue,
                                        MeisterExtensions.RemoveNullsAndEmptyArrays,
                                        MeisterOptions.None,
                                        AuthenticationModes.Basic,
                                        RuntimeOptions.ExecuteSync);
        dynamic response = null;
        try
        {
            if (resource.Authenticate())
            {
                response = resource.Execute("Meister.SDK.Report.Agenda", u);
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return response;
    }
    public dynamic UserSetAgenda(Agenda a)
    {
        var byteArray = Encoding.ASCII.GetBytes("DEMOUSER:DemoUser.");
        AuthenticationHeaderValue headerValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        Resource<Agenda, AgendaResult> resource = new Resource<Agenda, AgendaResult>(od2, headerValue,
                                        MeisterExtensions.RemoveNullsAndEmptyArrays,
                                        MeisterOptions.None,
                                        AuthenticationModes.Basic,
                                        RuntimeOptions.ExecuteSync);
        dynamic response = null;
        try
        {
            if (resource.Authenticate())
            {
                response = resource.Execute("Meister.SDK.Report.Agenda.Add", a);
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return response;
    }
}