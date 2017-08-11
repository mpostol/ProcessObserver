//_______________________________________________________________
//  Title   : CommServerConsole
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

using System;
using System.Windows.Forms;

namespace CAS.CommServerConsole
{
  class Program
  {
    [MTAThread]
    public static void Main()
    {
      try
      {
        Application.Run( new MainForm() );
      }
      catch (Exception _ex)
      {
        string _message = $"{Properties.Resources.Tx_InitCommError}, An exception has been thrown: {_ex.Message}";
        MessageBox.Show(_message, Properties.Resources.Tx_InitFailedCap, MessageBoxButtons.OK, MessageBoxIcon.Stop );
      }
    }
  }
}
