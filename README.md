# MECHENG_313_A2
----------GROUP 8---------- 
----------Members----------

Andrew Koh      akoh751
Joe Kim         jkim914

-----------Notes-----------

Changes to IController 
    - Added System.Timers
    - Added declarations for the action delegates 
    - Added declarations for the helper functions LogPrint() (for printing to serial) and TimerConfig() (foor adjusting the timer length)

Changes to IFininiteStateMachine 
    - Added an internal interface INextEventAction for the helper class NextEventAction used to hold the actions and the next state 
    - Added the declaration for the helper method GetNextState()
