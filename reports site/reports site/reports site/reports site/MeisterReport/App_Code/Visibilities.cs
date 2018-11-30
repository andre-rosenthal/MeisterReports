using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Visibilities
/// </summary>
/// 
public class Visibilities
{
    public enum Operations
    {
        gurnist,
        Starting,
        RetrieveReports,
        ScheduleReports,
        ShowHits,
        ShowParameters,
        ShowScheduledItems
    }

    public enum UIElements
    {
        ShowGetMyReports,
        ShowHintChoices,
        ShowHintResults,
        ShowParms,
        ShowScheduler
    }

    public Operations MyOperations { get; set; }

    public List<UIElements> MyUIs { get; set; }

    public Visibilities()
    {
        MyOperations = Operations.gurnist;
        MyUIs = new List<UIElements>();
    }

    public void SetOperations(Operations op)
    {
        MyUIs.Clear();
        switch (op)
        {
            case Operations.gurnist:
                break;
            case Operations.Starting:
                UIElements u = new UIElements();
                u = UIElements.ShowGetMyReports;
                MyUIs.Add(u);
                break;
            case Operations.RetrieveReports:
                break;
            case Operations.ScheduleReports:
                break;
            case Operations.ShowHits:
                break;
            case Operations.ShowParameters:
                break;
            case Operations.ShowScheduledItems:
                break;
            default:
                break;
        }

    }
}