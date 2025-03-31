using System.Collections;
using System.Collections.Generic;
using Command.Commands;
using Command.Main;
using UnityEngine;


namespace Command.Replay
{
    public class ReplayService
    {
        // A stack to store recorded commands for replay.
        private Stack<ICommand> replayCommandStack;

        // Property to get or set the current replay state.
        public ReplayState ReplayState { get; private set; }

        // Constructor for the ReplayService. Initializes the replay state as "DEACTIVE."
        public ReplayService() => SetReplayState(ReplayState.DEACTIVE);

        /// Set the replay state to the specified state.
        public void SetReplayState(ReplayState stateToSet) => ReplayState = stateToSet;

        // Set the command stack for replay, providing a collection of commands to replay.
        public void SetCommandStack(Stack<ICommand> commandsToSet) => replayCommandStack = new Stack<ICommand>(commandsToSet);

        // Execute the next recorded command in the stack if there are commands left to replay.
        public IEnumerator ExecuteNext()
        {
            yield return new WaitForSeconds(1);

            if(replayCommandStack.Count > 0)
                GameService.Instance.ProcessUnitCommand(replayCommandStack.Pop());
        }
    }
    // A class responsible for managing and controlling the replay of recorded commands.
    public enum ReplayState
    {
        ACTIVE,
        DEACTIVE
    }
}