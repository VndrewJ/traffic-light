using MECHENG_313_A2.Views;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MECHENG_313_A2.Tasks
{
    internal class Task2 : IController
    {   
        /*-------------------SETUP-------------------*/
        //Instantiate serial interface
        protected MECHENG_313_A2.Serial.MockSerialInterface serial = new MECHENG_313_A2.Serial.MockSerialInterface();

        //declare StreamWriter to write to log 
        protected StreamWriter write;

        //-------Set up FST--------//
        //instantiate new FST, set initial state as green
        protected FiniteStateMachine fsm = new FiniteStateMachine("G"); 
        
        public async void ActionA(DateTime timestamp) 
        { 
            /*---Sequence of actions---
            * 1. Check current state 
            * 2. Change to next state on serial
            * 3. Change traffic light state on GUI
            * 4. Log and print on GUI
            */

            if (fsm.GetCurrentState() == "G"){                
                await serial.SetState(TrafficLightState.Yellow);
                _taskPage.SetTrafficLightState(TrafficLightState.Yellow);
                LogPrint("Tick", "Yellow");
                return;
            }
            else if (fsm.GetCurrentState() == "Y"){
                await serial.SetState(TrafficLightState.Red);
                _taskPage.SetTrafficLightState(TrafficLightState.Red);
                LogPrint("Tick", "Red");
                return;
            }
            else if (fsm.GetCurrentState() == "R"){
               await serial.SetState(TrafficLightState.Green);
               _taskPage.SetTrafficLightState(TrafficLightState.Green);
               LogPrint("Tick", "Green");
               return;
            }
            else if (fsm.GetCurrentState() == "Y'"){
                await serial.SetState(TrafficLightState.None);
                _taskPage.SetTrafficLightState(TrafficLightState.None);
                LogPrint("Tick", "N/A");
                return;
            }
            else if (fsm.GetCurrentState() == "B"){
                await serial.SetState(TrafficLightState.Yellow);
                _taskPage.SetTrafficLightState(TrafficLightState.Yellow);
                LogPrint("Tick", "Yellow (Config)");
                return;
            }
        }

        public async void ActionB(DateTime timestamp) 
        { 
            // Same sequence of actions as actnA but with trigger 'b'
            if (fsm.GetCurrentState() == "R"){
                await serial.SetState(TrafficLightState.Yellow);
                _taskPage.SetTrafficLightState(TrafficLightState.Yellow);
                LogPrint("Config", "Yellow (Config)");
                return;
            }
            else if (fsm.GetCurrentState() == "Y'"){
                await serial.SetState(TrafficLightState.Red);
                _taskPage.SetTrafficLightState(TrafficLightState.Red);
                LogPrint("Config", "Red");
                return;
            }
            else if (fsm.GetCurrentState() == "B"){
                await serial.SetState(TrafficLightState.Red);
                _taskPage.SetTrafficLightState(TrafficLightState.Red);
                LogPrint("Config", "Red");
                return;
            }
        }

        //----------------------------------------

        public virtual TaskNumber TaskNumber => TaskNumber.Task2;

        protected ITaskPage _taskPage;

        public virtual void ConfigLightLength(int redLength, int greenLength)
        {
            //No need for task 2. 
        }

        public virtual async Task<bool> EnterConfigMode()
        {
            //Do nothing if not in Red state
            if (fsm.GetCurrentState() != "R"){
                return false;
            }
            else {
                
                //Send the action associated with the trigger 
                ActionB(DateTime.Now);

                //set the new current state on button press (event trigger "b")
                fsm.SetCurrentState(fsm.GetNextState("b"));
                LogPrint("config", "Entered Config Mode");

                return true;
            }
        }

        public virtual void ExitConfigMode()
        {
            if ((fsm.GetCurrentState() == "Y'") || (fsm.GetCurrentState() == "B")){
                //Send the action associated with the trigger 
                ActionB(DateTime.Now);

                //set the new current state on button press (event trigger "b")
                fsm.SetCurrentState(fsm.GetNextState("b"));
                LogPrint("config", "Exited Config Mode");
            }
        }

        public async Task<string[]> GetPortNames()
        {
            return await serial.GetPortNames();
        }

        public async Task<string> OpenLogFile()
        {   
            //find file path
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "log.txt");

            //check if file exists
            if(File.Exists(filePath)){
                
                //get text from file
                string text = File.ReadAllText(filePath);

                //split the text string into an array and set log entry in GUI
                string[] logArray = text.Split('\n');
                _taskPage.SetLogEntries(logArray);

                //clear old file
                File.WriteAllText(filePath, String.Empty);

                //instantiate streamwriter
                write = new StreamWriter(filePath);

                //indicate that file has been accessed
                write.WriteLine("File Accessed\n");
                write.Flush();

                return filePath;
            }
            else
            {
                return "error: file not found";
            }
        }

        public async Task<bool> OpenPort(string serialPort, int baudRate)
        {
            //open the selected port at the selected baudRate
            return await serial.OpenPort(serialPort, baudRate);
        }

        public void RegisterTaskPage(ITaskPage taskPage)
        {
            _taskPage = taskPage;
        }

        public virtual async void Start()
        {
            //Add the five states to the table 
            fsm.fst.Add("G", new Dictionary<string, NextEventAction>());
            fsm.fst.Add("Y", new Dictionary<string, NextEventAction>());
            fsm.fst.Add("R", new Dictionary<string, NextEventAction>());
            fsm.fst.Add("Y'", new Dictionary<string, NextEventAction>());
            fsm.fst.Add("B", new Dictionary<string, NextEventAction>());

            //Add the event triggers 
            fsm.fst["G"].Add("a", new NextEventAction(null));
            fsm.fst["Y"].Add("a", new NextEventAction(null));
            fsm.fst["R"].Add("a", new NextEventAction(null));
            fsm.fst["R"].Add("b", new NextEventAction(null));
            fsm.fst["Y'"].Add("a", new NextEventAction(null));
            fsm.fst["Y'"].Add("b", new NextEventAction(null));
            fsm.fst["B"].Add("a", new NextEventAction(null));
            fsm.fst["B"].Add("b", new NextEventAction(null));

            //Add actions (AddAction)
            TimestampedAction actionA;
            TimestampedAction actionB;
            actionA = ActionA;
            actionB = ActionB;
            fsm.AddAction("G", "a", actionA);
            fsm.AddAction("Y", "a", actionA);
            fsm.AddAction("R", "a", actionA);
            fsm.AddAction("R", "b", actionB);
            fsm.AddAction("Y'", "a", actionA);
            fsm.AddAction("Y'", "b", actionB);
            fsm.AddAction("B", "a", actionA);
            fsm.AddAction("B", "b", actionB);
            
            //Add Next states
            fsm.SetNextState("G", "Y", "a");
            fsm.SetNextState("Y", "R", "a");
            fsm.SetNextState("R", "G", "a");
            fsm.SetNextState("R", "Y'", "b");
            fsm.SetNextState("Y'", "B", "a");
            fsm.SetNextState("Y'", "R", "b");
            fsm.SetNextState("B", "Y'", "a");
            fsm.SetNextState("B", "R", "b"); 

            //Enter green 
            await serial.SetState(TrafficLightState.Green); //Unsure if this is the right method, for now keep
            _taskPage.SetTrafficLightState(TrafficLightState.Green);
            LogPrint("Starting...", "Green");
        }

        public virtual void Tick()
        {
            //Call action 'a'
            ActionA(DateTime.Now);
            
            //set the new current state on button press (event trigger "a")
            fsm.SetCurrentState(fsm.GetNextState("a"));
        }

        public void LogPrint(string eventTrigger, string state)
        {
            //print to serial
            _taskPage.SerialPrint(DateTime.Now, state);

            //write to log and gui
            //write = new StreamWriter(filePath);
            write.WriteLine(DateTime.Now + " " + eventTrigger + " " + state);
            write.Flush();
            _taskPage.AddLogEntry(DateTime.Now + " " + eventTrigger + " " + state);
        }
    }
}
