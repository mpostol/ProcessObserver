//_______________________________________________________________
//  Title   : CSVManagement
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2016, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System;
using System.IO;

namespace BaseStation.ItemDescriber
{
  /// <summary>
  /// Summary description for CSVManagement.
  /// </summary>
  public class CSVManagement
  {
    /// <summary>
    /// constructor for csv management
    /// </summary>
    public CSVManagement()
    {
    }
    public void SaveCSV(ItemDecriberDataSet config, string filename)
    {
      using (StreamWriter sw = File.CreateText(filename))
      {
        //zapisujemy najpierw pierwsza linie:
        sw.Write("ItemID;ItemName;");
        foreach (ItemDecriberDataSet.PropertyRow row in config.Property.Rows)
        {
          sw.Write(row.Name + ";");
        }
        sw.WriteLine();
        //teraz zapisujemy w³aœciwe dane
        foreach (ItemDecriberDataSet.ItemsRow itemrow in config.Items.Rows)
        {
          sw.Write(itemrow.ItemID.ToString() + ";");
          sw.Write(itemrow.ItemName + ";");
          foreach (ItemDecriberDataSet.PropertyRow row in config.Property.Rows)
          {
            ItemDecriberDataSet.ItemPropertyRow[] itemproperties = itemrow.GetItemPropertyRows();
            foreach (ItemDecriberDataSet.ItemPropertyRow itemprop in itemproperties)
            {
              if (itemprop.PropertyCode.Equals(row.Code)) sw.Write(itemprop.Value);
            }
            sw.Write(";");
          }
          sw.WriteLine("ENDLINE;");
        }
        sw.Close();
      }
    }
    public void LoadCSV(ItemDecriberDataSet config, string filename)
    {
      StreamReader plik = new StreamReader(filename);//,System.Text.Encoding.Default);
      string sourcefile = plik.ReadToEnd();
      plik.Close();
      int pos = sourcefile.IndexOf("\r\n");
      sourcefile = sourcefile.Remove(0, pos + 2);
      sourcefile = sourcefile.Replace(";\r\n", ";");
      sourcefile = sourcefile.Replace("\r\n", ";");
      //przed chwila pozbylismy sie pierwszej lini i wszystkich znakow konca lini teraz:
      while (sourcefile.Length > 0)
      {
        //odczytujemy ID
        pos = sourcefile.IndexOf(";");
        string itemId_str = sourcefile.Substring(0, pos);
        sourcefile = sourcefile.Remove(0, pos + 1);
        int itemID = System.Convert.ToInt32(itemId_str);
        //odczytujemy nazwe
        pos = sourcefile.IndexOf(";");
        string item_name = sourcefile.Substring(0, pos);
        sourcefile = sourcefile.Remove(0, pos + 1);
        //dodajemy nowego Itema
        try
        {
          ItemDecriberDataSet.ItemsRow row = config.Items.NewItemsRow();
          row.ItemID = itemID;
          row.ItemName = item_name;
          config.Items.AddItemsRow(row);
        }
        catch (Exception)
        {
        }
        string wartosc = "";
        foreach (ItemDecriberDataSet.PropertyRow rowp in config.Property.Rows)
        {
          pos = sourcefile.IndexOf(";");
          wartosc = sourcefile.Substring(0, pos);
          sourcefile = sourcefile.Remove(0, pos + 1);
          if (wartosc != "")
          {
            try
            {
              ItemDecriberDataSet.ItemPropertyRow row = config.ItemProperty.NewItemPropertyRow();
              row.ItemID = itemID;
              row.PropertyCode = rowp.Code;
              row.Value = wartosc;
              config.ItemProperty.AddItemPropertyRow(row);
            }
            catch (Exception)
            {
            }
          }
        }
        pos = sourcefile.IndexOf(";");
        wartosc = sourcefile.Substring(0, pos);
        sourcefile = sourcefile.Remove(0, pos + 1);
      }
    }
  }
}
