//<summary>
//  Title   : Configuration management utilities
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>


namespace BaseStation.ItemDescriber
{
  /// <summary>
  /// Summary description for ItemDescriber2OpcDA.
  /// </summary>
  public class ItemDescriber2OpcDA
  {
    /// <summary>
    /// constructor for ItemDescriber2OpcDA
    /// </summary>
    public ItemDescriber2OpcDA() { }
    /// <summary>
    /// static function that read item property collection for seleted item
    /// </summary>
    /// <param name="ItemName">item name to be read</param>
    /// <param name="ds">data set with settings</param>
    /// <returns>collection of properies</returns>
    public static Opc.Da.ItemPropertyCollection GetItemPropertiesCollection(string ItemName, ItemDecriberDataSet ds)
    {
      Opc.Da.ItemPropertyCollection ret = null;
      if (ds != null)
      {
        ret = new Opc.Da.ItemPropertyCollection();
        foreach (ItemDecriberDataSet.ItemsRow row_items in ds.Items.Rows)
        {
          if (row_items.ItemName == ItemName)
          {
            foreach (ItemDecriberDataSet.ItemPropertyRow row_property in row_items.GetItemPropertyRows())
            {
              Opc.Da.PropertyDescription prop_dsc = Opc.Da.PropertyDescription.Find(new Opc.Da.PropertyID(row_property.PropertyCode));
              Opc.Da.ItemProperty itemprop = new Opc.Da.ItemProperty();
              itemprop.ID = prop_dsc.ID;
              itemprop.Value = row_property.Value;
              ret.Add(itemprop);
            }
            break;
          }
        }
      }
      return ret;
    }
  }
}
