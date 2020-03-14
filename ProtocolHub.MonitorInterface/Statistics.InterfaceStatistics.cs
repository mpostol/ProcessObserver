//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.RTLib.Management;
using System;

namespace CAS.CommServer.ProtocolHub.MonitorInterface
{
  /// <summary>
  /// Communication statistics management class
  /// </summary>
  public partial class Statistics
  {
    /// <summary>
    /// Provides statistic information to management level - human machine interface (HMI)
    /// </summary>
    [Serializable]
    public abstract class InterfaceStatistics : Metronom.RefreshAble, IHtmlOutput, IUpdateInternalStatistics
    {

      #region PRIVATE
      private IInterface2SegmentLink mySegment;
      private StationStatistics myStation;
      private UAOOI.ProcessObserver.RealTime.Processes.Stopwatch failTime = new UAOOI.ProcessObserver.RealTime.Processes.Stopwatch();
      private UAOOI.ProcessObserver.RealTime.Processes.Stopwatch activeTime = new UAOOI.ProcessObserver.RealTime.Processes.Stopwatch();
      private UAOOI.ProcessObserver.RealTime.Processes.Stopwatch standbyTime = new UAOOI.ProcessObserver.RealTime.Processes.Stopwatch();

      private void GeatProtocolStatistics(out bool isAnySuccess)
      {
        mySegment.GetProtocolStatistics(ref myStat.currCount, out isAnySuccess);
        for (ushort idx = 0; idx < myStat.currCount.Length; idx++)
          myStat.packetsCount[idx] += myStat.currCount[idx];
      }//GeatProtocolStatistics
      #endregion

      #region PUBLIC
      /// <summary>
      /// provides statistic information to management level - human machine interface (HMI)(internal statistics - that are send through remoting)
      /// </summary>
      [Serializable]
      public class InterfaceStatisticsInternal
      {

        #region private
        private static ulong InternalIDCounter = 0;
        /// <summary>
        /// Interface statistics
        /// </summary>
        protected InterfaceState myState = InterfaceState.Standby;
        private ulong activeTime = 0, failTime = 0, standbyTime = 0;
        internal uint[] packetsCount = new uint[] { 0, 0, 0, 0, 0, 0 };
        internal uint[] currCount = new uint[] { 0, 0, 0, 0, 0, 0 };
        #endregion

        #region public
        /// <summary>
        /// Name
        /// </summary>
        public readonly string myName;
        /// <summary>
        /// ID InterfaceNum
        /// </summary>
        public readonly ulong myID_InterfaceNum;
        /// <summary>
        /// ID Internal
        /// </summary>
        public readonly ulong myID_Internal;
        #endregion

        #region public method
        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceStatisticsInternal"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The id.</param>
        internal InterfaceStatisticsInternal(string name, ulong id)
        {
          myName = name;
          myID_InterfaceNum = id;
          myID_Internal = InternalIDCounter++;
        }
        /// <summary>
        /// Gets the get state as  string.
        /// </summary>
        /// <value>The get state as string.</value>
        public string GetState2String => myState.ToString();//CurrentInterfaceState
        /// <summary>
        /// Gets a value indicating whether this <see cref="InterfaceStatisticsInternal"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active => (myState == InterfaceState.Active);
        /// <summary>
        /// Gets  state.
        /// </summary>
        /// <value> state.</value>
        public InterfaceState MyState => myState;
        /// <summary>
        /// Gets the active time.
        /// </summary>
        /// <value>The active time.</value>
        public ulong ActiveTime => activeTime;
        /// <summary>
        /// Gets the fail time.
        /// </summary>
        /// <value>The fail time.</value>
        public ulong FailTime => failTime;
        /// <summary>
        /// Gets the standby time.
        /// </summary>
        /// <value>The standby time.</value>
        public ulong StandbyTime => standbyTime;
        /// <summary>
        /// Gets the state .
        /// </summary>
        /// <value>The state .</value>
        public InterfaceState State
        {
          get => myState;
          internal set => myState = value;
        }
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
          return myName + "(" + myID_InterfaceNum.ToString() + ")";
        }
        internal void UpdateTimers(ulong ActiveTime, ulong FailTime, ulong StandbyTime)
        {
          activeTime = ActiveTime;
          failTime = FailTime;
          standbyTime = StandbyTime;
        }
        #endregion public method

      };
      /// <summary>
      /// access to statistic internal
      /// </summary>
      public InterfaceStatisticsInternal myStat;
      /// <summary>
      /// Gets  name.
      /// </summary>
      /// <value>name.</value>
      private string myName => myStat.myName;
      /// <summary>
      /// Gets my ID interface num.
      /// </summary>
      /// <value>My ID interface num.</value>
      public ulong myID_InterfaceNum => myStat.myID_InterfaceNum;
      /// <summary>
      /// states of the interfaces
      /// </summary>
      public enum InterfaceState
      {
        /// <summary>
        /// Active
        /// </summary>
        Active,
        /// <summary>
        /// Fail
        /// </summary>
        Fail,
        /// <summary>
        /// Standby
        /// </summary>
        Standby
      }

