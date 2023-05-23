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

        ConfigHelper configHelper = new ConfigHelper();

        //IMPLEMENTATION-----------------------------------------------

        public override void ConfigLightLength(int redLength, int greenLength)
        {
            //Overriden to set ther light length 
            this.redLength = redLength;
            this.greenLength = greenLength; 
        }

        public override async Task<bool> EnterConfigMode()
        {
            Task waitingTask = configHelper.WaitForRedLightAsync();
            Thread.Sleep(redLength);
            configHelper.SetRedLight();
            
            if((configHelper.GetIsRed())==false){
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

        

    }

    public class ConfigHelper
    {
        private SemaphoreSlim semaphore = new SemaphoreSlim (0,1);
        private bool isRed = false; 

        public async Task WaitForRedLightAsync()
        {
            while(true){
                await semaphore.WaitAsync();
                if (isRed){
                    break;
                }
            }
        }

        public void SetRedLight()
        {
            isRed = true;
            semaphore.Release();
        }

        public void SetGreenLight()
        {
            isRed = false;
        }

        public bool GetIsRed(){
            return this.isRed;
        }
    }

    
}
