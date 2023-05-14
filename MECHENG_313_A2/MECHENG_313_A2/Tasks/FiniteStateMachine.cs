using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MECHENG_313_A2.Tasks
{
    public class FiniteStateMachine : IFiniteStateMachine
    {
        //Setup........//

        //Intitialise action delegates (a and b)
        public static void a(DateTime timestamp) { return; }
        public static void b(DateTime timestamp) { return; }
        static TimestampedAction actionA = a;
        static TimestampedAction actionB = b;

        //create nested dictionary for FST 
        public Dictionary<string, Dictionary<string, TimestampedAction>> fst =
            new Dictionary<string, Dictionary<string, TimestampedAction>>
            {   
                //State G 
                {"G", new Dictionary<string, TimestampedAction>{{"Y", actionA}}},
                //State Y
                {"Y", new Dictionary<string, TimestampedAction>{{"R", actionA}}},
                //State R
                {"R", new Dictionary<string, TimestampedAction>{{"G", actionA},{"Y'", actionB}}},
                //State Y'
                {"Y'", new Dictionary<string, TimestampedAction>{{"B", actionA},{"R", actionB}}},
                //State B
                {"B", new Dictionary<string, TimestampedAction>{{"Y'", actionA},{"R", actionB}}}
            };

        //Initialise current state varible
        string currentState = "G";

        //--------------------------------------------------------------------------------------//

        public void AddAction(string state, string eventTrigger, TimestampedAction action)
        {
           // TODO: Implement this - Andrew 
        }

        public string GetCurrentState()
        {
            return currentState;
        }

        public string ProcessEvent(string eventTrigger)
        {
            // TODO: Implement this - Andrew 
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
}
