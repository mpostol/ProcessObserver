//<summary>
//  Title   : Configuration management utilities
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20081224: mzbrzezny: XMLManagement.cs is checking whether the configuration file exists before throwing an exception
//    2005: mzbrzezny: created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Data;
using System;
using System.IO;
using CAS.Lib.RTLib.Utils;
using CAS.Lib.RTLib.Processes;
using System.Xml;
using System.Text;

namespace BaseStation.ItemDescriber
{
  /// <summary>
  /// Opens and reads configuration from XML file.
  /// </summary>
  public class XMLManagement
  {
    private string itemdscHasNotBeenSet = "The location of the item_dsc.xml file has not been set in the config file";
    private string itemdscDoesNotExists = "The item description file {0} does not exist";
    private string itemdscCannotBeOpened = "Item_dsc.xml file cannot be opened";
    private FileStream _FileStream;

    /// <summary>
    /// reading of configuration xml file
    /// </summary>
    /// <param name="myData">target data set</param>
    /// <param name="filename">filename</param>
    public void readXMLFile(DataSet myData, string filename)
    {
      if (string.IsNullOrEmpty(filename))
      {
        EventLogMonitor.WriteToEventLogInfo(itemdscHasNotBeenSet, 39);
        return;
      }
      else if (filename == "item_dsc.xml")
      {
        FileInfo fi = RelativeFilePathsCalculator.GetAbsolutePathToFileInApplicationDataFolder(filename);
        string itemdscPath = fi.FullName;
        if (!new FileInfo(itemdscPath).Exists)
        {
          EventLogMonitor.WriteToEventLog(itemdscDoesNotExists, System.Diagnostics.EventLogEntryType.Warning);
          return;
        }
        else
          filename = itemdscPath;
      }
      else if (!new FileInfo(filename).Exists)
      {
        EventLogMonitor.WriteToEventLog(string.Format(itemdscDoesNotExists, filename), System.Diagnostics.EventLogEntryType.Warning);
        return;
      }
      myData.Clear();
      try
      {
        myData.ReadXml(filename, XmlReadMode.IgnoreSchema);
      }
      catch (Exception)
      {
        EventLogMonitor.WriteToEventLog(itemdscCannotBeOpened, System.Diagnostics.EventLogEntryType.Warning);
      }
    }
    /// <summary>
    /// writing of configuration data set
    /// </summary>
    /// <param name="myData">dataset to be written</param>
    /// <param name="filename">target filename</param>
    public void writeXMLFile(DataSet myData, string filename)
    {
      _FileStream = new FileStream(filename, FileMode.Create);
      //Create an XmlTextWriter with the fileStream.
      XmlTextWriter _XmlWriter = new XmlTextWriter(_FileStream, Encoding.Unicode);
      _XmlWriter.Formatting = Formatting.Indented;
      myData.WriteXml(_XmlWriter);
      _XmlWriter.Close();
    }
    /// <summary>
    /// constructor for xml management
    /// </summary>
    public XMLManagement()
    {
    }
  }
}
