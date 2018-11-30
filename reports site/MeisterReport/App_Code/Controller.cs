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
using Newtonsoft.Json;

/// <summary>
/// Summary description for Controller
/// </summary>
public class Controller
{
    public Controller()
    {
        endPoint = new EndPoint();
    }
    protected readonly string _endpoint;
    public EndPoint endPoint { get; set; }
    public TimeSpan TimeSpan { get; set; }

    HttpClient Client { get; set; }

    public dynamic ExecuteCall(EndPoint ep, string json, bool wait = true)
    {

        string parms = JsonConvert.SerializeObject(ep.Parm);
        dynamic d = null;
        if (wait)
        {
            Task<dynamic> t = GetAsync<dynamic>(ep.Handler, parms, json);
            t.Wait();
            d = t.Result;
            t.Dispose();
        }
        else
        {
            Task t = new Task(() =>
            {
                d = Get<dynamic>(ep.Handler, parms, json);
            });
            t.RunSynchronously();
        }
        return d;
    }

    public T Get<T>(string ep, string parms, string json)
    {
        T t = default(T);
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = Authenticate();
            string call = CreateCall(ep, parms, json);
            var response = client.GetAsync(call).Result;
            var responseContent = response.Content;
            string result = responseContent.ReadAsStringAsync().Result;
            t = JsonConvert.DeserializeObject<T>(result);
        }
        return t;
    }

    public async Task<T> GetAsync<T>(string ep, string parms, string json)
    {
        T t = default(T);
        using (var client = new HttpClient())
        {
            client.Timeout = TimeSpan.FromMinutes(5);
            client.DefaultRequestHeaders.Authorization = Authenticate();
            string call = CreateCall(ep, parms, json);
            using (HttpResponseMessage response = await client.GetAsync(call))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                t = JsonConvert.DeserializeObject<T>(result);
            }
        }
        return t;
    }

    private string CreateCall(string ep, string parms, string json)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(ConfigurationManager.AppSettings["URL"]);
        sb.Append(@"/sap/opu/odata/MEISTER/ENGINE/Execute?Endpoint=");
        sb.Append(AddSingleQuotes(ep));
        sb.Append(@"&Parms=");
        sb.Append(AddSingleQuotes(parms));
        sb.Append(@"&Json=");
        sb.Append(AddSingleQuotes(json));
        sb.Append(@"&$format=json");
        return sb.ToString();
    }

    public string AddQuotes(string s)
    {
        char q = '"';
        return q.ToString() + s + q.ToString();
    }

    public string AddSingleQuotes(string s)
    {
        string q = "'";
        return q + s + q;
    }


    public List<T> ToList<T>(dynamic d)
    {
        List<T> list = new List<T>();
        if (d == null)
        {
            return list;
        }

        dynamic j = d["d"].results[0];
        string json = j["Json"];
        if (endPoint.Parm.Compression == "O")
        {
            json = Unzip(json);
        }
        list = JsonConvert.DeserializeObject<List<T>>(json);
        return list;
    }

    public string ToString(dynamic d)
    {
        string s = string.Empty;
        try
        {
            if (d == null)
            {
                return s;
            }

            dynamic j = d["d"].results[0];
            if (j != null)
            {
                string json = j["Json"];
                if (endPoint.Parm.Compression == "O")
                {
                    json = Unzip(json);
                }
                s = json;
            }
        }
        catch
        {

        }
        return s;
    }

    public static void CopyTo(Stream src, Stream dest)
    {
        byte[] bytes = new byte[4096];
        int cnt;
        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        {
            dest.Write(bytes, 0, cnt);
        }
    }

    public static string ByteArrayToString(byte[] ba)
    {
        return Encoding.ASCII.GetString(ba);
    }

    public static byte[] StringToByteArray(string st)
    {
        string h = st.Replace(System.Environment.NewLine, String.Empty);
        int NumberChars = h.Length / 2;
        byte[] bytes = new byte[NumberChars];
        using (var sr = new StringReader(h))
        {
            for (int i = 0; i < NumberChars; i++)
                bytes[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
        }
        return bytes;
    }

    [SuppressUnmanagedCodeSecurity, SecurityCritical]
    public static string Zip(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                CopyTo(msi, gs);
            }
            return ByteArrayToString(mso.ToArray());
        }
    }

    public string FromList<T>(List<T> l)
    {
        return JsonConvert.SerializeObject(l);
    }

    public string FromList<T>(Dictionary<string, string> d)
    {
        return JsonConvert.SerializeObject(d);
    }
    public AuthenticationHeaderValue Authenticate()
    {
        string uid = string.Empty;
        string psw = string.Empty;
        uid = ConfigurationManager.AppSettings["UserId"];
        psw = ConfigurationManager.AppSettings["Password"];
        var uap = $"{uid}:{psw}";
        var byteArray = Encoding.ASCII.GetBytes(uap);
        return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }

    [SuppressUnmanagedCodeSecurity, SecurityCritical]
    public static string Unzip(string st)
    {
        var bytes = StringToByteArray(st);
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                CopyTo(gs, mso);
            }
            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }
}