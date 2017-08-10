
//_______________________________________________________________
//  Title   : CommServer Monitor Console MainForm
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2017, CAS LODZ POLAND.
//  TEL: +48 608 61 98 99 
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.CommServer.ProtocolHub.MonitorInterface;
using CAS.CommServerConsole.Properties;
using CAS.Lib.ControlLibrary;
using CAS.Lib.RTLib.Management;
using CAS.Lib.RTLib.Processes;
using CAS.Windows.Forms.CodeProtectControls;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows.Forms;

namespace CAS.CommServerConsole
{
  /// <summary>
  /// Main Form Class for CommServer.
  /// </summary>
  public partial class MainForm
  {
    #region statistics classes
    private interface MasterRefresh
    {
      void Refresh();
    }
    private class Interface : System.Windows.Forms.ListViewItem//, MasterRefresh
    {
      #region PRIVATE
      //      private bool needService;
      //      private ushort m_imageIdx;
      private Statistics.InterfaceStatistics.InterfaceStatisticsInternal myStatistics;
      //      private void myInterfaceStat_UpdateInterfaceState
      //        (BaseStation.Management.Statistics.Interface.InterfaceState currState)
      //      {
      //        lock(this)
      //        {
      //          needService = true;
      //          m_imageIdx = Convert.ToByte( currState == Statistics.Interface.InterfaceState.Active );
      //        }
      //      }
      #endregion
      #region PUBLIC
      //      void MasterRefresh.Refresh()
      //      {
      //        lock(this)
      //        {
      //          if ( needService ) this.ImageIndex = m_imageIdx;
      //        }
      //      }
      public override string ToString()
      {
        return myStatistics.ToString();
      }
      public Statistics.InterfaceStatistics.InterfaceStatisticsInternal Parent
      {
        get { return myStatistics; }
      }
      public Interface(Statistics.InterfaceStatistics.InterfaceStatisticsInternal statistics)
        : base(statistics.ToString())
      {
        myStatistics = statistics;
        //        myStatistics.UpdateInterfaceState += 
        //          new Statistics.Interface.InterfaceStateChanged(myInterfaceStat_UpdateInterfaceState);
        //        ObjectToRefresh.Add(this);
      }
      #endregion
    }//interface
    private class Segment : ListViewItem
    {
      #region PRIVATE
      //      private bool needService;
      //      private ushort m_imageIdx;
      private Statistics.SegmentStatistics.SegmentStatisticsInternal myStatistics;
      //      private void myStatistics_MarkNewState(Statistics.Segment.States currState)
      //      {
      //        lock(this)
      //        {
      //          needService = true;
      //          m_imageIdx = Convert.ToByte( currState == Statistics.Segment.States.Connected );
      //        }
      //      }
      #endregion
      #region PUBLIC
      public Statistics.SegmentStatistics.SegmentStatisticsInternal Parent
      {
        get { return myStatistics; }
      }
      public override string ToString()
      {
        return myStatistics.ToString();
      }
      public Segment(Statistics.SegmentStatistics.SegmentStatisticsInternal statistics)
        : base(statistics.ToString())
      {
        myStatistics = statistics;
        //        myStatistics.MarkNewState += new Statistics.Segment.StateChanged(myStatistics_MarkNewState);
        //        ObjectToRefresh.Add(this);
      }
      #endregion
    }//Segment
    private class Station : System.Windows.Forms.ListViewItem//, MasterRefresh
    {
      #region PRIVATE]
      //      private bool needService;
      //      private ushort m_imageIdx;
      private Statistics.StationStatistics.StationStatisticsInternal myStatistics;
      //      private void myStatistics_MarkNewState(bool currState)
      //      {
      //        lock(this)
      //        {
      //          needService = true;
      //          m_imageIdx = Convert.ToByte( currState );
      //        }
      //      }
      #endregion
      #region PUBLIC
      //      void MasterRefresh.Refresh()
      //      {
      //        lock(this)
      //        {
      //          if ( needService ) 
      //          {
      //            this.ImageIndex = m_imageIdx;
      //            needService = false;
      //          }
      //        }
      //      }
      public override string ToString()
      {
        return myStatistics.myName;
      }
      public Statistics.StationStatistics.StationStatisticsInternal Parent
      {
        get { return myStatistics; }
      }
      public Station(Statistics.StationStatistics.StationStatisticsInternal statistics)
        : base(statistics.myName)
      {
        myStatistics = statistics;
        //        statistics.MarkNewState += new Statistics.StateChanged(myStatistics_MarkNewState);
        //        ObjectToRefresh.Add(this);
      }
      #endregion
    }//Station
    #endregion statistics classes
    #region refresh pages
    private const string ns = "not selected";
    private void RefreshStationPage()
    {
      SortedList<long, int> statesList = m_remoterServer.GetStationStates();
      foreach (Station cs in StationsListView.Items)
        if (statesList.ContainsKey(cs.Parent.myID))
          cs.ImageIndex = statesList[cs.Parent.myID];
      if (StationsListView.SelectedItems.Count > 0)
      {
        Statistics.StationStatistics.StationStatisticsInternal curr = m_remoterServer.GetStation(((Station)StationsListView.SelectedItems[0]).Parent.myID);
        if (curr != null)
        {
          groupBoxCurrStation.Text = curr.ToString();
          label_StationCurrState.Text = curr.StationStateString;
        }
        else
        {
          groupBoxCurrStation.Text = ns;
          label_StationCurrState.Text = ns;
        }

      }
    }
    private void RefreshProtocolPage()
    {
      if (listBoxProtocol.Items.Count == 0)
        return;
      IProtocol curr =
          //(BaseStation.Management.Protocol)listBoxProtocol.SelectedItem; 
          m_remoterServer.GetProtocol(((ProtocolDsc)listBoxProtocol.SelectedItem).m_ID);
      if (curr != null)
      {
        textBox_protpar.Text = ((ProtocolDsc)listBoxProtocol.SelectedItem).m_protocolPar_humanreadable;
        labelRxDBPlusVal.Text = curr.GetRXDBSucc.ToString();
        labelRxDBMinusVal.Text = curr.GetRXDBFail.ToString();
        labelTxDBPlusVal.Text = curr.GetTXDBSucc.ToString();
        labelTxDBMinVal.Text = curr.GetTXDBFail.ToString();
        labelRXFramesCount.Text = curr.GetStRxFrameCounter.ToString();
        labelTXFramesCount.Text = curr.GetStTxFrameCounter.ToString();
        labelCRCErrorsCnt.Text = curr.GetStRxCRCErrorCounter.ToString();
        labelIncompleteCnt.Text = curr.GetStRxFragmentedCounter.ToString();
        labelTimeOutCnt.Text = curr.GetStRxNoResponseCounter.ToString();
        labelProtocolnvalidVVal.Text = curr.GetStRxInvalid.ToString();
        LabelProtocolRxSynchErrorVal.Text = curr.GetStRxSynchError.ToString();
        labelProtNAKVal.Text = curr.GetStRxNAKCounter.ToString();
        labelMaxResponseTime.Text = curr.GetTimeMaxResponseDelay;
        labeProtlMaxCharGapTime.Text = curr.GetTimeCharGap;
      }
    }
    private void RefreshSegmentPage()
    {
      SortedList<long, Statistics.SegmentStatistics.States> statesList = m_remoterServer.GetSegmentStates();
      foreach (Segment cs in SegmentsListView.Items)
        if (statesList.ContainsKey(cs.Parent.MyID))
          cs.ImageIndex = (int)statesList[cs.Parent.MyID];
      if (SegmentsListView.SelectedItems.Count > 0)
      {
        //Statistics.Segment curr = ( (Segment)SegmentsListView.SelectedItems[0] ).Parent;
        Statistics.SegmentStatistics.SegmentStatisticsInternal curr = m_remoterServer.GetSegment(((Segment)(SegmentsListView.SelectedItems[0])).Parent.MyID);
        SegmentsListView.Text = curr.ToString();
        labelSegStateVal.Text = curr.CurrentStateAsString;
        labelSegAciveVal.Text = curr.ConnectTime.ToString();
        labelSegConnectionsVal.Text = curr.GetMadeCount.ToString();
        labelSegConnFailVal.Text = curr.GetFailCount.ToString();
        labelSegWriteDelayVal.Text = curr.WriteDelay;
        labelSegReadDelayVal.Text = curr.ReadDelay;
        labelSegConnectMMAVal.Text = curr.GetSegmentConnectionTimeMinimumMximumAverage;
        labelSegDataOvertimeVal.Text = curr.GetOvertimeCoefficient;
        groupBoxSegmentCurr.Text = curr.ToString() + " statistics";
      }
      else
      {
        SegmentsListView.Text = ns;
        labelSegStateVal.Text = ns;
        labelSegAciveVal.Text = ns;
        labelSegConnectionsVal.Text = ns;
        labelSegConnFailVal.Text = ns;
        labelSegWriteDelayVal.Text = ns;
        labelSegReadDelayVal.Text = ns;
        labelSegConnectMMAVal.Text = ns;
        labelSegDataOvertimeVal.Text = ns;
      }
    }
    private void RefreshInterfacePage()
    {
      SortedList<ulong, Statistics.InterfaceStatistics.InterfaceState> statesList = m_remoterServer.GetInterfaceStates();
      foreach (Interface ci in InterfaceListView.Items)
        if (statesList.ContainsKey(ci.Parent.myID_Internal))
          ci.ImageIndex = (int)statesList[ci.Parent.myID_Internal];
      if (InterfaceListView.SelectedItems.Count == 0)
      {
        labelIntStateVal.Text = ns;
        labelIntActiveTimeVal.Text = ns;
        labelIntFailTimeVal.Text = ns;
        labelIntStandByVal.Text = ns;
        groupBoxCurrInt.Text = ns;
      }
      else
      {
        Statistics.InterfaceStatistics.InterfaceStatisticsInternal currInt = m_remoterServer.GetInterface(((Interface)InterfaceListView.SelectedItems[0]).Parent.myID_Internal);
        labelIntStateVal.Text = currInt.GetState2String;
        labelIntActiveTimeVal.Text = currInt.ActiveTime.ToString();
        labelIntFailTimeVal.Text = currInt.FailTime.ToString();
        labelIntStandByVal.Text = currInt.StandbyTime.ToString();
        groupBoxCurrInt.Text = currInt.ToString();
      }
    }
    private void RefreshMainForm()
    {
      try
      {
        labelTestTime.Text = new TimeSpan(0, 0, (int)m_remoterServer.GetRunTime()).ToString();
        labelProgramName.Text = m_remoterServer.GetProductName();
        labelRelease.Text = m_remoterServer.GetProductVersion();
      }
      catch (Exception)
      {
      }
    }
    #endregion refresh pages
    #region Init Pages
    private void InitStationsPage()
    {
      foreach (Statistics.StationStatistics.StationStatisticsInternal curr in m_remoterServer.GetStationList().Values)
      {
        Station listItem = new Station(curr);
        listItem.ImageIndex = 0;
        StationsListView.Items.Add(listItem);
      }
      StationsListView.Refresh();
    }//InitStationsPage
    private void InitProtocolPage()
    {
      foreach (ProtocolDsc curr in m_remoterServer.GetProtocolList())
      {
        listBoxProtocol.Items.Add(curr);
      }
      if (listBoxProtocol.Items.Count == 0)
        return;
      listBoxProtocol.SelectedIndex = 0;
    }
    private void InitSegmentPage()
    {
      foreach (Statistics.SegmentStatistics.SegmentStatisticsInternal curr in m_remoterServer.GetSegmentList().Values)
      {
        Segment newRep = new Segment(curr);
        newRep.ImageIndex = 0;
        SegmentsListView.Items.Add(newRep);
      }
      SegmentsListView.Refresh();
    }
    private void InitInterfacePage()
    {
      foreach (Statistics.InterfaceStatistics.InterfaceStatisticsInternal currInt in m_remoterServer.GetInterfaceList().Values)
      {
        Interface newRep = new Interface(currInt);
        newRep.ImageIndex = 0;
        InterfaceListView.Items.Add(newRep);
      }
      InterfaceListView.Refresh();
    }
    #endregion
    #region Creator
    internal MainForm()
    {
      InitializeComponent();
      if (!DisplayQuestionAboutConfiguration())
        return;
      ConnectToRemoteServerAndInitializeDiagnosticPages();
      RefreshFormTimer.Enabled = true;
      new EventLogMonitor
        (
        "Communication server (MainForm) started: product name:" + Application.ProductName
        + "; product version: " + Application.ProductVersion,
        System.Diagnostics.EventLogEntryType.Information, 0, 0
        ).WriteEntry();
    }
    private void ConnectToRemoteServerAndInitializeDiagnosticPages()
    {
      GoService();
      InitStationsPage();
      InitProtocolPage();
      InitInterfacePage();
      InitSegmentPage();
    }
    #endregion
    #region private
    private ConsoleInterfaceAbstract m_remoterServer;
    private void GoService()
    {
      //      HttpClientChannel channel = new HttpClientChannel();
      TcpClientChannel channel = new TcpClientChannel();
      ChannelServices.RegisterChannel(channel, false);

      string RemoteHost = Settings.Default.CommServer_Host_Primary;
      int RemotePort = Settings.Default.CommServer_ListenPort_Primary;
      if (Settings.Default.UseAlternativeConfiguration)
      {
        RemoteHost = Settings.Default.CommServer_Host_AlternativeConfiguration;
        RemotePort = Settings.Default.CommServer_ListenPort_AlternativeConfiguration;
      }

      label_connected_to.Text = String.Format(Settings.Default.CommServer_Connection_Template, RemoteHost, RemotePort);

      //      ulong times1 =  testTimeStopWatch.Start;
      m_remoterServer = (ConsoleInterfaceAbstract)Activator.GetObject(typeof(ConsoleInterfaceAbstract), label_connected_to.Text);
    }

