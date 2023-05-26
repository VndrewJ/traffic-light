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
        //----------------------SETUP------------------------//
        public override TaskNumber TaskNumber => TaskNumber.Task3;

        // Initialise light length variables
        private int _redLength = 1000;
        private int _greenLength = 1000;
        private int _defaultLength = 1000; 

        // Initialise flags for entering config mode
        private bool _isRed = false;
        private bool _config = false;

        //declare timer for light length
        static System.Timers.Timer timer;

        //---------------------IMPLEMENTATION-----------------------//

        public override void ConfigLightLength(int redLength, int greenLength)
        {
            //Overriden to set ther light length 
            this._redLength = redLength;
            this._greenLength = greenLength; 
        }

        public override async Task<bool> EnterConfigMode()
        {
            //Overriden to be able to wait for lights to turn red

            _config = true;
            //spinlock that stalls thread if not red 
            SpinWait.SpinUntil(()=> (_isRed));

            //Send the action associated with the trigger 
            ActionB(DateTime.Now);

            //set the new current state on button press (event trigger "b")
            fsm.SetCurrentState(fsm.GetNextState("b"));
            LogPrint("config", "Entered Config Mode");

            return true;
        }

        public override void ExitConfigMode()
        {   
            // Overriden to reset timer on exit

            base.ExitConfigMode();
            timer.Stop();
            _isRed = false;
            _config = false; 
            timer.Interval = _redLength;
            timer.Start();
        }

        public override async void Start()
        {   
            //Overriden to start timer 
            base.Start();
            timer = new System.Timers.Timer(_greenLength);
            timer.Elapsed += TimerConfig;
            timer.AutoReset = true;
            timer.Start();
        }

        public override void Tick()
        {
            //Overriden to disallow tick to occur past red if config is flagged

            //checks if light is red and config flag is up, flags _isRed
            if (fsm.GetCurrentState() == "R" && _config ) {
                _isRed = true;
            }

            //if no flag, continue to tick past red
            else {
                //Call action a
                ActionA(DateTime.Now);
                //set the new current state on button press (event trigger "a")
                fsm.SetCurrentState(fsm.GetNextState("a"));
            }
        }


        public void TimerConfig(object state, ElapsedEventArgs e)
        {
            //tick, then adjust timer length based on current light state
            Tick();
            if(fsm.GetCurrentState()=="G"){
                timer.Interval = _greenLength;
            }
            else if(fsm.GetCurrentState()=="R" && !_isRed){
                timer.Interval = _redLength;
            }
            else{
                timer.Interval = _defaultLength;
            }
        }
    }
}
