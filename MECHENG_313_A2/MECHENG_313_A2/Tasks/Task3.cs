using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

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
            if(fsm.GetCurrentState() != "R"){
                SpinWait.SpinUntil(()=> (fsm.GetNextState("a")=="G"));
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
            timer.Change(redLength, Timeout.Infinite);
            base.ExitConfigMode();
        }

        public override async void Start()
        {
            base.Start();
            timer = new Timer(TimerCallback, null, 0, greenLength);
            timer.Change(greenLength, Timeout.Infinite);

        }

        
        //where the timer if configured for each state
        public void TimerCallback(object state)
        {
            Tick();
            //for green to Yellow
            if(fsm.GetCurrentState() == "G"){ 
                timer.Change(greenLength, Timeout.Infinite);
            }
            //for red to green
            else if(fsm.GetCurrentState() == "R"){  
                timer.Change(redLength, Timeout.Infinite);
            }
            else{
                timer.Change(defaultLength, Timeout.Infinite);
            }
        }
        
    }
}
