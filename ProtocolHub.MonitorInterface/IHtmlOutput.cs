//<summary>
//  Title   : HTML output interface (for reports)
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    <Author> - <date>:
//    <description>
//    Maciej Zbrzezny - 12-04-2006
//    zmieoniono interfejs na publiczny
//		Maciej Zbrzezny - 25,03,2005
//			created
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http://www.cas.eu
//</summary>


namespace CAS.Lib.RTLib.Management
{
  /// <summary>
  /// Interface that provide main function for HTML output
  /// </summary>
  public interface IHtmlOutput
  {
    /// <summary>
    /// this function is for retrieving description for this object in the HTML
    /// </summary>
    /// <returns>string with HTML output</returns>
    string GetHtmlTableRowDescription();
    /// <summary>
    /// this function is for retrieving data that represents that object
    /// </summary>
    /// <returns>string with HTML data</returns>
    string ToHtmlTableRow();
    /// <summary>
    /// this function is for retrieving data that represents that object (data + description)
    /// </summary>
    /// <returns>string with HTML data</returns>
    string ToHtml();
  }
}
