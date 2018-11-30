using System.Collections.Generic;
using Newtonsoft.Json;

[JsonObject("EDMSET")]
public class EDMSet
{
    [JsonProperty("EDM")]
    public IList<EDM> EDM { get; set; }
}

[JsonObject("EDM")]
public class EDM
{

    [JsonProperty("ROW_POS")]
    public string ROW_POS { get; set; }

    [JsonProperty("COL_POS")]
    public string COL_POS { get; set; }

    [JsonProperty("FIELDNAME")]
    public string FIELDNAME { get; set; }

    [JsonProperty("TABNAME")]
    public string TABNAME { get; set; }

    [JsonProperty("CURRENCY")]
    public string CURRENCY { get; set; }

    [JsonProperty("CFIELDNAME")]
    public string CFIELDNAME { get; set; }

    [JsonProperty("QUANTITY")]
    public string QUANTITY { get; set; }

    [JsonProperty("QFIELDNAME")]
    public string QFIELDNAME { get; set; }

    [JsonProperty("IFIELDNAME")]
    public string IFIELDNAME { get; set; }

    [JsonProperty("ROUND")]
    public string ROUND { get; set; }

    [JsonProperty("EXPONENT")]
    public string EXPONENT { get; set; }

    [JsonProperty("KEY")]
    public string KEY { get; set; }

    [JsonProperty("KEY_SEL")]
    public string KEY_SEL { get; set; }

    [JsonProperty("ICON")]
    public string ICON { get; set; }

    [JsonProperty("SYMBOL")]
    public string SYMBOL { get; set; }

    [JsonProperty("CHECKBOX")]
    public string CHECKBOX { get; set; }

    [JsonProperty("JUST")]
    public string JUST { get; set; }

    [JsonProperty("LZERO")]
    public string LZERO { get; set; }

    [JsonProperty("NO_SIGN")]
    public string NO_SIGN { get; set; }

    [JsonProperty("NO_ZERO")]
    public string NO_ZERO { get; set; }

    [JsonProperty("NO_CONVEXT")]
    public string NO_CONVEXT { get; set; }

    [JsonProperty("EDIT_MASK")]
    public string EDIT_MASK { get; set; }

    [JsonProperty("EMPHASIZE")]
    public string EMPHASIZE { get; set; }

    [JsonProperty("FIX_COLUMN")]
    public string FIX_COLUMN { get; set; }

    [JsonProperty("DO_SUM")]
    public string DO_SUM { get; set; }

    [JsonProperty("NO_SUM")]
    public string NO_SUM { get; set; }

    [JsonProperty("NO_OUT")]
    public string NO_OUT { get; set; }

    [JsonProperty("TECH")]
    public string TECH { get; set; }

    [JsonProperty("OUTPUTLEN")]
    public string OUTPUTLEN { get; set; }

    [JsonProperty("CONVEXIT")]
    public string CONVEXIT { get; set; }

    [JsonProperty("SELTEXT")]
    public string SELTEXT { get; set; }

    [JsonProperty("TOOLTIP")]
    public string TOOLTIP { get; set; }

    [JsonProperty("ROLLNAME")]
    public string ROLLNAME { get; set; }

    [JsonProperty("DATATYPE")]
    public string DATATYPE { get; set; }

    [JsonProperty("INTTYPE")]
    public string INTTYPE { get; set; }

    [JsonProperty("INTLEN")]
    public string INTLEN { get; set; }

    [JsonProperty("LOWERCASE")]
    public string LOWERCASE { get; set; }

    [JsonProperty("REPTEXT")]
    public string REPTEXT { get; set; }

    [JsonProperty("HIER_LEVEL")]
    public string HIER_LEVEL { get; set; }

    [JsonProperty("REPREP")]
    public string REPREP { get; set; }

    [JsonProperty("DOMNAME")]
    public string DOMNAME { get; set; }

    [JsonProperty("SP_GROUP")]
    public string SP_GROUP { get; set; }

    [JsonProperty("HOTSPOT")]
    public string HOTSPOT { get; set; }

    [JsonProperty("DFIELDNAME")]
    public string DFIELDNAME { get; set; }

    [JsonProperty("COL_ID")]
    public string COL_ID { get; set; }

    [JsonProperty("F4AVAILABL")]
    public string F4AVAILABL { get; set; }

    [JsonProperty("AUTO_VALUE")]
    public string AUTO_VALUE { get; set; }

    [JsonProperty("CHECKTABLE")]
    public string CHECKTABLE { get; set; }

    [JsonProperty("VALEXI")]
    public string VALEXI { get; set; }

    [JsonProperty("WEB_FIELD")]
    public string WEB_FIELD { get; set; }

    [JsonProperty("HREF_HNDL")]
    public string HREF_HNDL { get; set; }

    [JsonProperty("STYLE")]
    public string STYLE { get; set; }

    [JsonProperty("STYLE2")]
    public string STYLE2 { get; set; }

