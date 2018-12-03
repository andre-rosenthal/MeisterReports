using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MeisterCore;
using System.Net;
using System.Linq.Expressions;
using System.Web.Services;
using System.Threading;

public partial class Main : System.Web.UI.Page
{
    public List<string> AvailableReports { get; set; }
    public List<string> AvailableReportsDescr { get; set; }
    public List<string> ValidOptionsNH { get; set; }
    public List<string> ValidOptionsH { get; set; }
    public List<string> ParmChanges { get; set; }
    private bool DemoMode = false;
    public Model model { get; set; }
    public Visibilities MyVisibilities { get; set; }
    public bool ReportsShown { get; set; }
    public bool showAgenda { get; set; }
    public bool LookupShown { get; set; }
    public bool HasParmChose { get; set; }
    public const string Show = "Show My Reports";
    public const string Hide = "Hide My Reports";
    public string EDM { get; private set; }
    public string Content { get; private set; }
    private const string nl = @"\r\n";
    private const string SesShowRep = "ShownReports";
    private const string SesParmList = "ParmsList";
    private const string SesHasParm = "HasParmChosen";
    private const string SesMatches = "Matches";
    private const string SesReports = "Reports";
    private const string SesVariants = "Variants";
    private const string ValidH = "ValidH";
    private const string ValidNH = "ValidNH";
    private const string ParmSet = "ParmSet";
    private const string VarParCnt = "VarParContent";
    private const string ParmsAltered = "ParmsAltered";
    private const string OriginalParms = "OriginalParms";
    private const string ReportName = "ReportName";
    private const string ShowAgenda = "ShowAgenda";
    private const string SaveAgenda = "SaveAgenda";
    private const string SelectedAgenda = "SelectedAgenda";
    private const string SavedNick = "SavedNick";
    private const string VarNameSaved = "VarNameSaved";
    private const string SavedAgendaForUpdate = "SavedAgendaForUpdate";
    private const string Password = "Password";
    private const string UserName = "UserName";
    private const string IsDemo = "IsDemo";
    private const string ShowA = "Show Scheduler";
    private const string HideA = "Hide Scheduler";
    public List<BindReportsByUser> reps = null;
    private List<ParmBind> parmBind = null;
    public List<VarientBind> variantBodies { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        model = new Model();
        SetMessage(String.Empty);
        SetInitial();
        AvailableReports = new List<string>();
        AvailableReportsDescr = new List<string>();
        AddRepors(AvailableReports);
        AddReporDescriptions(AvailableReportsDescr);
        ValidOptionsNH = AddOptions(false);
        ValidOptionsH = AddOptions(true);
        Session[ValidH] = ValidOptionsH;
        Session[ValidNH] = ValidOptionsNH;
        Controller c = new Controller();
        // build {"EDM":'
        EDM = "{" + c.AddQuotes("EDM") + ":";
        Content = "," + c.AddQuotes("CONTENT") + ":";
        if (!this.IsPostBack)
        {
            SetMode("*");
            SetDemo();           
            Session[ParmsAltered] = new List<string>();
            MyVisibilities = new Visibilities();
            MyVisibilities.SetOperations(Visibilities.Operations.gurnist);
        }
        else
        {
            if (Session.Count > 0)
            {
                foreach (var k in Session.Keys)
                {
                    switch (k.ToString())
                    {
                        case SesShowRep:
                            {
                                ReportsShown = (bool)Session[SesShowRep];
                                break;
                            }
                        case SesHasParm:
                            {
                                HasParmChose = (bool)Session[SesHasParm];
                                break;
                            }
                        case ShowAgenda:
                            {
                                showAgenda = (bool)Session[ShowAgenda];
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }
    }

    private void SetDemo()
    {
        if (IsDemoMode())
        {
            DemoMode = true;
            ddpDemo.Visible = true;
            TextBox1.Text = ddpDemo.SelectedValue;
            int i = AvailableReports.IndexOf(ddpDemo.SelectedValue);
            if (i >= 0)
            {
                TextBox8.Text = AvailableReportsDescr[i];
            }
        }
        else
            TextBox1.Visible = true;
    }

    private bool IsDemoMode()
    {
        if (Boolean.TryParse(ConfigurationManager.AppSettings[IsDemo], out DemoMode))
            return DemoMode;
        return false;
    }

    private void SetInitial()
    {
        Session[UserName] = ConfigurationManager.AppSettings[UserName];
        Session[Password] = ConfigurationManager.AppSettings[Password];
    }

    private string GetUserName()
    {
        return Session[UserName] as string;
    }

    private char[] GetPassword()
    {
        return ((string)Session[Password]).ToCharArray();
    }

    private List<string> AddOptions(bool high)
    {
        // if HIGH is false
        // EQ, NE, GT, LE, LT,CP, and NP
        // otherwise BT (BeTween) and NB (Not Between)
        List<string> l = new List<string>();
        if (high)
        {
            l.Add("BT");
            l.Add("NB");
        }
        else
        {
            l.Add("EQ");
            l.Add("NE");
            l.Add("GT");
            l.Add("LT");
            l.Add("CP");
            l.Add("NP");
        }
        return l;
    }

    public List<string> GetOptions(bool high)
    {
        if (high)
            return ValidOptionsH;
        return ValidOptionsNH;
    }

    private void AddRepors(List<string> l)
    {
        l.Add("RM07RESLH");
        l.Add("S_ALR_87012326");
        l.Add("SD_SALES_ORDERS_VIEW");
        l.Add("S_ALR_87012332");
        l.Add("S_ALR_87012291");
    }

    private void AddReporDescriptions(List<string> l)
    {
        l.Add("List Inventory Mgmt - report");
        l.Add("Char of Accounts - TCode");
        l.Add("List of Sales Orders - Report");
        l.Add("Statement Cust/Vendor/GL Acctn - TCode");
        l.Add("Line Item Journal - TCode");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Cleanup();
        Label10.Visible = false;
        if (TextBox1.Text != string.Empty)
        {
            model.ReportFinder(TextBox1.Text.ToUpper());
            IEnumerable<ResultSet> list = model.ResultSet as IEnumerable<ResultSet>;
            List<BindResults> enums = new List<BindResults>();
            foreach (var l in list)
                foreach (var l1 in l.results)
                {
                    var br = new BindResults();
                    br.type = l1.Enum_id;
                    foreach (var l2 in l1.Enums)
                    {
                        br.name = l2.Name;
                        br.value = l2.value;
                        enums.Add(br);
                    }
                }
            BindData<List<BindResults>>(GridView1, enums, SesMatches);
            Grid1.Visible = true;
        }
    }

    private void Cleanup()
    {
        Grid4.Visible = false;
        Grid2.Visible = false;
        VariantSave.Visible = false;
        AfterB2.Visible = false;
    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }

    private string ToCamelCase(string s)
    {
        return char.ToUpper(s.First()) + s.Substring(1).ToLower();
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            if (row.RowIndex == GridView1.SelectedIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                row.ToolTip = "Program Selected";
                UpdateProgress1.Visible = true;
                Session[ReportName] = GridView1.Rows[row.RowIndex].Cells[2].Text;
                SetMessage("Reading Report and Variants ....");
                model.Parameters(Session[ReportName] as string);
                Session[ParmSet] = model.parmSet;
                BuildParameterList(GridView2);
                model.FindVariants(GridView1.Rows[row.RowIndex].Cells[2].Text);
                BuildVariants(GridView4);
                Grid2.Visible = true;
                BeforeB2.Visible = false;
                SearchSAP.Visible = true;
                UpdateProgress1.Visible = false;
                SetMessage("Done reading Report and Variants ....");
                if (IsDemoMode())
                {
                    Label10.Visible = true;
                }
            }
            else
            {
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                row.ToolTip = "Click to select this row.";
            }
        }
    }

    private void BuildVariants(GridView grv)
    {
        IEnumerable<VariantsSet> list = model.Variants as IEnumerable<VariantsSet>;
        List<VarientBind> ls = new List<VarientBind>();
        foreach (var l in list)
            foreach (var v in l.VariantSet)
            {
                VarientBind vb = new VarientBind();
                vb.VariantName = v.name;
                vb.Description = v.Description;
                ls.Add(vb);
            }
        BindData<List<VarientBind>>(grv, ls, SesVariants);
        Grid4.Visible = true;
    }

    private void BuildParameterList(GridView gridView)
    {
        IEnumerable<ParmSet> list = model.parmSet as IEnumerable<ParmSet>;
        parmBind = new List<ParmBind>();
        List<ParmBind> saved = new List<ParmBind>();
        foreach (var l in list)
            foreach (var l1 in l.Parameters.Metadata)
                foreach (var l2 in l1.Parameter)
                {
                    var br = new ParmBind();
                    br.Kind = DisplayKind(l2.Parameter.Kind);
                    br.ParameterName = l2.Parameter.Name;
                    br.Description = l2.Description;
                    br.Operation = "EQ";
                    br.From = "";
                    br.To = "";
                    parmBind.Add(br);
                    var br1 = new ParmBind();
                    br1.Kind = DisplayKind(l2.Parameter.Kind);
                    br1.ParameterName = l2.Parameter.Name;
                    br1.Description = l2.Description;
                    br1.Operation = "EQ";
                    br1.From = "";
                    br1.To = "";
                    saved.Add(br1);
                }
        Session[OriginalParms] = saved;
        BindData<List<ParmBind>>(gridView, parmBind, SesParmList);
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        DoWork();
    }

    public Scheduler DoWork(bool saved = false)
    {
        string rep = GridView1.Rows[GridView1.SelectedIndex].Cells[2].Text;
        if (DemoMode)
            if (!AvailableReports.Contains(rep))
                ShowAlert(DoMessage(rep, AvailableReports));
        AfterB2.Visible = true;
        Scheduler s = new Scheduler();
        if (RadioButtonList3.SelectedValue == "N" )
            s.ColumnsNamed = "X";
        else
            s.WithMetadata = "X";
        foreach (GridViewRow g0 in GridView2.Rows)
            if (IsValid(g0.Cells[4].Text) || IsValid(g0.Cells[5].Text))
            {
                Parameter ps = new Parameter();
                ps.Selname = g0.Cells[1].Text;
                ps.Sign = "I";
                ps.Kind = SAPKind(g0.Cells[2].Text);
                ps.Option = g0.Cells[4].Text;
                ps.Low = g0.Cells[5].Text;
                if (IsValid(g0.Cells[5].Text))
                {
                    if (IsValid(g0.Cells[6].Text))
                    {
                        if (GetOptions(true).Contains(ps.Option))
                            ps.High = g0.Cells[6].Text;
                        else
                        {
                            ShowAlert(DoMessage(ps.Option, ValidOptionsH, true));
                            return null;
                        }
                    }
                    else
                        if (GetOptions(false).Contains(ps.Option))
                        ps.High = ps.Low;
                    else
                    {
                        ShowAlert(DoMessage(ps.Option, ValidOptionsNH, true));
                        return null;
                    }
                    s.Parameters.Add(ps);
                }
            }
        s.Report = GridView1.Rows[GridView1.SelectedIndex].Cells[2].Text;
        s.Username = GetUserName();
        if (!saved)
        {
            model.ScheduleReport(s);
            TextBox2.Text = model.schedulerResponses.FirstOrDefault().Pky;
            TextBox3.Text = model.schedulerResponses.FirstOrDefault().Message;
        }
        return s;
    }

    public static void ShowAlert(string message)
    {
        string cleanMessage = message.Replace("'", "\'");

        Page page = HttpContext.Current.CurrentHandler as Page;
        string script = string.Format("alert('{0}');", cleanMessage);
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
        {
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "alert", script, true /* addScriptTags */);
        }
    }
    private string DoMessage(string msg, List<string> l, bool option = false)
    {
        if (!option)
        {
            string s = "Selected report " + msg + " not availale on this demo" + nl;
            s += "Available Reports are:" + nl;
            foreach (var r in l)
            {
                s += r + nl;
            }
            return s;
        }
        else
        {
            string s = "Selected Option " + msg + " invalid" + nl;
            s += "Available options are:" + nl;
            foreach (var r in l)
            {
                s += r + nl;
            }
            return s;
        }
    }


    private bool IsValid(string s)
    {
        return s != string.Empty && s != "&nbsp;" ? true : false;
    }
    protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView2.EditIndex = e.NewEditIndex;
        parmBind = Session[SesParmList] as List<ParmBind>;
        var v = parmBind.ElementAt(GridView2.EditIndex);
        if (v != null)
        {
            BindData<List<ParmBind>>(GridView2, parmBind, SesParmList);
            Freshup(GridView4);
        }
        VariantSave.Visible = true;
        BeforeB2.Visible = true;
    }

    private void RebindData<T>(GridView gv, string ses)
    {
        T t = (T)Session[ses];
        gv.DataSource = t;
        gv.DataBind();

    }
    private void BindData<T>(GridView gv, T t, string ses)
    {
        gv.DataSource = t;
        gv.DataBind();
        Session[ses] = t;
    }

    protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow gvr = GridView2.Rows[e.RowIndex];
        parmBind = Session[SesParmList] as List<ParmBind>;
        var v = parmBind.ElementAt(GridView2.EditIndex);
        if (v != null)
        {
            if (IsValid(GrabGridUpdate(gvr, 6)))
            {
                if (!GetOptions(true).Contains(GrabGridUpdate(gvr, 4)))
                {
                    ShowAlert(DoMessage(GrabGridUpdate(gvr, 4), ValidOptionsH, true));
                    e.Cancel = true;
                }
            }
            else if (!GetOptions(false).Contains(GrabGridUpdate(gvr, 4)))
            {
                ShowAlert(DoMessage(GrabGridUpdate(gvr, 4), ValidOptionsNH, true));
                e.Cancel = true;
            }
            if (e.Cancel == false)
            {
                ParmChanges = Session[ParmsAltered] as List<string>;
                if (ParmChanges == null)
                    ParmChanges = new List<string>();
                ParmChanges.Add(v.ParameterName);
                v.Operation = GrabGridUpdate(gvr, 4);
                v.From = GrabGridUpdate(gvr, 5);
                v.To = GrabGridUpdate(gvr, 6);
                GridView2.EditIndex = -1;
                BindData<List<ParmBind>>(GridView2, parmBind, SesParmList);
                Session[SesHasParm] = true;
                BeforeB2.Visible = true;
                Session[ParmsAltered] = ParmChanges;
            }
        }
    }

