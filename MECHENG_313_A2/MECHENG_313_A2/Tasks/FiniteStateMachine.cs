using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MECHENG_313_A2.Tasks
{
    public class FiniteStateMachine : IFiniteStateMachine
    {
        //Index for the FSM table
        private int currentState = 0;
        
        //Struct for FSM table
        private struct Cell
        {
            private string nextState;
            TimestampedAction currentAction;
        }

        

        //Setting up FSM table 
        Cell[,] fsmTable = new Cell[5,2];
        fsmTable [0,0] = new Cell {nextState = "Y",  currentAction = GREEN};
        fsmTable [0,1] = new Cell {nextState = "G",  currentAction = GREEN};
        fsmTable [1,0] = new Cell {nextState = "R",  currentAction = YELLOW};
        fsmTable [1,1] = new Cell {nextState = "Y",  currentAction = YELLOW};
        fsmTable [2,0] = new Cell {nextState = "G",  currentAction = RED};
        fsmTable [2,1] = new Cell {nextState = "Y'", currentAction = RED};
        fsmTable [3,0] = new Cell {nextState = "B",  currentAction = YELLOW};
        fsmTable [3,1] = new Cell {nextState = "R",  currentAction = YELLOW};
        fsmTable [4,0] = new Cell {nextState = "B",  currentAction = NONE};
        fsmTable [4,1] = new Cell {nextState = "Y'", currentAction = NONE};
        
        //-------------------------------------------------------------------------------------
        public void AddAction(string state, string eventTrigger, TimestampedAction action)
        {
            // TODO: Implement this
        }

        public string GetCurrentState()
        {
            if (currentState == 0){
                return "G";
            }
            else if (currentState == 1){
                return "Y";
            }
            else if (currentState == 2){
                return "R";
            }
            else if (currentState == 3){
                return "Y'";
            }
            else{
                return "B";
            }
        }

        public string ProcessEvent(string eventTrigger)
        {
            // TODO: Implement this
            return null;
        }

        public void SetCurrentState(string state)
        {
            if (state == "G"){
                currentState = 0;
            }
            else if (state == "Y"){
                currentState = 1;
            }
            else if (currentState == "R"){
                currentState = 2;
            }
            else if (currentState == "Y'"){
                currentState = 3;
            }
            else{
                currentState = 4;
            }
        }

        public void SetNextState(string state, string nextState, string eventTrigger)
        {
            // TODO: Implement this
        }
    }
}