    #region Events Handlers
    private void MainForm_Resize(object sender, EventArgs e)
    {
      if (this.WindowState == FormWindowState.Minimized)
        this.Hide();
    }
    private void EMainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      string message = Properties.Resources.Tx_MainFormClosing;
      string caption = this.Text;
      MessageBoxButtons buttons = MessageBoxButtons.YesNo;
      DialogResult result;
      // Displays the MessageBox.
      result = MessageBox.Show(this, message, caption, buttons,
        MessageBoxIcon.Question, MessageBoxDefaultButton.Button1,
        MessageBoxOptions.RightAlign);
      if (result != DialogResult.Yes)
      {
        e.Cancel = true;
        RefreshFormTimer.Start(); // in case of the tmier is stopped(eg. lost of communication) - it is better to start it 
      }
    }
    private void notifyIcon_trayicon_DoubleClick(object sender, System.EventArgs e)
    {
      this.Show();
      this.WindowState = FormWindowState.Normal;
    }
    delegate void SplashScreenShow(int time);
    private void MainForm_Load(object sender, System.EventArgs e)
    {
      RunMethodAsynchronously runasync = new RunMethodAsynchronously(delegate (object[] param)
     {
       SplashScreen SplashScreenObj = new SplashScreen();
       SplashScreenObj.Show();
       SplashScreenObj.Refresh();
       int time = 1000;
       if (param != null)
         time = (int)param[0];
       System.Threading.Thread.Sleep(time);
       SplashScreenObj.Close();
     });
      runasync.RunAsync(new object[] { 2000 });
    }
    private bool DisplayQuestionAboutConfiguration()
    {
      if (!Settings.Default.DisplayConfigurationQuestionAtStartup)
        return false;
      bool _returnValue = false;
      using (OKCancelForm _questionForm = new OKCancelForm(this.Text))
      {
        ConfigurationQuestionControl _configuration = new ConfigurationQuestionControl();
        _questionForm.SetUserControl = _configuration;
        _questionForm.CanBeAccepted(true);
        if (_questionForm.ShowDialog(this) == DialogResult.OK)
        {
          Settings.Default.DisplayConfigurationQuestionAtStartup = _configuration.DisplayConfigurationQuestionAtStartup;
          Settings.Default.UseAlternativeConfiguration = _configuration.UseAlternativeConfiguration;
          Settings.Default.Save();
          _returnValue = true;
        }
      }
      return _returnValue;
    }
    private void TimerTick(object sender, System.EventArgs e)
    {
      try
      {
        RefreshMainForm();
        if (tabControlInterface.SelectedTab.Name == tabStation.Name)
          RefreshStationPage();
        if (tabControlInterface.SelectedTab.Name == tabPageSegments.Name)
          RefreshSegmentPage();
        if (tabControlInterface.SelectedTab.Name == tabPageInterface.Name)
          RefreshInterfacePage();
        if (tabControlInterface.SelectedTab.Name == tabPageProtocol.Name)
          RefreshProtocolPage();
        if (Manager.NumOfErrors > 0)
        {
          new EventLogMonitor
            (
            "Communication server console finished with Assert error", System.Diagnostics.EventLogEntryType.Error, 0, 0
            ).WriteEntry();
          this.Close();
        }
      }
      catch (Exception exc)
      {
        RefreshFormTimer.Stop();
        MessageBox.Show("CommServer - has ben stopped, it is highly recommended to close the console. "
#if DEBUG
 + " reason:" + exc.ToString()
#endif
 );
        new EventLogMonitor
          (
          "CommServer - has ben stopped, it is highly recommended to close the console.", System.Diagnostics.EventLogEntryType.Error, 0, 0
          ).WriteEntry();
        this.Close();
      }
    }
    #endregion Events Handlers

