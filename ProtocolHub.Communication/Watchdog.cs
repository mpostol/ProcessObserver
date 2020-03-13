//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.CommServer.ProtocolHub.Communication.Properties;
using System;
using System.Diagnostics;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using UAOOI.ProcessObserver.RealTime.Processes;

namespace CAS.Lib.RTLib.Processes
{
  /// <summary>
  /// Watchdog implementation - To apply deadline the class method call add this attribute 
  /// and inherit from <see cref=" ContextBoundObject"/>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public class WatchdogAttribute : Attribute, IContextAttribute
  {

    #region private
    private string m_ObjectName;
    private readonly TimeSpan m_deadline;
    private string GetName(IConstructionCallMessage ctorMsg, string defaultName)
    {
      string id = "";
      if (ctorMsg.ArgCount > 0)
        for (int ix = 0; ix < ctorMsg.ArgCount; ix++)
          if (ctorMsg.GetInArg(ix) is string)
            id += (string)ctorMsg.GetInArg(ix) + " ";
      if (id.Length < 2)
        id = defaultName;
      return id;
    }
    #endregion

    #region ContextAttribute
    /// <summary>
    /// Returns a Boolean value indicating whether the context parameter meets the context attribute's requirements.
    /// </summary>
    /// <param name="ctx">The context in which to check.</param>
    /// <param name="ctorMsg">The <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage"></see> to which to add the context property.</param>
    /// <returns>
    /// true if the passed in context is okay; otherwise, false.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">Either ctx or ctorMsg is null. </exception>
    /// <PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/></PermissionSet>
    [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    public bool IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
    {
      // Force new context.
      return false;
    }
    /// <summary>
    /// Adds the current context property to the given message.
    /// </summary>
    /// <param name="ctorMsg">The <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage"></see>
    ///  to which to add the context property.
    /// </param>
    /// <exception cref="T:System.ArgumentNullException">The ctorMsg parameter is null. </exception>
    /// <PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/>
    /// </PermissionSet>
    [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    public void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
    {
      m_ObjectName = GetName(ctorMsg, m_ObjectName);
      ctorMsg.ContextProperties.Add(new WatchdogProperty(m_ObjectName, m_deadline));
    }
    #endregion

    #region constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="WatchdogAttribute"/> class.
    /// </summary>
    public WatchdogAttribute() : this("WatchdogAttribute", 5 * 60) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="WatchdogAttribute"/> class.
    /// </summary>
    /// <param name="objectName">Name of the object.</param>
    /// <param name="deadlineInS">The deadline in S.</param>
    public WatchdogAttribute(string objectName, int deadlineInS)
    {
      m_ObjectName = objectName;
      m_deadline = new TimeSpan(0, 0, deadlineInS);
    }
    #endregion

  }
  /// <summary>
  /// Implementation of the interface for a message sink.
  /// </summary>
  public class WatchdogSink : IMessageSink
  {

    #region private
    private IMessageSink m_Next;
    private WatchdogProperty m_Property; //obiekt na rzecz kt°rego nale¨y wywo-aä metod‡
    #endregion

    #region IMessageSink Members
    /// <summary>
    /// Asynchronously processes the given message.
    /// </summary>
    /// <param name="msg">The message to process.</param>
    /// <param name="replySink">The reply sink for the reply message.</param>
    /// <returns>
    /// Returns an <see cref="T:System.Runtime.Remoting.Messaging.IMessageCtrl"></see> interface that provides a way to control asynchronous messages after they have been dispatched.
    /// </returns>
    /// <exception cref="T:System.Security.SecurityException">The immediate caller makes the call through a reference to the interface and does not have infrastructure permission. </exception>
    [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    IMessageCtrl IMessageSink.AsyncProcessMessage(IMessage msg, IMessageSink replySink)
    {
      return m_Next.AsyncProcessMessage(msg, replySink);
    }
    /// <summary>
    /// Gets the next message sink in the sink chain.
    /// </summary>
    /// <value></value>
    /// <returns>The next message sink in the sink chain.</returns>
    /// <exception cref="T:System.Security.SecurityException">The immediate caller makes the call through a reference to the interface and does not have infrastructure permission. </exception>
    /// <PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/></PermissionSet>
    //[SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    IMessageSink IMessageSink.NextSink { get { return m_Next; } }
    /// <summary>
    /// Synchronously processes the given message.
    /// </summary>
    /// <param name="msg">The message to process.</param>
    /// <returns>
    /// A reply message in response to the request.
    /// </returns>
    /// <exception cref="T:System.Security.SecurityException">The immediate caller makes the call through a reference to the interface and does not have infrastructure permission. </exception>
    [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    IMessage IMessageSink.SyncProcessMessage(IMessage msg)
    {
      IMethodMessage methodMessage = (IMethodMessage)msg;
      m_Property.EnterWatchdog(new MethodCall(msg));
      IMessage retMsg = m_Next.SyncProcessMessage(msg);
      m_Property.ExitWatchdog();
      return retMsg;
    }
    #endregion

    #region constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="WatchdogSink"/> class.
    /// </summary>
    /// <param name="ims">The message sink.</param>
    /// <param name="property">The property <see cref="WatchdogProperty"/> owner of this object.</param>
    internal WatchdogSink(IMessageSink ims, WatchdogProperty property)
    {
      m_Next = ims;
      m_Property = property;
    }
    #endregion

  }
  /// <summary>
  /// Watchdog property to be added to the guarded by the Watchdog object
  /// </summary>
  public class WatchdogProperty : IContextProperty, IContributeObjectSink
  {
    #region private
    private string MethodCallMessageToString(IMethodCallMessage msg)
    {
      if (msg != null)
      {
        StringBuilder mDescription = new StringBuilder();
        if (msg.MethodName != null)
        {
          mDescription.AppendFormat("Method = {0}(", msg.MethodName);
        }
        for (int ix = 0; ix < msg.ArgCount; ix++)
        {
          mDescription.AppendFormat(" {0}=", msg.GetArgName(ix));
          string val = "null";
          if (msg.GetArg(ix) != null)
            val = msg.GetArg(ix).ToString();
          mDescription.AppendFormat(" {0} ;", val);
        }
        mDescription.Append(")");
        return mDescription.ToString();
      }
      else
        return "Null message";
    }
    private const string PropName = "WatchdogProperty";
    private string m_ObjectName;
    private readonly TimeSpan m_deadline;
    private TimeSpan m_maxDelay = new TimeSpan();
    private IMethodCallMessage m_lastCall;
    private System.Diagnostics.Stopwatch stopwatch;
    private System.Threading.Timer timer;
    private int InsideWatchdogCoutner = 0;
    private void MaxDelayMessage(string SourceName)
    {
      if (stopwatch.Elapsed > m_maxDelay)
      {
        m_maxDelay = stopwatch.Elapsed;
        string message = String.Format(Resources.MaxDelayMessageFormat, m_maxDelay.TotalMilliseconds.ToString(), m_ObjectName, SourceName + ":" + MethodCallMessageToString(m_lastCall));
        AssemblyTraceEvent.Trace(TraceEventType.Information, 267, "WatchdogProperty", message);
      }

    }
    private void Pendulum(object state)
    {
      if (!Monitor.TryEnter(this)) // instead of lock (this)
        return; // we are exiting, because the lock is already acquired (this is not the problem because it will be soon launched again by the Timer)
      try
      {
        if (stopwatch.Elapsed > m_deadline)
        {
          MaxDelayMessage("Pendulum");
          stopwatch.Reset();
          string message = String.Format(Resources.RestartMessageFormat, m_ObjectName, MethodCallMessageToString(m_lastCall));
          AssemblyTraceEvent.Trace(TraceEventType.Critical, 162, "WatchdogProperty", message);
#if DEBUG
          MarkRestart(message);
          //NUnit.Framework.Assert.Fail
          //( "I am about to reboot the system, but reboot is now switched off because of debug mode", "Processes.MonitoredThread" );
#else
          CAS.Lib.RTLib.Processes.Manager.ForceReboot();
#endif
        }
      }
      finally
      {
        Monitor.Exit(this);
      }
    }
    #endregion

    #region IContributeObjectSink Members
    /// <summary>
    /// Chains the message sink of the provided server object in front of the given sink chain.
    /// </summary>
    /// <param name="obj">The server object which provides the message sink that is to be chained in front of the given chain.</param>
    /// <param name="nextSink">The chain of sinks composed so far.</param>
    /// <returns>The composite sink chain.</returns>
    [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    IMessageSink IContributeObjectSink.GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
    {
      return new WatchdogSink(nextSink, this);
    }
    #endregion
    
    #region IContextProperty Members
    /// <summary>
    /// Called when the context is frozen.
    /// </summary>
    /// <param name="newContext">The context to freeze.</param>
    /// <PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/></PermissionSet>
    [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    void IContextProperty.Freeze(Context newContext) { }
    /// <summary>
    /// Returns a Boolean value indicating whether the context property is compatible with the new context.
    /// </summary>
    /// <param name="newCtx">The new context in which the <see cref="T:System.Runtime.Remoting.Contexts.ContextProperty"></see> has been created.</param>
    /// <returns>
    /// true if the context property can coexist with the other context properties in the given context; otherwise, false.
    /// </returns>
    [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    bool IContextProperty.IsNewContextOK(Context newCtx)
    {
      // We could also inspect the other properties for the context to make sure none conflict,
      // but for this, we just find out if the new context has a WatchdogProperty property. If not, reject it
      WatchdogProperty newContextLogProperty = newCtx.GetProperty(PropName) as WatchdogProperty;
      if (newContextLogProperty == null)
      {
        Debug.Assert(false);
        return false;
      }
      return true;
    }
    /// <summary>
    /// Gets the name of the property under which it will be added to the context.
    /// </summary>
    /// <value></value>
    /// <returns>The name of the property.</returns>
    /// <PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/></PermissionSet>
    //[SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    string IContextProperty.Name
    {
      get
      {
        return (PropName);
      }
    }
    #endregion
    
    #region public
    /// <summary>
    /// Enters the guarded by the watchdog object.
    /// </summary>
    /// <param name="lastCall">The call message.</param>
    public void EnterWatchdog(IMethodCallMessage lastCall)
    {
      lock (this)
      {
        if (stopwatch.Elapsed > TimeSpan.Zero)
        {
          string message = String.Format(Resources.WatchdogStopwatchIsNotZeroMessageFormat, stopwatch.Elapsed.Milliseconds);
          AssemblyTraceEvent.Trace(TraceEventType.Warning, 286, "WatchdogProperty", message);
        }
        if (InsideWatchdogCoutner != 0)
        {
          string message = String.Format(Resources.InsideWatchdogMessageFormat, InsideWatchdogCoutner);
          AssemblyTraceEvent.Trace(TraceEventType.Warning, 291, "WatchdogProperty", message);
        }
#if DEBUG
        string meth = MethodCallMessageToString(lastCall);
#endif
        m_lastCall = lastCall;
        stopwatch.Start();
        InsideWatchdogCoutner++;
      }
    }
    /// <summary>
    /// Exits the guarded by the watchdog object.
    /// </summary>
    public void ExitWatchdog()
    {
      lock (this)
      {
        MaxDelayMessage("ExitWatchdog");
        stopwatch.Reset();
        InsideWatchdogCoutner--;
      }
    }
    #endregion

    #region constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="WatchdogProperty"/> class.
    /// </summary>
    /// <param name="objectName">Name of the object.</param>
    /// <param name="deadline">The deadline.</param>
    public WatchdogProperty(string objectName, TimeSpan deadline)
    {
      stopwatch = new System.Diagnostics.Stopwatch();
      stopwatch.Reset();
      timer = new System.Threading.Timer(new TimerCallback(Pendulum), null, 0, 1000);
      m_ObjectName = objectName;
      m_deadline = deadline;
    }
    #endregion

    #if DEBUG
    /// <summary>
    /// counts the number of times when restart should occurs
    /// </summary>
    public static byte count = 0;
    /// <summary>
    /// Marks the restart.
    /// </summary>
    [Conditional("DEBUG")]
    private static void MarkRestart(string message)
    {
      Console.WriteLine(message);
      count++;
    }
#endif
  }

}