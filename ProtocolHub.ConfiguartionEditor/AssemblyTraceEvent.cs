//_______________________________________________________________
//  Title   : AssemblyTraceEvent
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate: 2017-05-30 23:05:02 +0200 (Tue, 30 May 2017) $
//  $Rev: 13042 $
//  $LastChangedBy: mpostol $
//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/CommServer.DataProvider/DataProvider.TextReader/AssemblyTraceEvent.cs $
//  $Id: AssemblyTraceEvent.cs 13042 2017-05-30 21:05:02Z mpostol $
//
//  Copyright (C) 2017, CAS LODZ POLAND.
//  TEL: +48 608 61 98 99 
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________


using CAS.CommServer.ProtocolHub.ConfigurationEditor.Properties;
using System;
using System.Diagnostics;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor
{

  /// <summary>
  /// Singleton implementation of the <see cref="TraceSource"/>.
  /// </summary>
  public class AssemblyTraceEvent : ITraceSource
  {

    private static AssemblyTraceEvent m_singleton = new AssemblyTraceEvent();
    private Lazy<TraceSource> m_TraceEventInternal = new Lazy<TraceSource>(() => new TraceSource(Settings.Default.TraceSourceName));
    private AssemblyTraceEvent() { }

    /// <summary>
    /// Gets the tracer.
    /// </summary>
    /// <value>The tracer.</value>
    public static AssemblyTraceEvent Tracer
    {
      get
      {
        return m_singleton;
      }
    }
    internal string Name { get { return m_TraceEventInternal.Value.Name; } }
    /// <summary>
    /// a trace event message to the trace listeners in the System.Diagnostics.TraceSource.Listeners collection using the specified event type and event identifier.
    /// </summary>
    /// <param name="eventType">One of the enumeration values that specifies the event type of the trace data.</param>
    /// <param name="id">A numeric identifier for the event.</param>
    /// <param name="message">The trace message to write.</param>
    public void TraceMessage(TraceEventType eventType, int id, string message)
    {
      m_TraceEventInternal.Value.TraceEvent(eventType, id, message);
    }
    /// <summary>
    /// Gets the wrapped instance of <see cref="TraceSource"/>, which provides a set of methods and properties that enable applications to trace the 
    /// execution of code and associate trace messages with their source.
    /// </summary>
    /// <value>the wrapped instance of <see cref="TraceSource"/>.</value>
    internal TraceSource TraceSource { get { return m_TraceEventInternal.Value; } }

  }
  /// <summary>
  /// Interface ITraceSource - declares basic functionality for the component behavior tracing.
  /// </summary>
  public interface ITraceSource
  {
    /// <summary>
    /// a trace event message to the trace listeners in the System.Diagnostics.TraceSource.Listeners collection using the specified event type and event identifier.
    /// </summary>
    /// <param name="eventType">One of the enumeration values that specifies the event type of the trace data.</param>
    /// <param name="id">A numeric identifier for the event.</param>
    /// <param name="message"> The trace message to write.</param>
    /// <exception cref="T:System.ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
    void TraceMessage(TraceEventType eventType, int id, string message);

  }

}
