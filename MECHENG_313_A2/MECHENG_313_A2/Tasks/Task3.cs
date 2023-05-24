using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

        private bool _isRed = false;
        static System.Timers.Timer timer;

        //IMPLEMENTATION-----------------------------------------------

        public override void ConfigLightLength(int redLength, int greenLength)
        {
            //Overriden to set ther light length 
            this.redLength = redLength;
            this.greenLength = greenLength; 
        }

        public override async Task<bool> EnterConfigMode()
        {
            //spinlock that stalls thread if not red 
            if(fsm.GetCurrentState() != "R"){
                SpinWait.SpinUntil(()=> (fsm.GetCurrentState()=="G"));
            }

            //Send the action associated with the trigger 
            actnB(DateTime.Now);

            //set the new current state on button press (event trigger "b")
            fsm.SetCurrentState(fsm.GetNextState("b"));
            LogPrint("config", "Entered Config Mode");

            return true;
        }

        public override void ExitConfigMode()
        {
            base.ExitConfigMode();
            timer.Stop();
            _isRed = false;
            timer.Interval = redLength;
            timer.Start();
        }

        public override async void Start()
        {
            base.Start();
            timer = new System.Timers.Timer(greenLength);
            timer.Elapsed += TimerConfig;
            timer.AutoReset = true;
            timer.Start();
        }

        public void TimerConfig(object state, ElapsedEventArgs e)
        {
            Tick();
            //g to r
            if(fsm.GetCurrentState()=="G"){
                
                timer.Interval = greenLength;
            }
            else if(fsm.GetCurrentState()=="R" && !_isRed){
            
                timer.Interval = redLength;
            }
            else{
                
                timer.Interval = defaultLength;
            }

        }
    }
}
