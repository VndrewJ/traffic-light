using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MECHENG_313_A2.Tasks
{
    public class FiniteStateMachine : IFiniteStateMachine
    {
        // create nested dictionary for FST 
        // First string for current event 
        // second string for event trigger 
        // object for next state and a list of actions 
        public Dictionary<string, Dictionary<string, nextEventAction>> fst; 
        
        //Declare current state varible
        private string currentState;
        //--------------------------------------------------------------------------------------//

        //constructor 
        public FiniteStateMachine(string currentState){
            SetCurrentState(currentState);
            fst = new Dictionary<string, Dictionary<string, nextEventAction>>();
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
            return currentState;
        }

        public string ProcessEvent(string eventTrigger)
        {
            /*pseudocode for multrithreading
                initialise list of tasks
                foreach action in actionlist
                    queue each action into a task

                return nextState;

            */
            // //initialise list of tasks to run in parallel
            // List<Task> runningTasks = new List<Task>();

            // foreach(TimestampedAction action in fst[currentState][eventTrigger].actionList){
            //     runningTasks.Add(Task.Run(action));
            // }

           
            //TEMP 
            //execute each delegate sequentially for now
            foreach(TimestampedAction action in fst[currentState][eventTrigger].actionList){
                action.Invoke(DateTime.Now);
            }
            

            return fst[currentState][eventTrigger].getNextState();
        }

        public void SetCurrentState(string state)
        {
            this.currentState = state;
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
            return fst[currentState][eventTrigger].getNextState();
        }
    }

    public class nextEventAction
    {
        //variables
        private string nextState;
        public List<TimestampedAction> actionList;

        //Constructor 
        public nextEventAction(string nextState){
            setNextState(nextState);
            actionList = new List<TimestampedAction>();
        }
        
        //Methods 
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
