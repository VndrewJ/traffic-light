using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MECHENG_313_A2.Tasks
{
    public class FiniteStateMachine : IFiniteStateMachine
    {
        //Setup........//

        // create nested dictionary for FST 
        // First string for current event 
        // second string for event trigger 
        // event for next state and a list of actions 
        public Dictionary<string, Dictionary<string, nextEventAction>> fst; 
        
        //Declare current state varible
        string currentState;
        //--------------------------------------------------------------------------------------//

        //constructor 
        public FiniteStateMachine(string currentState){
            this.currentState = currentState;
            fst = new Dictionary<string, Dictionary<string, nextEventAction>>();
        }

        public void AddAction(string state, string eventTrigger, TimestampedAction action)
        {
           // TODO: Implement this - Andrew 
           //idk how to do this, needa ask around
        }

        public string GetCurrentState()
        {
            return currentState;
        }

        public string ProcessEvent(string eventTrigger)
        {
            //wtf 
            return null;
        }

        public void SetCurrentState(string state)
        {
            currentState = state;
        }

        public void SetNextState(string state, string nextState, string eventTrigger)
        {
            // TODO: Implement this - Joe 
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
