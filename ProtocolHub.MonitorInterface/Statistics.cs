//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.RTLib.Management;
using System;
using System.Threading;
using UAOOI.ProcessObserver.RealTime.Processes;
using CommunicationDSC = UAOOI.ProcessObserver.Configuration.ComunicationNet;

namespace CAS.CommServer.ProtocolHub.MonitorInterface
{
  /// <summary>
  /// class that allows logging from statistics related classes - source "CAS.BaseStation.Management.Statistics"
  /// </summary>
  internal class StatisticsTracer
  {
    /// <summary>
    /// source "CAS.BaseStation.Management.Statistics"
    /// </summary>
    internal static readonly TraceEvent TraceSource = new TraceEvent("CAS.BaseStation.Management.Statistics");
  }

  /// <summary>
  /// Interface Update Internal Statistics
  /// </summary>
  public interface IUpdateInternalStatistics
  {
    /// <summary>
    /// Updates the internal statistic.
    /// </summary>
    void UpdateInternal();
  }

  /// <summary>
  /// Provides statistic information to management level - human machine interface (HMI)
  /// </summary>
  [Serializable]
  public class Metronom
  {
    #region private

    private delegate void StateChanged();

    private static event StateChanged stateChangedEvnt;

    private static MonitoredThread clock;

    private static void MonitoringMet()
    {
      while (true)
      {
        Thread.Sleep(TimeSpan.FromSeconds(1));
        clock.ResetWatchDog(10);
        if (stateChangedEvnt != null)
          stateChangedEvnt();
      }
    }

    #endregion private

    #region public

    /// <summary>
    /// Provides statistic information to management level - human machine interface (HMI)
    /// </summary>
    [Serializable]
    public abstract class RefreshAble
    {
      /// <summary>
      /// Refreshes this instance.
      /// </summary>
      protected abstract void refresh();

      private void RefreshHandler()
      {
        string mySource = "Metronom.RefreshAble:" + GetType().Name;
        try
        {
          refresh();
        }
        catch (Exception ex)
        {
          string message = ex.Message +
#if DEBUG
 "; stacktrace:" + ex.StackTrace.ToString() +
#endif
 "";
          StatisticsTracer.TraceSource.TraceError(104, mySource, message);
        }
      }

      internal RefreshAble()
      {
        stateChangedEvnt += new StateChanged(RefreshHandler);
      }
    }

    static Metronom()
    {
      clock = new MonitoredThread
        (
        600,
        "Metronome main scanner thread had to be timed out after 600s inactivity and cause system reboot",
        800,
        new System.Threading.ThreadStart(MonitoringMet),
        "Metronome",
        true,
        ThreadPriority.Normal
        );
    }

    #endregion public
  }

  /// <summary>
  /// Communication statistics management class
  /// </summary>
  [Serializable]
  public partial class Statistics
  {
    #region PRIVATE

    private enum RWOperationRes : ushort
    { ORReadGood, ORWriteGood, ORCRCError, ORIncoplete, ORTimeout, ORMaxTimeRound };

    /// <summary>
    ///  Title   : Communication statistics management class
    /// </summary>

    #endregion PRIVATE

    #region PUBLIC refresh

    public delegate void StateChanged(bool currState);

    /// <summary>
    /// list of station see:<see cref="StationStatistics"/>
    /// </summary>
    public static System.Collections.ArrayList stationList = new System.Collections.ArrayList();

    /// <summary>
    /// list of channels see: <see cref="ChannelStatistics"/>
    /// </summary>
    public static System.Collections.ArrayList channelList = new System.Collections.ArrayList();

    /// <summary>
    /// list of segments <see cref="SegmentStatistics"/>
    /// </summary>
    public static System.Collections.ArrayList segmentList = new System.Collections.ArrayList();

    /// <summary>
    /// list of interfaces <see cref="InterfaceStatistics"/>
    /// </summary>
    public static System.Collections.ArrayList interfaceList = new System.Collections.ArrayList();

    /// <summary>
    /// Provides statistic information to management level - human machine interface (HMI)
    /// </summary>
    [Serializable]
    public abstract class StationStatistics : Metronom.RefreshAble, IHtmlOutput
    {
      #region PRIVATE

      private System.Collections.ArrayList myInterfaceList = new System.Collections.ArrayList();

      #endregion PRIVATE

      #region PUBLIC

      /// <summary>
      /// Provides statistic informaton to management level - human mascine interface (HMI) (internal statistics - that are send through remoting)
      /// </summary>
      [Serializable]
      public class StationStatisticsInternal
      {
        internal bool myPriority;
        internal bool myOn = false;

        /// <summary>
        /// name
        /// </summary>
        public readonly string myName;

        /// <summary>
        /// Identifier
        /// </summary>
        public readonly long myID;