    private string GrabGridUpdate(GridViewRow gvr, int idx)
    {
        string value = string.Empty;
        try
        {
            value = ((TextBox)(TextBox)(gvr.Cells[idx].Controls[0])).Text.ToUpper();
        }
        catch (Exception ex)
        { }
        return value;
    }

    protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView2.SelectedIndex = -1;
        GridView2.EditIndex = -1;
        RebindData<List<ParmBind>>(GridView2, SesParmList);
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        if (ReportsShown == true)
        {
            Button3.Text = Show;
            Session[SesShowRep] = false;
            Button7.Enabled = true;
            Grid3.Visible = false;
        }
        else
        {
            Grid3.Visible = true;
            Button7.Enabled = false;
            model.ReportRetriever(GetUserName());
            IEnumerable<ReportData> list = model.ReportData as IEnumerable<ReportData>;
            reps = new List<BindReportsByUser>();
            if (list != null)
            {
                foreach (var l in list)
                    foreach (var l1 in l.ReportDatum)
                    {
                        var br = new BindReportsByUser();
                        br.UUID = l1.Pky;
                        DateTime dt = FromStringDT(l1.Date, l1.Time);
                        br.Date = String.Format("{0:yyyy/MM/dd}", dt);
                        br.Time = String.Format("{0:HH:mm tt}", dt);
                        br.ReportName = l1.Report.Name;
                        br.Description = l1.Report.Description;
                        switch (l1.Status)
                        {
                            case "S":
                                {
                                    br.Status = "Scheduled";
                                    break;
                                }
                            case "R":
                                {
                                    br.Status = "Running";
                                    break;
                                }
                            case "F":
                                {
                                    br.Status = "Finished";
                                    break;
                                }
                            case "A":
                                {
                                    br.Status = "Aborted";
                                    break;
                                }
                            default:
                                break;
                        }                       
                        reps.Add(br);
                    }
                GridView3.Caption = "Reports found for user";
                BindData<List<BindReportsByUser>>(GridView3, reps, SesReports);
            }
            ReportsShown = true;
            Session[SesShowRep] = true;
            Button3.Text = Hide;
        }
    }

    private DateTime FromStringDT(string date, string time)
    {
        return new DateTime(Int32.Parse(date.Substring(0, 4)), Int32.Parse(date.Substring(4, 2)), Int32.Parse(date.Substring(6, 2)), Int32.Parse(time.Substring(0, 2)), Int32.Parse(time.Substring(2, 2)), Int32.Parse(time.Substring(4, 2)));
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        SetDemo();
        Grid3.Visible = false;
        Button3.Enabled = true;
        Button4.Enabled = true;
        SearchSAP.Visible = true;
        Button7.Text = ShowA;
        Session[ShowAgenda] = false;
        Button3.Text = Show;
    }

    protected void GridView3_PageIndexChanged(object sender, EventArgs e)
    {

    }

    protected void GridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (DoingAgenda())
            NewPage<AgendaBind>(GridView3, e.NewPageIndex, SaveAgenda);
        else
            NewPage<BindReportsByUser>(GridView3, e.NewPageIndex, SesReports);
    }

    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        NewPage<ParmBind>(GridView2, e.NewPageIndex, SesParmList);
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        NewPage<BindResults>(GridView1, e.NewPageIndex, SesMatches);
    }


    private void NewPage<T>(GridView grv, int newPageIndex, string ses)
    {
        grv.PageIndex = newPageIndex;
        List<T> l = Session[ses] as List<T>;
        grv.DataSource = l;
        grv.DataBind();
    }

    protected void GridView3_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DoingAgenda())
        {
            BeforeB2.Visible = true;
            Button2.Visible = false;
            Button9.Visible = true;
            DOWs.Visible = true;
            Button8.Text = "Save Item";
            foreach (GridViewRow row in GridView3.Rows)
            {
                if (row.RowIndex == GridView3.SelectedIndex)
                {
                    string uuid = GridView3.Rows[row.RowIndex].Cells[5].Text;
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    row.ToolTip = "Retrieving Schedule for user";
                    AgendaBind ab = FromAgenda(uuid);
                    if (ab != null)
                    {
                        Button8.Enabled = true;
                        Session[SelectedAgenda] = uuid;
                        RadioButtonList2.Visible = false;
                        TextBox7.Text = ab.TimeSlot;
                        txtNickName.Text = ab.NickName;
                        lbDOW.Text = "Schedule Time Slot";
                        if (ab.WeekDay != string.Empty)
                        {
                            RadioButtonList2.SelectedValue = ab.WeekDay;
                            RadioButtonList2.Visible = true;
                            lbDOW.Text = "Schedule Day of the Week and Time Slot";
                        }
                        if (ab.Schedule_Type != string.Empty)
                            RadioButtonList1.SelectedValue = ab.Schedule_Type.Substring(0, 1);
                        else
                            RadioButtonList1.SelectedValue = "D";
                    }
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "Click to select this row.";
                }
            }
        }
        else
        {
            Button9.Visible = false;
            Dictionary<string, string> Files = new Dictionary<string, string>();
            foreach (GridViewRow row in GridView3.Rows)
            {
                if (row.RowIndex == GridView3.SelectedIndex)
                {
                    string name = GridView3.Rows[row.RowIndex].Cells[4].Text;
                    string zn = name + ".zip";
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    row.ToolTip = "Retrieving Report";
                    Thread.Sleep(100);
                    model.ReportRetriever(GridView3.Rows[row.RowIndex].Cells[3].Text, true);
                    var i = model.RawReport.IndexOf(EDM);
                    if (i != -1)
                    {
                        var j = model.RawReport.IndexOf(Content);
                        string edm = model.RawReport.Substring(i, j - 1) + "]}";
                        string cnt = "{" + model.RawReport.Substring(j + 1);
                        WriteTempFile(name + "_EDM.json", edm, Files);
                        WriteTempFile(name + "_Content.json", cnt, Files);
                        DownloadZipFiles(zn, Files);
                    }
                    else
                    {
                        WriteTempFile(name + "_Content.json", model.RawReport, Files);
                        DownloadZipFiles(zn, Files);
                    }
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "Click to select this row.";
                }
            }
        }
    }

    private AgendaBind FromAgenda(string uuid)
    {
        List<AgendaBind> abl = Session[SaveAgenda] as List<AgendaBind>;
        if (abl != null)
            return (from ab1 in abl where (ab1.UUID == uuid) select ab1).FirstOrDefault();
        return null;
    }

    public void DownloadZipFiles(string fileName, Dictionary<string, string> files)
    {
        string zipname = Path.Combine(Path.GetTempPath(), fileName);
        using (Ionic.Zip.ZipFile zipFile = new Ionic.Zip.ZipFile())
        {
            foreach (var file in files)
            {
                zipFile.AddFile(file.Value, "");
            }
            zipFile.Save(zipname);
        }
        Response.Clear();
        Response.ContentType = "application/zip";
        Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        Response.WriteFile(zipname);
        Response.End();
    }

    public void WriteTempFile(string filename, string text, Dictionary<string, string> files)
    {
        string filePath = Path.Combine(Path.GetTempPath(), filename);
        string[] sa = { text };
        System.IO.File.WriteAllLines(filePath, sa);
        files.Add(filename, filePath);
    }

    protected void GridView4_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GridView4.Rows)
        {
            if (row.RowIndex == GridView4.SelectedIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                row.ToolTip = "Variant Selected";
                model.VariantContents(GridView1.Rows[GridView1.SelectedIndex].Cells[2].Text, GridView4.Rows[row.RowIndex].Cells[1].Text);
                FilldParameterList(GridView2);
                Session[VarNameSaved] = GridView4.Rows[row.RowIndex].Cells[1].Text;
                Session[VarParCnt] = model.VariantValuesSet;
                VariantSave.Visible = false;
                BeforeB2.Visible = true;
            }
            else
            {
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                row.ToolTip = "Click to select this row.";
            }
        }
    }

    private void FilldParameterList(GridView grv)
    {
        List<ParmBind> binds = Session[OriginalParms] as List<ParmBind>;
        BindData(GridView2, binds, OriginalParms);
        IEnumerable<VariantValuesSet> list = model.VariantValuesSet as IEnumerable<VariantValuesSet>;
        foreach (var item in binds)
        {
            item.Operation = "EQ";
            item.From = string.Empty;
            item.To = string.Empty;
        }
        foreach (var l in list)
            foreach (var l1 in l.Variants)
            {
                foreach (var v0 in binds)
                {
                    if (v0.ParameterName == l1.Variant.Selname)
                    {
                        if (IsKosher(l1.Variant.Option))
                            v0.Operation = l1.Variant.Option;
                        v0.From = CheckContent(l1.Variant.Low);
                        v0.To = CheckContent(l1.Variant.High);
                    }
                }
            }
        BindData<List<ParmBind>>(grv, binds, SesParmList);
    }

    private string CheckContent(string s)
    {
        string temp = s;
        s = s.Replace("0", "");
        s = s.Replace(".", "");
        s = s.Replace(" ", "");
        if (s != string.Empty)
            return temp;
        else
            return string.Empty;
    }

    private bool IsKosher(string option)
    {
        return option != string.Empty;
    }

    protected void GridView2_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        Freshup(GridView4);
    }

    private void Freshup(GridView gv)
    {
        if (gv.SelectedIndex != -1)
        {
            gv.Rows[gv.SelectedIndex].BackColor = ColorTranslator.FromHtml("#FFFFFF");
            gv.SelectedIndex = -1;
        }
    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Enabled = false;
            e.Row.Cells[2].Enabled = false;
        }
    }

    protected void Button6_Click(object sender, EventArgs e)
    {
        //check if exists ...
        List<VarientBind> ls = Session[SesVariants] as List<VarientBind>;
        foreach (var v0 in ls)
        {
            if (v0.VariantName == TextBox4.Text.ToUpper() && CheckBox1.Checked == false)
            {
                ShowAlert("Variant " + v0.VariantName + " Already exists - cannot override");
                return;
            }
        }
        model.VariantValuesSet = Session[VarParCnt] as List<VariantValuesSet>;       
        NewVariant nv = new NewVariant();
        nv.Parameters = new List<VariantContent>();
        ParmChanges = Session[ParmsAltered] as List<string>;
        foreach (GridViewRow g0 in GridView2.Rows)
            if (ParmChanges.Contains(g0.Cells[1].Text))
                if (IsValid(g0.Cells[5].Text) || IsValid(g0.Cells[6].Text))
                {
                    if (model.VariantValuesSet != null)
                        foreach (var v0 in model.VariantValuesSet)
                        {
                            foreach (var v1 in v0.Variants)
                            {
                                if (v1.Variant.Selname == g0.Cells[1].Text && v1.Variant.Sign == "*")
                                {
                                    ShowAlert("Variant " + TextBox4.Text.ToUpper() + " has field " + v1.Variant.Selname + " locked for update");
                                    return;
                                }
                            }
                        }
                    VariantContent vc = new VariantContent();
                    vc.Selname = g0.Cells[1].Text;
                    vc.Sign = "I";
                    vc.Kind = SAPKind(g0.Cells[2].Text);
                    vc.Option = g0.Cells[4].Text;
                    vc.Low = g0.Cells[5].Text;
                    if (IsValid(g0.Cells[6].Text))
                        vc.High = g0.Cells[6].Text;
                    else
                        vc.High = vc.Low;
                    nv.Parameters.Add(vc);
                }
        nv.Report = GridView1.Rows[GridView1.SelectedIndex].Cells[2].Text;
        nv.VariantName = TextBox4.Text.ToUpper();
        nv.Description = TextBox5.Text;
        if (CheckBox1.Checked)
            nv.Save = "X";
        model.SaveVariant(nv);
        SetMessage("Variant Saved ...");
    }

    /// <summary>
    /// returns the sap kind
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private string SAPKind(string text)
    {
        if (text == System.Enum.GetName(typeof(ParmBind.KindTypes), ParmBind.KindTypes.Selection))
            return "S";
        else
            return "P";
    }

    /// <summary>
    /// retuns the display kind
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private string DisplayKind(string text)
    {
        if (text == "S")
            return System.Enum.GetName(typeof(ParmBind.KindTypes), ParmBind.KindTypes.Selection);
        else
            return System.Enum.GetName(typeof(ParmBind.KindTypes), ParmBind.KindTypes.Parameter);
    }

    protected void TextBox4_TextChanged(object sender, EventArgs e)
    {
        Session["VarNameSet"] = true;
        if (TextBox4.Text != string.Empty && TextBox5.Text != string.Empty)
            Button6.Enabled = true;
        else
            Button6.Enabled = false;
    }

    protected void TextBox5_TextChanged(object sender, EventArgs e)
    {
        Session["VarNameDesc"] = true;
        if (TextBox4.Text != string.Empty && TextBox5.Text != string.Empty)
            Button6.Enabled = true;
        else
            Button6.Enabled = false;
    }

    /// <summary>
    /// recurrence
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DoingAgenda())
            Button8.Text = "Save Item";
        else
            Button8.Text = "Create Item";
        string nm = " for " + ToCamelCase(GetUserName()) + " ";
        int rb = RadioButtonList1.SelectedIndex;
        if (rb != 0)
        {
            if (rb == 2)
            {
                DOWs.Visible = true;
                lbDOW.Text = "Schedule Day of the Week and Time Slot";
                RadioButtonList2.Visible = true;
                txtNickName.Text = (Session[ReportName] as string) + nm + RadioButtonList1.Text;
            }
            else
            {
                DOWs.Visible = true;
                lbDOW.Text = "Schedule Time Slot";
                RadioButtonList2.Visible = false;
                txtNickName.Text = (Session[ReportName] as string) + nm + RadioButtonList1.SelectedItem.Text;
            }
            Session[SavedNick] = txtNickName.Text;
        }
    }

    private bool DoingAgenda()
    {
        try
        {
            return (bool)Session[ShowAgenda];
        }
        catch (Exception)
        {
            return false;
        }
        
    }

    protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        int rb = RadioButtonList2.SelectedIndex;
        string nm = " for " + ToCamelCase(GetUserName()) + " ";
        txtNickName.Text = (Session[ReportName] as string) + nm + RadioButtonList1.SelectedItem.Text + " on " + RadioButtonList2.SelectedItem.Text;
        Session[SavedNick] = txtNickName.Text;
    }

    protected void TextBox7_TextChanged(object sender, EventArgs e)
    {
        int slot = 0;
        string sn = Session[SavedNick] as string;
        if (Int32.TryParse(TextBox7.Text, out slot))
            if (slot <= 23 && slot >= 0)
            {
                Button8.Enabled = true;
                txtNickName.Text = sn + " at " + TextBox7.Text + " Hours";
            }
            else
            {
                TextBox7.Text = string.Empty;
                ShowAlert("Hour Slot needs to be between 00 and 23");
            }
        else
        {
            TextBox7.Text = string.Empty;
            ShowAlert("Hour Slot needs to be between 00 and 23");
        }
    }

    protected void Button7_Click(object sender, EventArgs e)
    {
        if (showAgenda == true)
        {
            SetMode("a");
            Button7.Text = ShowA;
            Button8.Enabled = true;
            Button3.Enabled = true;
            Button4.Enabled = true;
        }
        else
        {
            SetMode("A");
            Button3.Enabled = false;
            Button4.Enabled = false;
            Grid3.Visible = true;
            GridView3.Caption = "Schedule for User";
            Resource<User, AgendaQuery> query = new Resource<User, AgendaQuery>(new Uri(ConfigurationManager.AppSettings["URL"]));
            User u = new User();
            u.Userid = GetUserName();
            query.Authenticate(GetUserName(), Encoding.ASCII.GetBytes(GetPassword()));
            List<AgendaQuery> agenda = query.Execute("Meister.SDK.Reports.Agenda", u, false);
            List<AgendaBind> reps = new List<AgendaBind>();
            if (agenda != null)
            {
                foreach (var l in agenda)
                    foreach (var l1 in l.AGENDA)
                    {
                        var ab = new AgendaBind();
                        ab.NickName = l1.NICKNAME;
                        ab.Schedule_Type = GetAgendaType(l1.AGENDA_TYPE);
                        ab.TimeSlot = l1.SLOT;
                        if (ab.TimeSlot.Length == 1)
                            ab.TimeSlot = "0" + ab.TimeSlot;
                        ab.UUID = l1.PKY;
                        ab.WeekDay = GetDOW(l1.DOW);
                        ab.UserName = GetUserName();
                        reps.Add(ab);
                    }
                GridView3.Caption = "Schedule found for user";
                BindData<List<AgendaBind>>(GridView3, reps, SaveAgenda);
            }
            ReportsShown = true;
            Session[ShowAgenda] = true;
            Button7.Text = HideA;
        }
    }

    private void SetMode(string v)
    {
        {
            UpdateProgress1.Visible = false;
            DemoMode = false;
            ddpDemo.Visible = false;
            LookupShown = false;
            showAgenda = false;
            CheckBox2.Visible = false;
            Button9.Visible = false;
            SearchSAP.Visible = false;
            Button8.Enabled = false;
            Session[SesShowRep] = false;
            ReportsShown = false;
            TextBox1.Visible = false;
            Button7.Text = ShowA;
            Session[ShowAgenda] = false;
            Grid3.Visible = false;
            Grid1.Visible = false;
            Grid4.Visible = false;
            Grid2.Visible = false;
            VariantSave.Visible = false;
            AfterB2.Visible = false;
            BeforeB2.Visible = false;
            DOWs.Visible = false;
        }
        switch (v)
        {
            case "a":
                {
                    Button8.Enabled = true;
                    break;
                }
            case "A":
                {
                    Grid3.Visible = true;
                    ReportsShown = true;
                    Session[ShowAgenda] = true;
                    Button7.Text = HideA;
                    break;
                }

            default:
                break;
        }
    }

    private string GetDOW(string dw)
    {
        foreach (ListItem item in RadioButtonList2.Items)
        {
            if (item.Value == dw)
                return item.Text;
        }
        return dw;
    }

    private string GetAgendaType(string at)
    {
        foreach (ListItem item in RadioButtonList1.Items)
        {
            if (item.Value == at)
                return item.Text;
        }
        return at;
    }

    /// <summary>
    /// save agenda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button8_Click(object sender, EventArgs e)
    {
        if (DoingAgenda())
        {
            AgendaBind ab = FromAgenda(Session[SelectedAgenda] as string);
            if (ab != null)
            {
                Agenda a = new Agenda();
                a.AGENDA_TYPE = RadioButtonList1.SelectedValue;
                if (a.AGENDA_TYPE == "W")
                    a.DOW = RadioButtonList2.SelectedValue;
                a.NICKNAME = txtNickName.Text;
                a.SLOT = TextBox7.Text;
                a.Parameters = new List<Parameter>();
                a.PKY = ab.UUID;
                Parameter p = new Parameter();
                a.Parameters.Add(p);
                // call to update the agenda ... not the report within!
                Resource<Agenda, AgendaResult> query = new Resource<Agenda, AgendaResult>(new Uri(ConfigurationManager.AppSettings["URL"]));
                query.Authenticate(GetUserName(), Encoding.ASCII.GetBytes(GetPassword()));
                List<AgendaResult> agenda = query.Execute("Meister.SDK.Report.Agenda.Add", a, false);
                SetMessage("Schedule " + a.NICKNAME + " changed successfully");
            }
        }
        else // new agenda item ..
        {
            Agenda a = new Agenda();
            a.NICKNAME = txtNickName.Text;
            a.AGENDA_TYPE = RadioButtonList1.SelectedValue;
            if (a.AGENDA_TYPE == "W")
                a.DOW = RadioButtonList2.SelectedValue;
            a.SLOT = TextBox7.Text;
            if (RadioButtonList3.SelectedValue == "N")
                a.WithMetadata = "X";
            else
                a.ColumnsNamed = "X";
            Scheduler s = DoWork(true);
            a.Name = TextBox1.Text;
            a.USERID = s.Username;
            if (IsValid(TextBox4.Text))
                a.Variant = TextBox4.Text;
            else
                a.Variant = Session[VarNameSaved] as string;
            a.Parameters = new List<Parameter>();
            foreach (var item in s.Parameters)
                a.Parameters.Add(item);
            Resource<Agenda, AgendaResult> query = new Resource<Agenda, AgendaResult>(new Uri(ConfigurationManager.AppSettings["URL"]));
            query.Authenticate(GetUserName(), Encoding.ASCII.GetBytes(GetPassword()));
            List<AgendaResult> agenda = query.Execute("Meister.SDK.Report.Agenda.Add", a, false);
            SetMessage("Schedule " + a.NICKNAME + " created successfully");
        }
    }

    protected void ddpDemo_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox1.Text = ddpDemo.SelectedValue;
        int i = AvailableReports.IndexOf(ddpDemo.SelectedValue);
        if (i >= 0)
        {
            TextBox8.Text = AvailableReportsDescr[i];
        }
    }

    /// <summary>
    /// delete a report ....
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView3_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected bool DoWarning(string rep)
    {

        string title = "Confirm";
        string text = @"You are about to delete the scheduling for " + rep + " Click Yes(OK) or No(Cancel) buttons";
        MessageBox messageBox = new MessageBox(text, title, MessageBox.MessageBoxIcons.Question, MessageBox.MessageBoxButtons.OKCancel, MessageBox.MessageBoxStyle.StyleB);
        messageBox.SuccessEvent.Add("OkClick");
        messageBox.FailedEvent.Add("CancelClick");
        if (messageBox.Show(this) == string.Empty)
            return true;
        return false;
    }


    [WebMethod]
    public static string OkClick(object sender, EventArgs e)
    {
        return string.Empty;
    }


    [WebMethod]
    public static string CancelClick(object sender, EventArgs e)
    {
        return "X";
    }


    protected void ConfirmDelete_Click(object sender, EventArgs e)
    {
        if (DoingAgenda())
        {
            string uuid = string.Empty;
            foreach (GridViewRow row in GridView3.Rows)
                if (row.RowIndex == GridView3.SelectedIndex)
                    uuid = GridView3.Rows[row.RowIndex].Cells[5].Text;
            if (uuid != string.Empty)
            {
                AgendaBind ab = FromAgenda(uuid);
                if (ab != null)
                {
                    Agenda a = new Agenda();
                    a.PKY = ab.UUID;
                    a.DELETE = "X";
                    a.Parameters = new List<Parameter>();
                    Parameter p = new Parameter();
                    a.Parameters.Add(p);
                    Session[SavedAgendaForUpdate] = a;
                    CheckBox2.Visible = true;
                }
            }
        }
    }

    protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBox2.Checked == true)
        {
            Agenda a = Session[SavedAgendaForUpdate] as Agenda;
            if (a != null)
            {
                CheckBox2.Visible = false;
                CheckBox2.Checked = false;
                Button9.Visible = false;
                Resource<Agenda, AgendaResult> query = new Resource<Agenda, AgendaResult>(new Uri(ConfigurationManager.AppSettings["URL"]));
                query.Authenticate(GetUserName(), Encoding.ASCII.GetBytes(GetPassword()));
                List<AgendaResult> agenda = query.Execute("Meister.SDK.Report.Agenda.Add", a, false);
                SetMessage("Schedule deleted");
                Grid3.Visible = false;
                BeforeB2.Visible = false;
                DOWs.Visible = false;
                showAgenda = false;
                Thread.Sleep(100);
                Button7_Click(Button7, null);
            }
        }
    }

    private void SetMessage(string v)
    {
        TextBox3.Text = v;
    }

    protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!DoingAgenda())
            foreach (GridViewRow row in GridView3.Rows)
            {
                TableCell selectCell = row.Cells[0];
                if (selectCell.Controls.Count > 0)
                {
                    LinkButton selectControl = selectCell.Controls[0] as LinkButton;
                    if (selectControl != null)
                    {
                        selectControl.Text = "Download";
                    }
                }
            }
    }
}