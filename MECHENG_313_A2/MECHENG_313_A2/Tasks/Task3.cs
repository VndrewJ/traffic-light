using System;
using System.Collections.Generic;
using System.Text;
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

        private bool configRequested = false; //Config request flag if config is requested from yellow and green

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
                configRequested = true;
                return false;
            }
            else{
                
                //Send the action associated with the trigger 
                actnB(DateTime.Now);

                //set the new current state on button press (event trigger "b")
                fsm.SetCurrentState(fsm.GetNextState("b"));
                LogPrint("config", "Entered Config Mode");

                return true;
            }
            

            // //Overriden to allow for input from other states as well 
            // if (fsm.GetCurrentState() != "R"){
            //     //If requested from green and yellow
            //     if ((fsm.GetCurrentState() == "G") || (fsm.GetCurrentState() == "Y")){
            //         //updates the request flag 
            //         configRequested=true;
            //         return false;
            //     }
            //     else {
            //         return false;
            //     }
            // }
            // else {
            //     //clear the request flag 
            //     configRequested=false;
            //     //Send the action associated with the trigger 
            //     actnB(DateTime.Now);
            //     //set the new current state on button press (event trigger "b")
            //     fsm.SetCurrentState(fsm.GetNextState("b"));
            //     LogPrint("config", "Entered Config Mode");
            //     return true;
            // }
        }
        public override void Tick()
        {
            /*
            Pseudocode
                if configRequested
                    if red
                        enterconfig
                        reset configRequested
                    tick
                tick
            */
            //if config is requested
            if(configRequested == true && fsm.GetCurrentState() == "R"){
                
            }
            base.Tick();
        }
    }
}
