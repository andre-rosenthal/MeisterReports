    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

public class VarientBind
{
    public string VariantName { get; set; }
    public string Description { get; set; }
}

[JsonObject("VARIANTNEWSET")]
public partial class NewVariant
{
    [JsonProperty("DESCRIPTION")]
    public string Description { get; set; }

    [JsonProperty("PARAMETERS")]
    public List<VariantContent> Parameters { get; set; }

    [JsonProperty("REPORT")]
    public string Report { get; set; }

    [JsonProperty("VARIANT_NAME")]
    public string VariantName { get; set; }

    [JsonProperty("SAVE")]
    public string Save { get; set; }
}

[JsonObject("VARIANQUERY")]
public partial class VariantQuery
{
    [JsonProperty("REPORT_NAME")]
    public string ReportName { get; set; }

    [JsonProperty("VARIANT_NAME")]
    public string VariantName { get; set; }
}

[JsonObject("VARIANTSSET")]
    public partial class VariantsSet
{
    [JsonProperty("REPORT")]
    public string Report { get; set; }

    [JsonProperty("VARIANTS")]
    public List<Variant> VariantSet { get; set; }
}

[JsonObject("VARIANTS")]
public partial class Variant
{
    [JsonProperty("VARIANT")]
    public string name { get; set; }

    [JsonProperty("DESCRIPTION")]
    public string Description { get; set; }

    [JsonProperty("PROTECTED")]
    public string Protected { get; set; }
}

[JsonObject("VARIANTVALUESSET")]
public partial class VariantValuesSet
{
    [JsonProperty("REPORT")]
    public string Report { get; set; }

    [JsonProperty("VARIANTS")]
    public List<VariantElement> Variants { get; set; }
}

public partial class VariantElement
{
    [JsonProperty("VARIANT")]
    public VariantContent Variant { get; set; }
}

[JsonObject("VARIANT")]
public partial class VariantContent
{
    [JsonProperty("SELNAME")]
    public string Selname { get; set; }

    [JsonProperty("KIND")]
    public string Kind { get; set; }

    [JsonProperty("SIGN")]
    public string Sign { get; set; }

    [JsonProperty("OPTION")]
    public string Option { get; set; }

    [JsonProperty("LOW")]
    public string Low { get; set; }

    [JsonProperty("HIGH")]
    public string High { get; set; }
}
