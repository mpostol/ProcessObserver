//<summary>
//  Title   : NReport Generator
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>

using System;
using System.IO;

namespace CAS.Lib.RTLib.Utils
{
  /// <summary>
  /// Class that generate HTML report about OPC Client Transporter state
  /// </summary>
  [Obsolete("Must be rewritten. Use translation pr style-sheet.")]
  public abstract class ReportGenerator
  {
    /// <summary>
    /// temporary destination filename
    /// </summary>
    protected string DestFilename;
    /// <summary>
    /// title for this report
    /// </summary>
    protected string reporttitle;
    /// <summary>
    /// header for the report
    /// </summary>
    /// <returns></returns>
    protected virtual string getHeader()
    {
      System.Reflection.Assembly thisApp = System.Reflection.Assembly.GetExecutingAssembly();
      string[] inforesources = thisApp.GetManifestResourceNames();
      System.IO.Stream file = null;
      foreach (string resourcename in inforesources)
      {
        if (resourcename.LastIndexOf("cas_logo.gif") > 0)
        {
          file =
            thisApp.GetManifestResourceStream(resourcename);
        }
      }
      if (file != null)
      {
        System.Drawing.Image img = System.Drawing.Image.FromStream(file);
        img.Save(System.IO.Path.GetTempPath() + "\\cas_logo.gif");
      }

      //TODO: dopisac kopiowanie naszej ikony
      //      //bierzemy obrazek z resourcow
      //      System.Resources.ResourceManager rm = new System.Resources.ResourceManager("items",   System.Reflection.Assembly.GetExecutingAssembly());
      //      Object o=rm.GetObject ("cas_logo.gif");
      //      string destgif = System.IO.Path.GetTempFileName()+".gif";
      //      //BinaryWriter w = new BinaryWriter(fs);
      //
      //      using (BinaryWriter sw = new BinaryWriter(File.Create(DestFilename))) 
      //      {
      //        sw.Write(o);
      //      }
      //
      //System.Drawing.Image.Bitmap image = (Bitmap)rm.GetObject ("cas_logo.gif");


      //wpisujemy nag³ówek
      return @"
<!DOCTYPE HTML PUBLIC '-//W3C//DTD HTML 4.0 Transitional//EN'>
<HTML>
<HEAD>
<title>" + this.reporttitle + " DateTime=" + DateTimeProvider.GetCurrentTime().ToString().Replace(":", "") + @"</title>
<meta http-equiv=Content-Type content='text/html; charset=windows-1250'>
<META NAME='Author' CONTENT='Maciej Zbrzezny'>
<META NAME='Keywords' CONTENT='report'>
<META NAME='Description' CONTENT='report'>
<style>
body{background:white;}
h1{text-decoration: none; font-family: times new roman; font-size:40px; color: #000000; font-weight: bold;}
h2{text-decoration: none; font-style: italic; font-family: times new roman; font-size:22px; color: #000000; font-weight: bold;}

table.tt{padding-right: 0.5cm; padding-left: 0.5cm; border-style: inset; border-color: none; background-color: none;}
table.t1{border-color: none; background-color: none;}
table.t2{border-style: inset; border-color: none; background-color: none;}

td.kk{padding-left: 1cm; font-family: times new roman, times; font-size:17px;}
td.k1{background-color:none ; text-decoration: none; font-family: times new roman, times; font-size:16px; color: #000000; font-weight: bold;}
td.k2{background-color:none ; text-decoration: none; font-family: times new roman, times; font-size:14px; color: #000000; font-weight: none;}
td.k3{background-color:#eeeee0 ; text-decoration: none; font-family: times new roman, times; font-size:12px; color: #000000; font-weight: bold;}
td.k4{background-color:none ; text-decoration: none; font-family: times new roman, times; font-size:12px; color: #000000; font-weight: none;}
td.k41{background-color:#eeeeeb ; text-decoration: none; font-family: times new roman, times; font-size:12px; color: #000000; font-weight: none;}
td.k5{background-color:#eeeee0 ; text-decoration: none; font-family: times new roman, times; font-size:16px; color: #000000; font-weight: bold;
</style>
</HEAD>
<BODY >
<table align='center' width='782' cellspacing='0' cellpadding='0' border='0' class='tt'>
<tr><td  bgcolor='#021376' width='782' height='1'></td></tr>
         ";
    }
    /// <summary>
    /// Gets the footer of the report.
    /// </summary>
    /// <returns>footer in string format</returns>
    protected virtual string getFooter()
    {
      //wpisujemy stopke
      return @"
</td>
</tr>
</body>
</html>
        ";

    }

    /// <summary>
    /// generation a report
    /// </summary>
    protected abstract void doReport();
    /// <summary>
    /// Initializes a new instance of the <see cref="ReportGenerator"/> class.
    /// </summary>
    /// <param name="title">The title of the report.</param>
    public ReportGenerator(string title)
    {
      reporttitle = title;
      DestFilename = System.IO.Path.GetTempFileName() + ".html";
      //TODO remove call to this method - it calls abstract member
      doReport();
    }
    /// <summary>
    /// open web browser and display an report
    /// </summary>
    public void DisplayReport()
    {
      System.Diagnostics.Process.Start(DestFilename);
    }
    /// <summary>
    /// Displays the report.
    /// </summary>
    /// <param name="ReportString">The string that contains the report.</param>
    public static void DisplayReport(string ReportString)
    {
      string DestFilename = System.IO.Path.GetTempFileName() + ".html";
      //otwieramy
      using (StreamWriter sw = File.CreateText(DestFilename))
      {
        sw.WriteLine(ReportString);
      }
      System.Diagnostics.Process.Start(DestFilename);
    }
    /// <summary>
    /// Gets the string that represents the report.
    /// </summary>
    /// <returns></returns>
    public abstract string GetReportString();
  }
}
