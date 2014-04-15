using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Adage.VSGrunt
{
    public delegate void NodeDelegate(NodeResponse response);

    public class NodeCommand
    {
        public string Path;
        public string AllOutput = String.Empty;

        public Process Process;
        public NodeDelegate Delegate;

        public string Arguments = String.Empty;
        public string Module = String.Empty;
        public bool IsNPM = true;

        public CommandStatus Status = CommandStatus.Initialized;

        public string ExecutablePath
        {
            get
            {
                if (IsNPM)
                {
                    return String.Format("{0}\\npm\\{1}.cmd", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Module);
                }
                else
                {
                    return Module;
                }
            }
        }

        public void Start()
        {
            CreateProcess();

            Process.EnableRaisingEvents = true;

            Process.OutputDataReceived += OnData;
            Process.ErrorDataReceived += OnError;
            Process.Exited += OnExit;

            Process.Start();
            
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();

        }

        private void CreateProcess()
        {
            Debug.WriteLine(String.Format("NodeCommand \n\t Module: {0} \n\t Arguments: {1}", Module, Arguments));

            Process = new System.Diagnostics.Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = Path,
                    UseShellExecute = false,
                    FileName = ExecutablePath,
                    //Arguments = String.Format("{0} 2>&1 > parfait.tmp & type parfait.tmp & del parfait.tmp \" ", Arguments),
                    Arguments = String.Format("{0}", Arguments),
                    CreateNoWindow = true,
                    ErrorDialog = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
        }
        
        private void OnData(object sender, DataReceivedEventArgs evt)
        {
            Status = CommandStatus.Running;
            NotifyDelegate(evt.Data);
        }

        private void OnError(object sender, DataReceivedEventArgs evt)
        {
            Status = CommandStatus.Error;
            string message = "No description provided.";
            if (!String.IsNullOrEmpty(evt.Data))
            {
                message = evt.Data;
            }
            UserInterface.Log(String.Format("Error: {0}", message));
        }

        private void OnExit(object sender, EventArgs evt)
        {
            Status = CommandStatus.Exited;
            NotifyDelegate(String.Empty, true);
        }

        private void NotifyDelegate(string message, bool isFinal = false)
        {
            AllOutput += message;

            var response = new NodeResponse
            {
                Message = message,
                FullText = AllOutput,
                IsFinal = isFinal,
                Process = Process
            };

            Delegate.Invoke(response);
        }

    }
}
