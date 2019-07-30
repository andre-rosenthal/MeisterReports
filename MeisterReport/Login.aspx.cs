using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    private string userId { get; set; }
    private string psw { get; set; }
    private string sapClient { get; set; }
    private Uri gatewayUrl { get; set; }
    private bool isAuthenticated { get; set; }
    public bool runningOD4 { get; private set; }
    private const string authenticated = "authenticated";
    private const string sap_client = "sap-client";
    private const string gtw_url = "gateway_url";
    private const string userName = "UserName";
    private const string password = "sap_psw";
    private const string model = "model";
    private const string od4 = "od4";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            SetLoginField(sap_client, ref this.SAPClient);
            SetLoginField(gtw_url, ref this.SAPGateway, true);
            SetLoginField(userName, ref this.UserName);
            SetLoginField(password, ref this.Password);
            string od = Application[od4] as string;
            bool bod4 = false;
            bool.TryParse(od, out bod4);
            this.OD4Mode.Checked = bod4;
            var v = Session[authenticated] as string;
            bool b = false;
            if (bool.TryParse(v, out b))
            {             
                this.Response.Redirect("~/main.aspx");
            }
        }
    }

    private bool TestApplicationVariable(string v, string t, bool isUrl)
    {
        if (isUrl)
            return Uri.IsWellFormedUriString(v, UriKind.RelativeOrAbsolute);
        else
            return (!string.IsNullOrEmpty(v) && string.IsNullOrEmpty(t));
    }

    private void SetLoginField(string a, ref TextBox tb, bool isUrl = false)
    {
        string v = Application[a] as string;
        if (TestApplicationVariable(v, tb.Text, isUrl))
            tb.Text = v;
    }

    private dynamic GetApplicationVariable(string v, bool IsUrl = false, bool IsBool = false)
    {
        dynamic d = Application[v] as dynamic;
        if (IsUrl)
        {
            if (d == null)
                return string.Empty;
            Uri u = new Uri(d as String);
            return u.AbsoluteUri;
        }
        else if (IsBool)
        {
            bool b = false;
            if (d != null)
                b = d;
            return b.ToString();
        }
        else
            return d;
    }
    protected void UserName_TextChanged(object sender, EventArgs e)
    {
        SetAppField(sender, userName);
    }
    protected void Password_TextChanged(object sender, EventArgs e)
    {
        SetAppField(sender, password);
    }

    protected void SAPClient_TextChanged(object sender, EventArgs e)
    {
        TextBox tb = sender as TextBox;
        short i = 0;
        if (!string.IsNullOrEmpty(tb.Text))
            if (Int16.TryParse(tb.Text, out i))
                if (i > 0 && i != 66)
                    Application[sap_client] = tb.Text;
    }

    protected void SAPGateway_TextChanged(object sender, EventArgs e)
    {
        SetAppField(sender, gtw_url);
    }

    protected void OD4Mode_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = sender as CheckBox;
        if (cb.Checked)
            Application[od4] = true;
        else
            Application[od4] = false;
    }

    private void SetAppField(object o, string a)
    {
        TextBox tb = o as TextBox;
        if (!string.IsNullOrEmpty(tb.Text))
            Session[a] = tb.Text;
    }
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        Model m = new Model();
        string us = RetrieveContent(this.UserName.Text, userName);
        string pw = RetrieveContent(this.Password.Text, password);
        string cl = RetrieveContent(this.SAPClient.Text, sap_client);
        string od = RetrieveContent(this.OD4Mode.Checked.ToString(), od4,false,true);
        bool bod4 = false;
        bool.TryParse(od, out bod4);
        Uri u = new Uri(RetrieveContent(this.SAPGateway.Text, gtw_url,true));
        if (!(string.IsNullOrEmpty(us) && string.IsNullOrEmpty(pw) && string.IsNullOrEmpty(cl) && string.IsNullOrEmpty(u.AbsoluteUri)))
            if (m.Controller.Authenticate(us, pw, u, cl, bod4))
            {
                this.isAuthenticated = true;
                Session[model] = m;
                Session[userName] = us;
                Session[authenticated] = true;
                Session[od4] = bod4;
                Application[userName] = us;
                Application[password] = pw;
                Application[sap_client] = cl;
                Application[gtw_url] = u.AbsoluteUri;
                Application[od4] = bod4;
                this.Response.Redirect("~/main.aspx");
            }
    }

    private string RetrieveContent(string t, string a, bool IsUrl = false, bool IsBool = false)
    {
        if (IsUrl && !string.IsNullOrEmpty(t))
        {
            Application[a] = t;
            return t;
        }
        else
        {
            string s = GetApplicationVariable(a, IsUrl, IsBool);
            if (!string.IsNullOrEmpty(s))
                return s;
            Application[a] = t;
            return t;
        }
    }
}
        