    [JsonProperty("STYLE3")]
    public string STYLE3 { get; set; }

    [JsonProperty("STYLE4")]
    public string STYLE4 { get; set; }

    [JsonProperty("DRDN_HNDL")]
    public string DRDN_HNDL { get; set; }

    [JsonProperty("DRDN_FIELD")]
    public string DRDN_FIELD { get; set; }

    [JsonProperty("NO_MERGING")]
    public string NO_MERGING { get; set; }

    [JsonProperty("H_FTYPE")]
    public string H_FTYPE { get; set; }

    [JsonProperty("COL_OPT")]
    public string COL_OPT { get; set; }

    [JsonProperty("NO_INIT_CH")]
    public string NO_INIT_CH { get; set; }

    [JsonProperty("DRDN_ALIAS")]
    public string DRDN_ALIAS { get; set; }

    [JsonProperty("DECFLOAT_STYLE")]
    public string DECFLOAT_STYLE { get; set; }

    [JsonProperty("PARAMETER")]
    public string PARAMETER { get; set; }

    [JsonProperty("PARAMETER1")]
    public string PARAMETER1 { get; set; }

    [JsonProperty("PARAMETER2")]
    public string PARAMETER2 { get; set; }

    [JsonProperty("PARAMETER3")]
    public string PARAMETER3 { get; set; }

    [JsonProperty("PARAMETER4")]
    public string PARAMETER4 { get; set; }

    [JsonProperty("PARAMETER5")]
    public string PARAMETER5 { get; set; }

    [JsonProperty("PARAMETER6")]
    public string PARAMETER6 { get; set; }

    [JsonProperty("PARAMETER7")]
    public string PARAMETER7 { get; set; }

    [JsonProperty("PARAMETER8")]
    public string PARAMETER8 { get; set; }

    [JsonProperty("PARAMETER9")]
    public string PARAMETER9 { get; set; }

    [JsonProperty("REF_FIELD")]
    public string REF_FIELD { get; set; }

    [JsonProperty("REF_TABLE")]
    public string REF_TABLE { get; set; }

    [JsonProperty("TXT_FIELD")]
    public string TXT_FIELD { get; set; }

    [JsonProperty("ROUNDFIELD")]
    public string ROUNDFIELD { get; set; }

    [JsonProperty("DECIMALS_O")]
    public string DECIMALS_O { get; set; }

    [JsonProperty("DECMLFIELD")]
    public string DECMLFIELD { get; set; }

    [JsonProperty("DD_OUTLEN")]
    public string DD_OUTLEN { get; set; }

    [JsonProperty("DECIMALS")]
    public string DECIMALS { get; set; }

    [JsonProperty("COLTEXT")]
    public string COLTEXT { get; set; }

    [JsonProperty("SCRTEXT_L")]
    public string SCRTEXT_L { get; set; }

    [JsonProperty("SCRTEXT_M")]
    public string SCRTEXT_M { get; set; }

    [JsonProperty("SCRTEXT_S")]
    public string SCRTEXT_S { get; set; }

    [JsonProperty("COLDDICTXT")]
    public string COLDDICTXT { get; set; }

    [JsonProperty("SELDDICTXT")]
    public string SELDDICTXT { get; set; }

    [JsonProperty("TIPDDICTXT")]
    public string TIPDDICTXT { get; set; }

    [JsonProperty("EDIT")]
    public string EDIT { get; set; }

    [JsonProperty("TECH_COL")]
    public string TECH_COL { get; set; }

    [JsonProperty("TECH_FORM")]
    public string TECH_FORM { get; set; }

    [JsonProperty("TECH_COMP")]
    public string TECH_COMP { get; set; }

    [JsonProperty("HIER_CPOS")]
    public string HIER_CPOS { get; set; }

    [JsonProperty("H_COL_KEY")]
    public string H_COL_KEY { get; set; }

    [JsonProperty("H_SELECT")]
    public string H_SELECT { get; set; }

    [JsonProperty("DD_ROLL")]
    public string DD_ROLL { get; set; }

    [JsonProperty("DRAGDROPID")]
    public string DRAGDROPID { get; set; }

    [JsonProperty("MAC")]
    public string MAC { get; set; }

    [JsonProperty("INDX_FIELD")]
    public string INDX_FIELD { get; set; }

    [JsonProperty("INDX_CFIEL")]
    public string INDX_CFIEL { get; set; }

    [JsonProperty("INDX_QFIEL")]
    public string INDX_QFIEL { get; set; }

    [JsonProperty("INDX_IFIEL")]
    public string INDX_IFIEL { get; set; }

    [JsonProperty("INDX_ROUND")]
    public string INDX_ROUND { get; set; }

    [JsonProperty("INDX_DECML")]
    public string INDX_DECML { get; set; }

    [JsonProperty("GET_STYLE")]
    public string GET_STYLE { get; set; }

    [JsonProperty("MARK")]
    public string MARK { get; set; }
}

