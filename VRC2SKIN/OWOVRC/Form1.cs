using SharpOSC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OWOGame;

namespace OWOVRC
{
    public partial class OWO_VRC_FORM : Form
    {
        public OWO_VRC_FORM()
        {
            InitializeComponent();
        }

        private static string VRCIP;
        private static int VRCPORT;
        private static string OWOIP;

        private class OWOTriggerer
        {
            public OWOTriggerer(Muscle muscle) => Muscle = muscle;
            public volatile bool curState;
            public Muscle Muscle;
        }
        private static BakedSensation bs = BakedSensation.Parse($"0~Touch~80,3,60,0,0,0,~{OWOGame.Icon.Environment}");
        private static readonly Dictionary<string, OWOTriggerer> Triggers = new Dictionary<string, OWOTriggerer>() 
        {
            {"Pectoral_R", new OWOTriggerer(Muscle.Pectoral_R)},
            {"Pectoral_L", new OWOTriggerer(Muscle.Pectoral_L)},
            {"Abdominal_R", new OWOTriggerer(Muscle.Abdominal_R)},
            {"Arm_R", new OWOTriggerer(Muscle.Arm_R)},
            {"Arm_L", new OWOTriggerer(Muscle.Arm_L)},
            {"Dorsal_R", new OWOTriggerer(Muscle.Dorsal_R)},
            {"Dorsal_L", new OWOTriggerer(Muscle.Dorsal_L)},
            {"Lumbar_R", new OWOTriggerer(Muscle.Lumbar_R)},
            {"Lumbar_L", new OWOTriggerer(Muscle.Lumbar_L)}
        };
        private void OWO_VRC_FORM_Load(object sender, EventArgs e)
        {
#if DEBUG
            string[] cmdArgs = Environment.GetCommandLineArgs().Skip(1).ToArray();
            cmdArgLabel.Text = string.Join(" ", cmdArgs);
            for (int i = 0; i < cmdArgs.Length; i++)
            {
                if (i + 1 == cmdArgs.Length)
                    break;

                string curArg = cmdArgs[i];
                string curVal = cmdArgs[i + 1];
                switch (curArg)
                {
                    case "-OWOIP":
                        OWOIP = curVal;
                        break;
                    case "-VRCIP":
                        VRCIP = curVal;
                        break;
                    case "-VRCPORT":
                        VRCPORT = int.Parse(curVal);
                        break;
                }
            }
#else
            OWOIP = "127.0.0.1";
            VRCIP = "127.0.0.1";
            VRCPORT = 9001;
            cmdArgLabel.Text = "this is the version that uses default stuff" + Environment.NewLine + "should work";
#endif
            // Async listen thread
            new Thread(async () => 
            {
                OWO.Configure(GameAuth.Create(bs));

                await OWO.AutoConnect();
                runOnUIThread(() =>
                {
                    debug.Text = "CON";
                });
                UDPListener listener = new UDPListener(VRCPORT);
                OscMessage messageReceived = null;
                while (true)
                {
                    if (messageReceived == null)
                        Thread.Sleep(1);
                    else messageReceived = null;

                    while (messageReceived == null)
                    {
                        messageReceived = (OscMessage)listener.Receive();
                        if (messageReceived == null)
                            Thread.Sleep(1);
                    }
                    
                    if (!messageReceived.Address.StartsWith("/avatar/parameters/owo_suit_"))
                        continue;

                    if (Triggers.TryGetValue(messageReceived.Address.Substring(28), out OWOTriggerer trig)) 
                    //{ 
                        trig.curState = (bool)messageReceived.Arguments[0];
                        //runOnUIThread(() =>
                        {
                        //    debug.Text = trig.curState.ToString();//.Substring(28);// messageReceived.Address + "  " + messageReceived.Arguments[0];
                        }//);
                    //}
                }
            }).Start();

            new Thread(() =>
            {
                OWOTriggerer[] trigs = Triggers.Values.ToArray();
                while(true)
                {
                    Thread.Sleep(300);
                    if (OWO.ConnectionState == OWOGame.ConnectionState.Connected)
                    {
                        List<Muscle> targs = new List<Muscle>();
                        foreach (OWOTriggerer trig in trigs)
                            if (trig.curState)
                                targs.Add(trig.Muscle);
                        
                        if (targs.Count > 0)
                        OWO.Send(bs, targs.ToArray());
                    }
                }
            }).Start();
        }
        private void runOnUIThread(Action function)
        {
            this.Invoke(new MethodInvoker(function));
        }
    }
}
