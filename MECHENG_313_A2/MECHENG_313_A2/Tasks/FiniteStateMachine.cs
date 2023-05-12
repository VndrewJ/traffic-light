using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MECHENG_313_A2.Tasks
{
    public class FiniteStateMachine : IFiniteStateMachine
    {
        /*Pseudocode for table
         * initialise delegates
         * ????
         * 
         * initialise dictionary variables
         * String currentState, nextState
         * 
         * initialise double-nested dict
         * var FST = new Dictionary<current state, Dictionary<next state, action delegate>>();
         * 
         * Add current state entry
         * FST.Add("State A", new Dictionary<next state, action delegate>());
         * 
         * Add next state + action entry in nested dictionary
         * data["State A"].Add("State B", action);
         * 
         * Repeat for other states
         * FST.Add("State B", new Dictionary<next state, action delegate>());
         * data["State B"].Add("State C", action);
         *
         * repeat...
         */


        public void AddAction(string state, string eventTrigger, TimestampedAction action)
        {
            // TODO: Implement this
        }

        public string GetCurrentState()
        {
            // TODO: Implement this
            return null;
        }

        public string ProcessEvent(string eventTrigger)
        {
            // TODO: Implement this
            return null;
        }

        public void SetCurrentState(string state)
        {
            // TODO: Implement this
        }

        public void SetNextState(string state, string nextState, string eventTrigger)
        {
            // TODO: Implement this
        }
    }
}