        /// <summary>
        /// Gets a value indicating whether priority is set.
        /// </summary>
        /// <value><c>true</c> if priority otherwise, <c>false</c>.</value>
        public bool MyPriority => myPriority;

        /// <summary>
        /// Gets a value indicating whether station is on.
        /// </summary>
        /// <value><c>true</c> if station is on otherwise, <c>false</c>.</value>
        internal bool MyOn => myOn;

        /// <summary>
        /// Gets the state of the station.
        /// </summary>
        /// <value>The state of the station.</value>
        public int StationState
        {
          get
          {
            int state = 0;
            if (myOn)
              state = 1;
            if (MyPriority)
              state += 2;
            return state;
          }
        }

        /// <summary>
        /// Gets the station state as string.
        /// </summary>
        /// <value>The station state as string.</value>
        public string StationStateString
        {
          get
          {
            string state = "";
            if (myOn)
              state += "Active";
            if (myPriority)
              state += "HiPriority";
            if (state == "")
              state = "Not Active";
            return state;
          }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
          return myName + "(" + myID.ToString() + ")";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StationStatisticsInternal"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="ID">The ID.</param>
        public StationStatisticsInternal(string name, long ID)
        {
          myName = name;
          myID = ID;
        }
      };

      /// <summary>
      /// internal statistics <see cref="StationStatisticsInternal"/>
      /// </summary>
      public StationStatisticsInternal myStat;

      /// <summary>
      /// Occurs when [mark new state].
      /// </summary>
      public event StateChanged MarkNewState;

      /// <summary>
      /// Adds the interface.
      /// </summary>
      /// <param name="intr">The interface statistics.</param>
      public void AddInterface(InterfaceStatistics intr)
      {
        myInterfaceList.Add(intr);
      }

      /// <summary>
      /// Gets  name.
      /// </summary>
      /// <value> name.</value>
      public string myName => myStat.myName;

      /// <summary>
      /// Gets a value indicating whether  on.
      /// </summary>
      /// <value><c>true</c> if station is on otherwise, <c>false</c>.</value>
      protected bool myOn => myStat.myOn;

      /// <summary>
      /// Gets or sets a value indicating whether [station state].
      /// </summary>
      /// <value><c>true</c> if [station state]; otherwise, <c>false</c>.</value>
      public bool StationState
      {
        set
        {
          myStat.myOn = value;
          if (MarkNewState != null)
            MarkNewState(value);
        }
        get => myStat.myOn;
      }

      /// <summary>
      /// Gets or sets a value indicating whether this <see cref="StationStatistics"/> is priority.
      /// </summary>
      /// <value><c>true</c> if priority; otherwise, <c>false</c>.</value>
      public bool priority
      {
        set => myStat.myPriority = value;
        get => myStat.myPriority;
      } //Station

      /// <summary>
      /// Initializes a new instance of the <see cref="StationStatistics"/> class.
      /// </summary>
      /// <param name="currDsc">The curr DSC.</param>
      public StationStatistics(CommunicationDSC.StationRow currDsc)
      {
        stationList.Add(this);
        myStat = new StationStatisticsInternal(currDsc.Name, currDsc.StationID);
      }

      #region IHtmlOutput Members

      /// <summary>
      /// variable responsible for row color changing
      /// </summary>
      private static bool changecolor = true;

      /// <summary>
      /// returns row color class
      /// </summary>
      private string RowClass
      {
        get
        {
          changecolor = !changecolor;
          if (changecolor)
            return "k41";
          return "k4";
        }
      }

      /// <summary>
      /// this function is for retrieving description fot this object in the HTML
      /// </summary>
      /// <returns>string with HTML output</returns>
      public string GetHtmlTableRowDescription()
      {
        string ret = "";
        ret += "<tr>";
        ret += "<td class='k3'>" + "Station ID" + "</td>";
        ret += "<td class='k3'>" + "Name" + "</td>";
        ret += "<td class='k3'>" + "On/Off" + "</td>";
        ret += "<td class='k3'>" + "Prioryty" + "</td>";
        ret += "</tr>";
        return ret;
      }

      /// <summary>
      /// this function is for retreiving data that represens that object
      /// </summary>
      /// <returns>string with HTML data</returns>
      public string ToHtmlTableRow()
      {
        string RowClassAct = RowClass;
        string ret = "";
        ret += "<tr>";
        ret += "<td class='" + RowClassAct + "'>&nbsp;" + myStat.myID.ToString() + "</td>";
        ret += "<td class='" + RowClassAct + "'>&nbsp;" + myStat.myName + "</td>";
        ret += "<td class='" + RowClassAct + "'>&nbsp;" + StationState + "</td>";
        ret += "<td class='" + RowClassAct + "'>&nbsp;" + priority + "</td>";
        ret += "</tr>";
        return ret;
      }

