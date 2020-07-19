using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;


namespace ProsxyScanner
{
    public partial class Main : Form
    {

        public Main()
        {
            InitializeComponent();
            // Declaration
            menuStrip1.ForeColor = Color.DimGray;

        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Stop.Enabled = false;
            
        }

        private bool _stop;

        private async void Start_Click(object sender, EventArgs e)
        {
            //disable button and boxes plsu stop loop
            _stop = false;
            this.dec1.Enabled = false;
            this.dec2.Enabled = false;
            this.dec3.Enabled = false;
            this.dec4.Enabled = false;
            this.port.Enabled = false;
            this.Interval.Enabled = false;
            this.Sockets.Enabled = false;
            this.Start.Enabled = false;
            this.Stop.Enabled = true;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 65025;
            progressBar1.Step = 1;
            // end disable 

            //ip stuff 
            string ip = this.dec1.Text.ToString() + "." + this.dec2.Text.ToString() + "." + this.dec3.Text.ToString() + "." + this.dec4.Text.ToString();
            int port = int.Parse(this.port.Text);
            int sockets = int.Parse(Sockets.Text);
            int time = int.Parse(Interval.Text);
            int decimal3 = int.Parse(dec3.Text);
            int decimal4 = int.Parse(dec4.Text);

            //load the amount of sockets to use 
 
            Socket[] connection = new Socket[sockets];

            //while loop scaning 
            Restart:
            try
            {
                
                while (decimal3 < 255)
                {

                    //for loop to control section 
                    for (int i = 0; decimal4 < 256; i++)
                    {

                        for (int j = 0; j < sockets && !_stop; j++)
                        {

                            if (decimal4 < 255)
                            { //create the socket your going to use in the array
                                connection[j] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                //connect with the socket you made to the port and ip with a wait time for the connection
                                connection[j].ConnectAsync(ip, port).Wait(time);
                                //if the connection is connected add and close
                                await Task.Delay(time);
                                if (connection[j].Connected)
                                {
                                    Found.Items.Add(ip + ":" + port);
                                    label4.Text = Found.Items.Count.ToString();
                                    connection[j].Close();
                                    progressBar1.PerformStep();
                                    decimal4++;
                                    this.dec4.Text = decimal4.ToString();
                                    ip = this.dec1.Text.ToString() + "." + this.dec2.Text.ToString() + "." + decimal3.ToString() + "." + decimal4.ToString();
                                }
                                else if (!connection[j].Connected)
                                {
                                    //increment and then set the ip to new value
                                    progressBar1.PerformStep();
                                    decimal4++;
                                    this.dec4.Text = decimal4.ToString();
                                    ip = this.dec1.Text.ToString() + "." + this.dec2.Text.ToString() + "." + decimal3.ToString() + "." + decimal4.ToString();

                                    //close the connection afterwords 

                                    connection[j].Close();
                                }
                            }
                            //if last decimal is 255 reset it to zero add it to text box then increment the 3rd decimal
                            if (decimal4 == 255)
                            {
                                decimal4 = 0;
                                this.dec4.Text = decimal4.ToString();
                                decimal3++;
                                dec3.Text = decimal3.ToString();

                            }

                        }
                        await Task.Delay(1000);
                    }

                    if (decimal3 == 255 & decimal4 == 255)
                    {
                        MessageBox.Show("Done");
                        break;

                    }
                }
            }

            catch
            {
                decimal4++;
                this.dec4.Text = decimal4.ToString();
                ip = this.dec1.Text.ToString() + "." + this.dec2.Text.ToString() + "." + decimal3.ToString() + "." + decimal4.ToString();
                goto Restart;
            }


        }
   



        private void Stop_Click(object sender, EventArgs e)
        {
            //enable all boxes and buttons disabled by start button
            this.dec1.Enabled = true;
            this.dec2.Enabled = true;
            this.dec3.Enabled = true;
            this.dec4.Enabled = true;
            this.port.Enabled = true;
            this.Interval.Enabled = true;
            this.Sockets.Enabled = true;
            this.Start.Enabled = true;
            this.Stop.Enabled = false;
            _stop = true;


            //end section 

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Found.Items.Clear();
            label4.Text = "0";
        }
    }

}
