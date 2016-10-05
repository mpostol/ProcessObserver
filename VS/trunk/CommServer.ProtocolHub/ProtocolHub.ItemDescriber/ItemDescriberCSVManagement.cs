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
    /// Saves the the configuration in the CSV file.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <param name="filename">The filename.</param>
    public void SaveCSV(ItemDecriberDataSet config, string filename)
    {
      using (StreamWriter _sw = File.CreateText(filename))
      {
        //zapisujemy najpierw pierwsza linie:
        _sw.Write("ItemID;ItemName;");
        foreach (ItemDecriberDataSet.PropertyRow _row in config.Property.Rows)
        {
          _sw.Write(_row.Name + ";");
        }
        _sw.WriteLine();
        //teraz zapisujemy w³aœciwe dane
        foreach (ItemDecriberDataSet.ItemsRow _itemRow in config.Items.Rows)
        {
          _sw.Write(_itemRow.ItemID.ToString() + ";");
          _sw.Write(_itemRow.ItemName + ";");
          foreach (ItemDecriberDataSet.PropertyRow row in config.Property.Rows)
          {
            ItemDecriberDataSet.ItemPropertyRow[] itemProperties = _itemRow.GetItemPropertyRows();
            foreach (ItemDecriberDataSet.ItemPropertyRow _property in itemProperties)
              if (_property.PropertyCode.Equals(row.Code)) _sw.Write(_property.Value);
            _sw.Write(";");
          }
          _sw.WriteLine("ENDLINE;");
        }
        _sw.Close();
      }
    }
    /// <summary>
    /// Loads the <see cref="ItemDecriberDataSet"/> form CSV file.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <param name="filename">The filename.</param>
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
