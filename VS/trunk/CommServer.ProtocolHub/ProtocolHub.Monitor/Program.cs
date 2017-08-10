using System;
using System.Text;
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
