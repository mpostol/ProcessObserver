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
  /// Summary description for ImportTagMappings.
  /// </summary>
  internal class ImportTagMappings : ImportFunctionRootClass
  {
    #region ImportTagMappingsInfo

    internal class ImportTagMappingsInfo : ImportFileControll.ImportInfo
    {
      public override string ImportName => "Import Tag Mappings";
      public override string InitialDirectory => AppDomain.CurrentDomain.BaseDirectory;

      /// <summary>
      /// default browse filter for the dialog which is used for selecting a file
      /// </summary>
      public override string BrowseFilter => "CSV Tag mappings definition file (*.CSV)|*.CSV";

      /// <summary>
      /// default extension for the dialog which is used for selecting a file
      /// </summary>
      public override string DefaultExt => ".CSV";

      /// <summary>
      /// text that is used to show the information about this importing function
      /// </summary>
      public override string InformationText => "This function changes the names of tag - each line format: PreviousName;NewName";
    }

    #endregion ImportTagMappingsInfo

    #region private

    private ComunicationNet m_database;
    private ImportTagMappingsInfo m_ImportTagMappingsInfo;

    #endregion private

    #region ImportFunctionRootClass

    protected override void DoTheImport()
    {
      #region IMPORT

      int changes_number = 0;
      CSVManagement _csvContainer = CSVManagement.ReadFile(m_ImportTagMappingsInfo.Filename);
      while (_csvContainer.ToString().Length > 0)
      {
        string _baseName = "";
        string _destinationName = "";
        try
        {
          _baseName = _csvContainer.GetAndMove2NextElement();
          _destinationName = _csvContainer.GetAndMove2NextElement();
          bool _tagHasBeenFound = false;
          foreach (ComunicationNet.TagsRow trow in m_database.Tags)
          {
            if (trow.Name.Equals(_baseName))
            {
              trow.Name = _destinationName;
              changes_number++;
              _tagHasBeenFound = true;
              break;
            }
          }
          if (!_tagHasBeenFound)
            AppendToLog("Tag " + _baseName + " -> " + _destinationName + " is not found");
        }
        catch (
Exception
#if DEBUG
 ex
#endif
)
        {
          AppendToLog("problem with: base:" + _baseName + " dest:" + _destinationName + " :"
#if DEBUG
 + ex.Message.ToString()
#endif
);
        }
      }

      #endregion IMPORT

      AppendToLog("Number of changed tags: " + changes_number.ToString());
    }

    #endregion ImportFunctionRootClass

    #region creator

    public ImportTagMappings(ComunicationNet database, Form parentForm)
      : base(parentForm)
    {
      m_database = database;
      m_ImportTagMappingsInfo = new ImportTagMappingsInfo();
      SetImportInfo(m_ImportTagMappingsInfo);
    }

    #endregion creator
  }
}