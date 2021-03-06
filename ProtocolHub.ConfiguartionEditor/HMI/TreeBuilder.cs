//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.CommServer.ProtocolHub.ConfigurationEditor.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UAOOI.ProcessObserver.Configuration;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.HMI
{
  internal static class TreeBuilder
  {
    #region private

    private static bool configLoadedFlag = false;
    private static TreeBuilderTreeViewType treeviewType = TreeBuilderTreeViewType.StationsAndChannels;

    #endregion private

    #region Properties

    /// <summary>
    /// TreeView type
    /// </summary>
    internal static TreeBuilderTreeViewType TreeViewType
    {
      get => treeviewType;
      set => treeviewType = value;
    }

    #endregion Properties

    #region Methods

    #region Public methods

    /// <summary>
    /// Creates TreeView based on given parameters
    /// </summary>
    /// <param name="configDataBase">Configuration data set</param>
    /// <param name="parentNode">From whith node create the tree</param>
    /// <param name="type"></param>
    public static void CreateTree(ComunicationNet configDataBase, TreeView parentNode, TreeBuilderTreeViewType type)
    {
      treeviewType = type;
      CreateTree(configDataBase, parentNode);
    }

    /// <summary>
    /// Creates TreeView based on given parameters. If TreeView type was not set loads it from config file.
    /// </summary>
    /// <param name="configDataBase">Configuration data set</param>
    /// <param name="parentNode">From with node create the tree</param>
    public static void CreateTree(ComunicationNet configDataBase, TreeView parentNode)
    {
      LoadConfig();
      switch (treeviewType)
      {
        case TreeBuilderTreeViewType.Stations:
          CreateTreeFromStations(configDataBase, parentNode);
          break;

        case TreeBuilderTreeViewType.Channels:
          CreateTreeFromChannels(configDataBase, parentNode);
          break;

        case TreeBuilderTreeViewType.StationsAndChannels:
          CreateTreeFromStationAndChannels(configDataBase, parentNode);
          break;
      }
    }

    #endregion Public methods

    #region Private methods

    /// <summary>
    /// Creates the TreeView only with station view
    /// </summary>
    /// <param name="configDataBase"></param>
    /// <param name="parentNode"></param>
    private static void CreateTreeFromStations(ComunicationNet configDataBase, TreeView parentNode)
    {
      parentNode.Nodes.Clear();
      parentNode.Nodes.Add(Wrappers.SA_TopWrapper.CreateNode(configDataBase));
    }

    /// <summary>
    /// Creates the TreeView only with channels view.
    /// </summary>
    /// <param name="configDataBase"></param>
    /// <param name="parentNode"></param>
    private static void CreateTreeFromChannels(ComunicationNet configDataBase, TreeView parentNode)
    {
      parentNode.Nodes.Clear();
      parentNode.Nodes.Add(Wrappers.CA_TopWrapper.CreateNode(configDataBase));
    }

    /// <summary>
    /// Creates the TreeView with channels and stations
    /// </summary>
    /// <param name="configDataBase"></param>
    /// <param name="parentNode"></param>
    private static void CreateTreeFromStationAndChannels(ComunicationNet configDataBase, TreeView parentNode)
    {
      List<TreeNode> _nodes = new List<TreeNode>();
      _nodes.AddRange(parentNode.Nodes.Cast<TreeNode>());
      foreach (TreeNode cn in _nodes)
        ((IAction)cn.Tag).Dispose();
      parentNode.Nodes.Add(Wrappers.CA_TopWrapper.CreateNode(configDataBase));
      parentNode.Nodes.Add(Wrappers.SA_TopWrapper.CreateNode(configDataBase));
    }

    /// <summary>
    /// Retrives default TreeView type from app.config file
    /// </summary>
    private static void LoadConfig()
    {
      if (!configLoadedFlag)
      {
        try
        {
          treeviewType = (TreeBuilderTreeViewType)Convert.ToInt32(Settings.Default.treeViewType);
        }
        //Podczas pracy na cas003 przy probie uzyskania konfiguracji zwaracany jest wyjatek (brak uprawnien do jakiegos rejestru), lokalnie jest wszystko ok
        catch (Exception)
        {
          treeviewType = TreeBuilderTreeViewType.StationsAndChannels;
        }
        configLoadedFlag = true;
      }
    }

    #endregion Private methods

    #endregion Methods
  }
}