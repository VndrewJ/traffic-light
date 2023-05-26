using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MECHENG_313_A2.Tasks
{
    public class FiniteStateMachine : IFiniteStateMachine
    {
        // Declare Finite State Table 
        // 1st string for current event 
        // 2nd string for event trigger 
        // object for next state and a list of actions 
        public Dictionary<string, Dictionary<string, NextEventAction>> fst; 
        
        //Declare current state varible
        private string _currentState;
        //--------------------------------------------------------------------------------------//

        //FST constructor 
        public FiniteStateMachine(string _currentState){
            SetCurrentState(_currentState);
            fst = new Dictionary<string, Dictionary<string, NextEventAction>>();
        }

        public void AddAction(string state, string eventTrigger, TimestampedAction action)
        {
           //Look for the current state and event trigger 
            if (fst.ContainsKey(state) && fst[state].ContainsKey(eventTrigger))
            {   
                //set the next action
                fst[state][eventTrigger].actionAdd(action);
            }
        }

        public string GetCurrentState()
        {
            return _currentState;
        }

        public string ProcessEvent(string eventTrigger)
        {
            //initialise list of tasks to run in parallel
            List<Task> runningTasks = new List<Task>();

            //iterate through list of tasks and run each in parallel
            foreach(TimestampedAction action in fst[_currentState][eventTrigger].actionList){
                runningTasks.Add(Task.Run(()=>action(DateTime.Now)));
            }

            //return next state
            return fst[_currentState][eventTrigger].getNextState();
        }

        public void SetCurrentState(string state)
        {
            this._currentState = state;
        }

        public void SetNextState(string state, string nextState, string eventTrigger)
        {
            //Look for the current state and event trigger 
            if (fst.ContainsKey(state) && fst[state].ContainsKey(eventTrigger))
            {   
                //set the next state
                fst[state][eventTrigger].setNextState(nextState);
            }
        }

        public string GetNextState(string eventTrigger)
        {
            return fst[_currentState][eventTrigger].getNextState();
        }
    }

    /*--------- NextEventAction Class ---------*/
    /*
    * Contains the next state and list of actions
    */
    public class NextEventAction: INextEventAction
    {
        // Declare variables
        private string nextState;
        public List<TimestampedAction> actionList;

        //Constructor that sets next state and initialises action list
        public NextEventAction(string nextState){
            setNextState(nextState);
            actionList = new List<TimestampedAction>();
        }
        
        /*----Methods----*/ 
        public void setNextState(string nextState){
            this.nextState=nextState;
        }

        public string getNextState(){
            return nextState;
        }

        public void actionAdd(TimestampedAction newAction)
        {
            actionList.Add(newAction);
            return; 
        }

        public List<TimestampedAction> getAction(){
            return actionList;
        }
    }
}