      #region HMI
      /// <summary>
      /// delegate  that is used when interface state is changed
      /// </summary>
      public delegate void InterfaceStateChanged(InterfaceState currState);
      /// <summary>
      /// Occurs when [update interface state].
      /// </summary>
      public event InterfaceStateChanged UpdateInterfaceState;
      /// <summary>
      /// Gets the active time.
      /// </summary>
      /// <value>The active time.</value>
      public ulong ActiveTime
      {
        get
        {
          ((IUpdateInternalStatistics)this).UpdateInternal();
          return myStat.ActiveTime;
        }
      }
      /// <summary>
      /// Gets the fail time.
      /// </summary>
      /// <value>The fail time.</value>
      public ulong FailTime
      {
        get
        {
          ((IUpdateInternalStatistics)this).UpdateInternal();
          return myStat.FailTime;
        }
      }
      /// <summary>
      /// Gets the standby time.
      /// </summary>
      /// <value>The standby time.</value>
      public ulong StandbyTime
      {
        get
        {
          ((IUpdateInternalStatistics)this).UpdateInternal();
          return myStat.StandbyTime;
        }
      }
      /// <summary>
      /// Gets the state .
      /// </summary>
      /// <value>The state .</value>
      public InterfaceState GetState => myStat.MyState;
      /// <summary>
      /// Gets the get state as string.
      /// </summary>
      /// <value>The get state as string.</value>
      public string GetState2String => myStat.GetState2String;//CurrentInterfaceState
      /// <summary>
      /// Gets a value indicating whether this <see cref="InterfaceStatistics"/> is active.
      /// </summary>
      /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
      public bool Active => myStat.Active;
      /// <summary>
      /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
      /// </returns>
      public override string ToString()
      {
        return myName + "_" + myID_InterfaceNum.ToString();
      }
      /// <summary>
      /// Gets the NAME.
      /// </summary>
      /// <value>The NAME.</value>
      public string NAME => myName;
      #endregion

      #region PARENT
      /// <summary>
      /// Marks the ReadWrite operation result.
      /// </summary>
      public void MarkRWOperationResult()
      {
        //        MPTD zaiplementowaæ - tu jest circular reference
        //        lock(this)
        //        {
        //          bool isAnySuccess;
        //          GeatProtocolStatistics(out isAnySuccess);
        ////          if ( (myState == InterfaceState.Fail) && isAnySuccess )
        ////          {
        ////            ulong voidV = activeTime.Start;
        ////            voidV       = failTime.Stop;
        ////            myState     = InterfaceState.Active;
        ////          }
        //        } 
      }
      /// <summary>
      /// Sets the state of the current interface.
      /// </summary>
      /// <value>The state of the current interface.</value>
      public InterfaceState CurrentInterfaceStateUpdate
      {
        set
        {
          lock (this)
          {
            ulong voidV;
            switch (value)
            {
              case InterfaceState.Active:
                //if (myState == InterfaceState.Standby )
                voidV = standbyTime.Stop;
                voidV = failTime.Stop;
                voidV = activeTime.Start;
                break;
              case InterfaceState.Fail:
                System.Diagnostics.Debug.Assert(myStat.State == InterfaceState.Active);
                voidV = activeTime.Stop;
                voidV = failTime.Start;
                break;
              case InterfaceState.Standby:
                System.Diagnostics.Debug.Assert(myStat.State == InterfaceState.Active);
                voidV = activeTime.Stop;
                voidV = standbyTime.Start;
                myStat.State = InterfaceState.Standby;
                break;
            }
            myStat.State = value;
          }
        }
      }//CurrentInterfaceState

