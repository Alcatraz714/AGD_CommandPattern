using System;
using System.Collections.Generic;
using Command.Commands;
using Command.Main;

public class CommandInvoker
{
    private Stack<ICommand> commandRegistry = new Stack<ICommand>();

    public CommandInvoker() => SubscribeToEvents();

    private void SubscribeToEvents() => GameService.Instance.EventService.OnReplayButtonClicked.AddListener(SetReplayStack);

    public void SetReplayStack()
    {
        //using the saved commands stack which is command registery
        GameService.Instance.ReplayService.SetCommandStack(commandRegistry);
        commandRegistry.Clear();
    }

    /// <summary>
    /// Process a command, which involves both executing it and registering it.
    /// </summary>
    /// <param name="commandToProcess">The command to be processed.</param>
    public void ProcessCommand(ICommand commandToProcess)
    {
        ExecuteCommand(commandToProcess);
        RegisterCommand(commandToProcess);
    }

    /// <summary>
    /// Execute a command, invoking its associated action.
    /// </summary>
    /// <param name="commandToExecute">The command to be executed.</param>
    public void ExecuteCommand(ICommand commandToExecute) => commandToExecute.Execute();

    /// <summary>
    /// Undo a command, invoking its associated action reversal.
    /// </summary>
    /// <param name="commandToUndo">The command to be executed which reverses the action in stack top.</param>
    public void Undo() => commandRegistry.Pop().Undo();

    /// <summary>
    /// Register a command by adding it to the command registry stack.
    /// </summary>
    /// <param name="commandToRegister">The command to be registered.</param>
    public void RegisterCommand(ICommand commandToRegister) => commandRegistry.Push(commandToRegister);
    
    // Prevent underflow of command regsitery stack
    private bool RegistryEmpty() => commandRegistry.Count == 0;

    // prevent player from using Undo on 2nd player acitons
    private bool CommandBelongsToActivePlayer() 
    {
        return(commandRegistry.Peek() as UnitCommand).commandData.ActorPlayerID == GameService.Instance.PlayerService.ActivePlayerID;
    }
}
