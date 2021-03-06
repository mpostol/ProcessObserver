//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System;
using System.Windows.Forms;
using UAOOI.ProcessObserver.Configuration;
using UAOOI.Windows.Forms;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.HMI.Import
{
  /// <summary>
  /// Summary description for ImportTagBits.
  /// </summary>
  internal class ImportTagBits : ImportFunctionRootClass
  {
    #region ImportTagBitsInfo

    internal class ImportTagBitsInfo : ImportFileControll.ImportInfo
    {
      public override string ImportName => "Import Tag Bits information";
      public override string InitialDirectory => AppDomain.CurrentDomain.BaseDirectory;

      /// <summary>
      /// default browse filter for the dialog which is used for selecting a file
      /// </summary>
      public override string BrowseFilter => "CSV Tag bits definition file (*.CSV)|*.CSV";

      /// <summary>
      /// default extension for the dialog which is used for selecting a file
      /// </summary>
      public override string DefaultExt => ".CSV";

      /// <summary>
      /// text that is used to show the information about this importing function
      /// </summary>
      public override string InformationText => "This function import tab-bits from file - each line format: BaseTagName;Bitnumber;Name";
    }

    #endregion ImportTagBitsInfo

    #region private

    private ComunicationNet m_database;
    private readonly ImportTagBitsInfo m_ImportTagBitsInfo;
    private int m_numberofTagBitsadded = 0;

    #endregion private

    #region ImportFunctionRootClass

    protected override void DoTheImport()
    {
      m_numberofTagBitsadded = 0;
      CSVManagement _CSVContainer = null;
      try
      {
        _CSVContainer = CSVManagement.ReadFile(this.m_ImportTagBitsInfo.Filename);
      }
      catch (Exception ex)
      {
        AppendToLog("problem with file " + this.m_ImportTagBitsInfo.Filename + " :" + ex.Message);
        return;
      }
      //przed chwila pozbylismy sie pierwszej lini i wszystkich znakow konca lini teraz:
      string BaseTagName = "";
      string Bitnumber = "";
      string Name = "";
      while (_CSVContainer.ToString().Length > 0)
      {
        try
        {
          //format: BaseTagName;Bitnumber;Name
          //odczytujemy BaseTagName:
          BaseTagName = _CSVContainer.GetAndMove2NextElement();
          //odczytujemy BaseTagName:
          Bitnumber = _CSVContainer.GetAndMove2NextElement();
          //odczytujemy BaseTagName:
          Name = _CSVContainer.GetAndMove2NextElement();
          //odnajdujemy odpowiendniego taga bazowego w bazie
          foreach (ComunicationNet.TagsRow trow in m_database.Tags)
          {
            if (trow.Name.Equals(BaseTagName))
            {
              //znalezlismy odpowiedniego base taga - dodajmy tagbita
              ComunicationNet.TagBitRow tagbitrow = m_database.TagBit.NewTagBitRow(trow, string.Empty);
              tagbitrow.Name = Name;
              tagbitrow.BitNumber = System.Convert.ToInt16(Bitnumber);
              m_database.TagBit.AddTagBitRow(tagbitrow);
              m_numberofTagBitsadded++;
            }
          }
        }
        catch (
Exception
#if DEBUG
 ex
#endif
)
        {
          AppendToLog("problem with: BaseTagName:" + BaseTagName + " Bitnumber:" + Bitnumber + " :"
#if DEBUG
 + ex.Message.ToString()
#endif
);
        }
      }//while (sourcefile)
      AppendToLog("Number of TagBits added: " + m_numberofTagBitsadded.ToString());
    }

    #endregion ImportFunctionRootClass

    #region creator

    public ImportTagBits(ComunicationNet database, Form parentForm)
      : base(parentForm)
    {
      m_database = database;
      m_ImportTagBitsInfo = new ImportTagBitsInfo();
      SetImportInfo(m_ImportTagBitsInfo);
    }

    #endregion creator
  }
}