using SharpOSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        private static volatile bool s_shutdown = false;

        private class OWOTriggerer
        {
            public OWOTriggerer(Muscle muscle) => Muscle = muscle;
            public volatile bool curState;
            public Muscle Muscle;
        }
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
            FormClosed += (a, b) => s_shutdown = true;
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
                    case "-STRENGTH":
                        Stren.Value = Strength = int.Parse(curVal);
                        break;
                    case "-FREQUENCY":
                        Freq.Value = Frequency = int.Parse(curVal);
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
#if DEBUG
                await OWO.Connect(OWOIP);
#else
                await OWO.AutoConnect();
#endif

                runOnUIThread(() =>
                {
                    debug.Text = "Connected to OWO!";
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
                    { 
                        trig.curState = (bool)messageReceived.Arguments[0];
#if DEBUG
                        runOnUIThread(() =>
                        {
                            debug.Text = messageReceived.Address + "  " + messageReceived.Arguments[0];
                        });
#endif
                    }

                    if (s_shutdown)
                        break;
                }
            }).Start();

            new Thread(() =>
            {
                OWOTriggerer[] trigs = Triggers.Values.ToArray();
                MicroSensation sen = null;
                while(true)
                {
                    Thread.Sleep(300);
                    if (Dirty)
                    {
                        Dirty = false;
                        sen = SensationsFactory.Create(Frequency, .3f, Strength);
                    }
                    if (OWO.ConnectionState == ConnectionState.Connected)
                    {
                        List<Muscle> targs = new List<Muscle>();
                        foreach (OWOTriggerer trig in trigs)
                            if (trig.curState)
                                targs.Add(trig.Muscle.WithIntensity(Strength));

                        if (targs.Count > 0)
                            OWO.Send(sen.WithMuscles(targs.ToArray()));
                    }
                    if (s_shutdown)
                        break;
                }
            }).Start();
        }
        private void runOnUIThread(Action function)
        {
            Invoke(new MethodInvoker(function));
        }

        private static volatile int Frequency = 40;
        private static volatile int Strength = 30;
        private static volatile bool Dirty = true;

        private void Freq_ValueChanged(object sender, EventArgs e)
        {
            Frequency = (int)Freq.Value/4;
            Dirty = true;
        }

        private void Stren_ValueChanged(object sender, EventArgs e)
        {
            Strength = (int)Stren.Value/4;
            Dirty = true;
        }
    }
}