      /// <summary>
      /// this function is for retreiving data that represens that object (data + description)
      /// </summary>
      /// <returns>string with HTML data</returns>
      public string ToHtml()
      {
        return "<table border='1' class='t2'>" + GetHtmlTableRowDescription() + ToHtmlTableRow() + "</table>";
      }

      #endregion IHtmlOutput Members

      #endregion PUBLIC
    }//Station

    /// <summary>
    /// this is helper class to add refresh possibility to Protocol class
    /// </summary>
    [Serializable]
    public abstract class ProtocolStatisticsUpdater : Protocol
    {
      private readonly MyStatisticsRefresh MyRefresher;

      /// <summary>
      /// Refreshes this instance.
      /// </summary>
      protected internal abstract void refresh();

      private class MyStatisticsRefresh : Metronom.RefreshAble
      {
        private ProtocolStatisticsUpdater m_parent;

        protected override void refresh()
        {
          m_parent.refresh();
        }

        internal MyStatisticsRefresh(ProtocolStatisticsUpdater parent)
        {
          m_parent = parent;
        }
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ProtocolStatisticsUpdater" /> class.
      /// </summary>
      /// <param name="protocolPar">The protocol parameters.</param>
      /// <param name="Name">The name.</param>
      /// <param name="ID">The ID.</param>
      /// <param name="protocolPar_humanreadable">The protocol human readable.</param>
      /// <param name="settingsBase"> Gets or sets the value of the specified settings property.</param>
      protected ProtocolStatisticsUpdater(string protocolPar, string Name, long ID, string protocolPar_humanreadable, ISettingsBase settingsBase)
        : base(protocolPar, Name, ID, protocolPar_humanreadable, settingsBase)
      {
        MyRefresher = new MyStatisticsRefresh(this);
      }
    }

    /// <summary>
    ///  Title   : Channel statistics management class
    /// </summary>
    [Serializable]
    public class ChannelStatistics
    {
      #region PRIVATE

      private System.Collections.ArrayList mySegmentList = new System.Collections.ArrayList();

      #endregion PRIVATE

      #region PUBLIC

      /// <summary>
      /// Channel statistics management class (internal statistics - that are send through remoting)
      /// </summary>
      [Serializable]
      public class ChannelStatisticsInternal
      {
        private readonly string myName;

        /// <summary>
        /// ID
        /// </summary>
        public readonly long myID;

        /// <summary>
        /// Gets name.
        /// </summary>
        /// <value>name.</value>
        public string MyName => myName;

        private readonly uint[] packetsCount = new uint[] { 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
          return myName + "(" + myID.ToString() + ")";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelStatisticsInternal"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The id.</param>
        internal ChannelStatisticsInternal(string name, long id)
        {
          myName = name;
          myID = id;
        }

        /// <summary>
        /// Gets or sets the <see cref="System.UInt32"/> at the specified index.
        /// </summary>
        /// <value></value>
        public uint this[int index]
        {
          get => packetsCount[index];
          set => packetsCount[index] = value;
        }
      };

      private ChannelStatisticsInternal myStat;

      /// <summary>
      /// Gets name.
      /// </summary>
      /// <value>name.</value>
      public string myName => myStat.MyName;

      /// <summary>
      /// Gets ID.
      /// </summary>
      /// <value>ID.</value>
      public long myID => myStat.myID;

      /// <summary>
      /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
      /// </returns>
      public override string ToString() { return myName; }

      /// <summary>
      /// Gets the protocol statistics.
      /// </summary>
      /// <param name="counters">The counters.</param>
      /// <param name="isAnySuccess">if set to <c>true</c> [is any success].</param>
      public void GeatProtocolStatistics(ref uint[] counters, out bool isAnySuccess)
      {
        lock (this)
        {
          for (ushort idx = 0; idx < counters.Length; idx++)
            counters[idx] = counters[idx] - myStat[idx];
          isAnySuccess =
            (counters[(ushort)RWOperationRes.ORReadGood] > 0) |
            (counters[(ushort)RWOperationRes.ORWriteGood] > 0);
          for (ushort idx = 0; idx < counters.Length; idx++)
            myStat[idx] += counters[idx];
        }
      }//GeatProtocolStatistics

      /// <summary>
      /// Adds the segment.
      /// </summary>
      /// <param name="sgmt">The segment statistics.</param>
      public void AddSegment(SegmentStatistics sgmt)
      {
        mySegmentList.Add(sgmt);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ChannelStatistics"/> class.
      /// </summary>
      /// <param name="currDsc">The channel row descriptor.</param>
      public ChannelStatistics(CommunicationDSC.ChannelsRow currDsc)
      {
        channelList.Add(this);
        myStat = new ChannelStatisticsInternal(currDsc.Name, currDsc.ChannelID);
      }

      #endregion PUBLIC
    }//Channel

    #endregion PUBLIC refresh
  }//Interface
}