    #region Menue Events
    private void CM_Show_click(object sender, EventArgs e)
    {
      Show();
      this.WindowState = FormWindowState.Normal;
    }
    private void menu_Report_Click(object sender, EventArgs e)
    {
      RunMethodAsynchronously runasync = new RunMethodAsynchronously(delegate (object[] o)
     {
       CAS.Lib.RTLib.Utils.ReportGenerator.DisplayReport(m_remoterServer.GetReport());
     });
      runasync.RunAsync();
    }
    private void menu_Configurator_Click(object sender, EventArgs e)
    {
      StartAppAsync(AppDomain.CurrentDomain.BaseDirectory + "CAS.NetworkConfig.exe", "CommServer Network Configurator Tool");
    }
    private void menuItem_DCOM_configuration_Click(object sender, EventArgs e)
    {
      //dcomcnfg
      StartAppAsync("dcomcnfg", "DCOM configuration console");
    }
    private void menuItem_XBUS_Measure_Click(object sender, EventArgs e)
    {
      StartAppAsync("CAS.DPDiagnostics.exe", "XBUS measurement (Data Provider Diagnostic tool)");
    }
    private static void StartAppAsync(string appname, string longappname)
    {
      RunMethodAsynchronously runasync = new RunMethodAsynchronously(delegate (object[] o)
     {
       try
       {
         System.Diagnostics.Process.Start(appname);
       }
       catch (Exception ex)
       {
         MessageBox.Show(appname, "Cannot start the " + longappname + " :" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
       };
     }
      );
      runasync.RunAsync();
    }
    private void ShowAboutDialog()
    {
      new CAS.Lib.ControlLibrary.AboutForm(null, null, Assembly.GetEntryAssembly()).ShowDialog();
    }
    private void menu_About_Click(object sender, EventArgs e)
    {
      //About box contains web browser controll that must be run in STA!!
      System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(ShowAboutDialog));
      th.SetApartmentState(System.Threading.ApartmentState.STA);
      th.Start();
    }
    private void menu_Hide_Click(object sender, EventArgs e)
    {
      //      this.WindowState = FormWindowState.Minimized;
      this.Hide();
      notifyIcon_trayicon.ShowBalloonTip(5000);
    }
    private void menu_Exit_Click(object sender, EventArgs e)
    {
      notifyIcon_trayicon.Visible = false;
      notifyIcon_trayicon.Dispose();
      this.Close();
    }
    private void menuItem_options_Click(object sender, EventArgs e)
    {
      PropertyGrid myGrid = new PropertyGrid();
      myGrid.SelectedObject = Properties.Settings.Default;
      myGrid.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      int windowHight;
      int windowWidth;
      using (OKCancelForm myConfigurationForm = new OKCancelForm(this.Text + ":Option"))
      {
        UserControl myControl = new UserControl();
        myControl.Controls.Add(myGrid);
        myConfigurationForm.SetUserControl = myControl;
        myConfigurationForm.CanBeAccepted(true);
        myConfigurationForm.ShowDialog(this);
        while (myConfigurationForm.DialogResult == DialogResult.Yes)
        {
          windowHight = myConfigurationForm.Height;
          windowWidth = myConfigurationForm.Width;
          myConfigurationForm.StartPosition = FormStartPosition.Manual;
          myGrid.SelectedObject = Properties.Settings.Default;
          myControl.Controls.Clear();
          myControl.Controls.Add(myGrid);
          myConfigurationForm.SetUserControl = myControl;
          myConfigurationForm.Controls[0].Refresh();
          myConfigurationForm.Height = windowHight;
          myConfigurationForm.Width = windowWidth;
          myConfigurationForm.ShowDialog(this);
        }
      }
    }
    private void menuItem_connectionTypeOptions_Click(object sender, EventArgs e)
    {
      DisplayQuestionAboutConfiguration();
    }
    private void menuItem6_Click(object sender, EventArgs e)
    {
      System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(ShowLicenseDialog));
      th.SetApartmentState(System.Threading.ApartmentState.STA);
      th.Start();
    }
    private void menuItem7_Click(object sender, EventArgs e)
    {
      using (UnlockKeyDialog dialog = new UnlockKeyDialog())
      {
        dialog.ShowDialog();
      }
    }
    #endregion Menue Handlers
    private void MainForm_FormClosing(object sender, FormClosingEventArgs e) { }
    private void ShowLicenseDialog()
    {
      using (LicenseForm dial = new LicenseForm(null, null, Assembly.GetEntryAssembly()))
      {
        Licenses cLicDial = new Licenses();
        dial.SetAdditionalControl = cLicDial;
        dial.LicenceRequestMessageProvider = new LicenseForm.LicenceRequestMessageProviderDelegate(() => cLicDial.GetLicenseMessageRequest());
        dial.ShowDialog(this);
      }
    }
    #endregion

  }//MainForm

}
