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
        //Initiate serial interface
        private MECHENG_313_A2.Serial.MockSerialInterface serial = new MECHENG_313_A2.Serial.MockSerialInterface();


        //-------Set up FST----------
        private FiniteStateMachine fsm = new FiniteStateMachine("G"); //create new FST, set current state as green
        
        public async void actnA(DateTime timestamp) 
        { 
            if (fsm.GetCurrentState() == "G"){
                //1. send serial command to microcontroller
                //2. write a log entry to file for the event trigger 
                //3. Update the traffic light state on the gui
                // Parallel under multiple threads 
                
                await serial.SetState(TrafficLightState.Yellow);
                
                return;
            }
            else if (fsm.GetCurrentState() == "Y"){
                await serial.SetState(TrafficLightState.Red);
                return;
            }
            else if (fsm.GetCurrentState() == "R"){
               await serial.SetState(TrafficLightState.Green);
               return;
            }
            else if (fsm.GetCurrentState() == "Y'"){
                await serial.SetState(TrafficLightState.None);
                return;
            }
            else if (fsm.GetCurrentState() == "B"){
                await serial.SetState(TrafficLightState.Yellow);
                return;
            }
        }

        public async void actnB(DateTime timestamp) 
        { 
            if (fsm.GetCurrentState() == "R"){
                await serial.SetState(TrafficLightState.Yellow);
                return;
            }
            else if (fsm.GetCurrentState() == "Y'"){
                await serial.SetState(TrafficLightState.Red);
                return;
            }
            else if (fsm.GetCurrentState() == "B"){
                await serial.SetState(TrafficLightState.Red);
                return;
            }
        }

        //----------------------------------------

        public virtual TaskNumber TaskNumber => TaskNumber.Task2;

        protected ITaskPage _taskPage;

        public void ConfigLightLength(int redLength, int greenLength)
        {
            //No need for task 2. 
        }

        public async Task<bool> EnterConfigMode()
        {
            if (fsm.GetCurrentState() != "R"){
                return false;
            }
            else {
                
                //Send the action associated with the trigger 
                actnB(DateTime.Now);

                //set the new current state on button press (event trigger "b")
                fsm.SetCurrentState(fsm.GetNextState("b"));

                return true;
            }
        }

        public void ExitConfigMode()
        {
            if ((fsm.GetCurrentState() == "Y'") || (fsm.GetCurrentState() == "B")){
                //Send the action associated with the trigger 
                actnB(DateTime.Now);

                //set the new current state on button press (event trigger "b")
                fsm.SetCurrentState(fsm.GetNextState("b"));
            }
        }

        public async Task<string[]> GetPortNames()
        {
            return await serial.GetPortNames();
        }

        public async Task<string> OpenLogFile()
        {
            // Help notes: to read a file named "log.txt" under the LocalApplicationData directory,
            // you may use the following code snippet:
            //string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "log.txt");
            //string text = File.ReadAllText(filePath);
            //
            // You can also create/write to file(s) through System.IO.File. 
            // See https://learn.microsoft.com/en-us/xamarin/xamarin-forms/data-cloud/data/files?tabs=windows, and
            // https://learn.microsoft.com/en-us/dotnet/api/system.io.file?view=netstandard-2.0 for more details.

            //await not working on this??
            //check if file exists
            if(File.Exists("log.txt")){
                //get text from file
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "log.txt");
                string text = File.ReadAllText(filePath);

                //set the log entries in the gui
                //TODO

                return text;
            }
            else{
                return "error: file not found";
            }
        }

        public async Task<bool> OpenPort(string serialPort, int baudRate)
        {
            return await serial.OpenPort(serialPort, baudRate);
        }

        public void RegisterTaskPage(ITaskPage taskPage)
        {
            _taskPage = taskPage;
        }

        public void Start()
        {
            //Add the five states to the table 
            fsm.fst.Add("G", new Dictionary<string, nextEventAction>());
            fsm.fst.Add("Y", new Dictionary<string, nextEventAction>());
            fsm.fst.Add("R", new Dictionary<string, nextEventAction>());
            fsm.fst.Add("Y'", new Dictionary<string, nextEventAction>());
            fsm.fst.Add("B", new Dictionary<string, nextEventAction>());

            //Add the event triggers 
            fsm.fst["G"].Add("a", new nextEventAction(null));
            fsm.fst["Y"].Add("a", new nextEventAction(null));
            fsm.fst["R"].Add("a", new nextEventAction(null));
            fsm.fst["R"].Add("b", new nextEventAction(null));
            fsm.fst["Y'"].Add("a", new nextEventAction(null));
            fsm.fst["Y'"].Add("b", new nextEventAction(null));
            fsm.fst["B"].Add("a", new nextEventAction(null));
            fsm.fst["B"].Add("b", new nextEventAction(null));

            //Add actions (AddAction)
            TimestampedAction actionA;
            TimestampedAction actionB;
            actionA = actnA;
            actionB = actnB;
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
            serial.SetState(TrafficLightState.Green); //Unsure if this is the right method, for now keep
        }

        public void Tick()
        {
            //Call action a
            actnA(DateTime.Now);
            
            //set the new current state on button press (event trigger "a")
            fsm.SetCurrentState(fsm.GetNextState("a"));
        }
    }
}
