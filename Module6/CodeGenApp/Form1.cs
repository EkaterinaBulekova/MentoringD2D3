using System;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace CodeGenApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ev_a(object A_0, EventArgs A_1)
        {
            resultTextbox.Text = GetExpectation();
        }

        private string GetExpectation()
        {
            NetworkInterface netInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
            byte[] addressBytes = netInterface.GetPhysicalAddress().GetAddressBytes();
            byte[] dateBytes = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());
            int[] result = addressBytes.Select((byte adr, int idx) => adr ^ dateBytes[idx]).Select((int num)=>(num <= 999) ? num * 10 : num).ToArray();
            return string.Join("-", result.Select(num => num.ToString()));
        }
    }
}

