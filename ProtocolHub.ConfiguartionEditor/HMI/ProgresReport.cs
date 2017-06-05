using System.Windows.Forms;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.HMI
{
  internal partial class ProgresReport: Form
  {
    internal ProgresReport()
    {
      InitializeComponent();
    }
    internal ProgressBar progresBar
    {
      get
      {
        return progressBar1;
      }
    }
    internal string LabelText
    {
      get
      {
        return label1.Text;
      }
      set
      {
        label1.Text = value;
      }
    }


  }
}