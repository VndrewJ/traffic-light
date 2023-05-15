using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MECHENG_313_A2.Tasks
{
    public class FiniteStateMachine : IFiniteStateMachine
    {
        //Setup........//


        // //Intitialise action delegates (a and b)
        // public static void a(DateTime timestamp) { return; }
        // public static void b(DateTime timestamp) { return; }
        // static TimestampedAction actionA = a;
        // static TimestampedAction actionB = b;

        //create nested dictionary for FST 
        // public Dictionary<string, Dictionary<string, ...>> fst =
        //     new Dictionary<string, Dictionary<string, ...>>
        //     {   
        //         // //State G 
        //         // {"G", new Dictionary<string, TimestampedAction>{{"Y", actionA}}},
        //         // //State Y
        //         // {"Y", new Dictionary<string, TimestampedAction>{{"R", actionA}}},
        //         // //State R
        //         // {"R", new Dictionary<string, TimestampedAction>{{"G", actionA},{"Y'", actionB}}},
        //         // //State Y'
        //         // {"Y'", new Dictionary<string, TimestampedAction>{{"B", actionA},{"R", actionB}}},
        //         // //State B
        //         // {"B", new Dictionary<string, TimestampedAction>{{"Y'", actionA},{"R", actionB}}}
        //     };

        //Initialise varibles
        string currentState;


        //initialise FST
        //current state, event trigger, eventHandler struct(next state, action list)
        public Dictionary<string, Dictionary<string, EventHandler>> finiteStateTable = 
        new Dictionary<string, Dictionary<string, EventHandler>>();


        //--------------------------------------------------------------------------------------//

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

    //struct for containing the nextState and list of actions 
    public struct EventHandler{

        //variables
        private string nextState;
        public List<TimestampedAction> actionsList;

        //constructor
        public EventHandler(string nextState){
            this.nextState = nextState;
            actionsList = new List<TimestampedAction>();
        }
    }
}
