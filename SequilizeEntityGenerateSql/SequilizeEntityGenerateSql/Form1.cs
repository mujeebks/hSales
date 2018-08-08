using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SequilizeEntityGenerateSql
{

    public partial class Form1 : Form
    {
        Process Process1 = new Process();
        private string ProcessName;
        public Form1()
        {

            InitializeComponent();
            txtdbpath.Text = "192.168.10.106";
            txtdbname.Text = "HydrowareV2";
            txtentitylocation.Text = "C:\\models";
            txtusername.Text = "sa";
            txtpassword.Text = "abc123*";
            txtport.Text = "8016";
            txtservertype.Text = "mssql";
            radioButton1.Checked = true;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
          
        }

      
        public void ExecuteCommand(string Command)
        {
            Process1.StartInfo = new ProcessStartInfo();
         
            Process1.StartInfo.UseShellExecute = true;

            Process1.StartInfo.WorkingDirectory = @"C:\Windows\System32";

            Process1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
           // Process1.StartInfo.Verb = "runas";
            Process1.StartInfo.Arguments = "/c " + Command;
            Process1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            Process1.EnableRaisingEvents = true;
            Process1.Start();
            Process1.Exited += new EventHandler(myProcess_Exited);
          
        }

        private void myProcess_Exited(object sender, EventArgs e)
        {
            var a = Process1.ExitTime;
            var b = Process1.ExitCode;

            if (Process1.HasExited)
            {
                
                //richTextBox1.AppendText("Process "+ (Process1.Id.ToString())+" Started at " + Process1.StartTime.ToString());
                //richTextBox1.AppendText("Process  " + (Process1.Id.ToString()) + " Ended at" + Process1.ExitTime.ToString());
                //richTextBox1.AppendText("With Exit code " + Process1.ExitCode.ToString());
                if(ProcessName == "auto-generate")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Entities Created Successfully for " + txtdbname.Text);
                    sb.AppendLine("Process " + (Process1.Id.ToString()) + " Started at " + Process1.StartTime.ToString());
                    sb.Append("Process  " + (Process1.Id.ToString()) + " Ended at" + Process1.ExitTime.ToString());
                    sb.AppendLine(" With Exit code " + Process1.ExitCode.ToString());
                    MessageBox.Show(sb.ToString(), "Successfully Created");
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                  
                    sb.AppendLine("Process " + (Process1.Id.ToString()) + " Started at " + Process1.StartTime.ToString());
                    sb.Append("Process  " + (Process1.Id.ToString()) + " Ended at" + Process1.ExitTime.ToString());
                    sb.AppendLine(" With Exit code " + Process1.ExitCode.ToString());
                    MessageBox.Show(sb.ToString(), "Installed Successfully");
                }

             
             

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProcessName = "npm install -g sequelize-auto";
            ExecuteCommand(ProcessName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProcessName = "auto-generate";
            ExecuteCommand("sequelize-auto -o "+ txtentitylocation.Text + " -d "+ txtdbname.Text + " -h "+ txtdbpath.Text + " -u sa -p "+ txtport.Text + " -x "+ txtpassword.Text + " -e "+ txtservertype.Text + " -z");
            // ExecuteCommand("sequelize-auto -o 'C:\\models' -d HydrowareV2 -h 192.168.10.106 -u sa -p 8016 -x abc123* -e mssql -z");
        }
    }
}
