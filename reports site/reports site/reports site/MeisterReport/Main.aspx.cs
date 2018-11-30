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

public partial class Main : System.Web.UI.Page
{
    public Model model { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        model = new Model();
        if (!this.IsPostBack)
        {
            
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (TextBox1.Text != string.Empty)
        {
            model.ReportFinder(TextBox1.Text);
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
            GridView1.DataSource = null;
            GridView1.DataSource = enums;
            GridView1.DataBind();
            
        }
    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            if (row.RowIndex == GridView1.SelectedIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                row.ToolTip = "Program Selected";
            }
            else
            {
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                row.ToolTip = "Click to select this row.";
            }
        }
    }
}