      /// <summary>
      /// Sets the state of the current interface.
      /// </summary>
      /// <value>The state of the current interface.</value>
      public InterfaceState CurrentInterfaceState
      {
        set
        {
          CurrentInterfaceStateUpdate = value;
          lock (this)
          {
            switch (value)
            {
              case InterfaceState.Active:
                //if (myState == InterfaceState.Standby )
                myStation.StationState = true;
                break;
              case InterfaceState.Fail:
                myStation.StationState = false;
                break;
              case InterfaceState.Standby:
                myStation.StationState = false;
                break;
            }
            if (UpdateInterfaceState != null)
              UpdateInterfaceState(value);
          }
        }
      }//CurrentInterfaceState
      /// <summary>
      /// Initializes a new instance of the <see cref="InterfaceStatistics"/> class.
      /// </summary>
      /// <param name="name">The name.</param>
      /// <param name="interfaceNumber">The interface number.</param>
      /// <param name="seg">The seg.</param>
      /// <param name="stt">The STT.</param>
      public InterfaceStatistics(string name, ulong interfaceNumber, IInterface2SegmentLink seg, StationStatistics stt)
      {
        mySegment = seg;
        myStation = stt;
        interfaceList.Add(this);
        stt.AddInterface(this);
        seg.AddInterface(this);
        myStat = new InterfaceStatisticsInternal(name, interfaceNumber);
      }
      #endregion

      #endregion

      #region IHtmlOutput Members
      /// <summary>
      /// variable resposible for row color changing
      /// </summary>
      private static bool changeColor = true;
      /// <summary>
      /// returns row color class
      /// </summary>
      private string RowClass
      {
        get
        {
          changeColor = !changeColor;
          if (changeColor)
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
        ret += "<td class='k3' >" + "Name" + "</td>";
        ret += "<td class='k3' >" + "State" + "</td>";
        ret += "<td class='k3' >" + "Active Time" + "</td>";
        ret += "<td class='k3' >" + "Fail Time" + "</td>";
        ret += "<td class='k3' >" + "Standby Time" + "</td>";
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
        ret += "<td class='" + RowClassAct + "'>&nbsp;" + ToString() + "</td>";
        ret += "<td class='" + RowClassAct + "'>&nbsp;" + GetState + "</td>";
        ret += "<td class='" + RowClassAct + "'>&nbsp;" + ActiveTime.ToString() + "</td>";
        ret += "<td class='" + RowClassAct + "'>&nbsp;" + FailTime.ToString() + "</td>";
        ret += "<td class='" + RowClassAct + "'>&nbsp;" + StandbyTime.ToString() + "</td>";
        ret += "</tr>";
        return ret;
      }
      /// <summary>
      /// this function is for retrieving data that represents that object (data + description)
      /// </summary>
      /// <returns>string with HTML data</returns>
      public string ToHtml()
      {
        return "<table border='1' class='t2'>" + GetHtmlTableRowDescription() + ToHtmlTableRow() + "</table>";
      }
      #endregion

      #region IUpdateInternalStatistics Members

      void IUpdateInternalStatistics.UpdateInternal()
      {
        myStat.UpdateTimers(UAOOI.ProcessObserver.RealTime.Processes.Stopwatch.ConvertTo_s(activeTime.Read),
          UAOOI.ProcessObserver.RealTime.Processes.Stopwatch.ConvertTo_s(failTime.Read),
          UAOOI.ProcessObserver.RealTime.Processes.Stopwatch.ConvertTo_s(standbyTime.Read));
      }

      #endregion

    }
  }
}