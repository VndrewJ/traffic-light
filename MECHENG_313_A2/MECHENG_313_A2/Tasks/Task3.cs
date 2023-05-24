using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;

namespace MECHENG_313_A2.Tasks
{
    internal class Task3 : Task2
    {
        //SETUP--------------------------------------------------------
        
        /*Objective 
        1. Trigger a tick event based on the provided tick duration

        2. Request configuration at any time - still enter through red, but must accept input at other stages 
        */
        
        public override TaskNumber TaskNumber => TaskNumber.Task3;

        private int redLength = 1000;
        private int greenLength = 1000;
        private int defaultLength = 1000; 

        private bool configRequested = false;
        static Timer timer;

        //IMPLEMENTATION-----------------------------------------------

        public override void ConfigLightLength(int redLength, int greenLength)
        {
            //Overriden to set ther light length 
            this.redLength = redLength;
            this.greenLength = greenLength; 
        }

        public override async Task<bool> EnterConfigMode()
        {
            if (fsm.GetCurrentState() != "R"){
                if ((fsm.GetCurrentState()=="Y")&&(fsm.GetCurrentState()=="G")){
                    configRequested = true;
                    return true;
                }
                else{
                    return false;
                }
            }
            else {
                
                //Send the action associated with the trigger 
                actnB(DateTime.Now);

                //set the new current state on button press (event trigger "b")
                fsm.SetCurrentState(fsm.GetNextState("b"));
                LogPrint("config", "Entered Config Mode");

                return true;
            }
        }

        public override void ExitConfigMode()
        {
            timer.Interval = redLength;
            timer.Stop();
            timer.Start();
            base.ExitConfigMode();
        }

        public override async void Start()
        {
            base.Start();
            TimerConfig();
        }

        public void TimerConfig(){
            timer = new Timer();
            timer.Elapsed += Trigger;
            timer.Interval = greenLength;
            timer.AutoReset = true;
            timer.Start();
        }

        public void Trigger (object sender, ElapsedEventArgs e)
        {
            if ((fsm.GetCurrentState()=="R")&&(configRequested)){
                configRequested = false;
                EnterConfigMode();
                timer.Interval=defaultLength;
            }
            else{
                Tick();
            }
           
            //Determine next interval based on the new state 
            if (fsm.GetCurrentState()=="G"){
                timer.Interval=greenLength;
            }
            else if (fsm.GetCurrentState()=="R"){
                timer.Interval=redLength;
            }
            else {
                timer.Interval=defaultLength;
            }
        }
    }
}
