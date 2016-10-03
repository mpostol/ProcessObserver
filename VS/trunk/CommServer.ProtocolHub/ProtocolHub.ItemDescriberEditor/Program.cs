//_______________________________________________________________
//  Title   : Program
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate:  $
//  $Rev: $
//  $LastChangedBy: $
//  $URL: $
//  $Id:  $
//
//  Copyright (C) 2016, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System;
using System.Windows.Forms;

namespace CAS.CommServer.DA.ItemDescriberEditor
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      DoApplicationRun( Application.Run);
    }
    /// <summary>
    /// Does the application run.
    /// </summary>
    /// <remarks>Added for the purpose of unit testing</remarks>
    /// <param name="applicationRunAction">The application run action.</param>
    internal static void DoApplicationRun( Action<Form> applicationRunAction)
    {
      applicationRunAction(new MainFormItemDescriber());
    }
  }
}
