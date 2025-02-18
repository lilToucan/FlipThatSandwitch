using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker
{
   Stack<ICommand> commands;

    public CommandInvoker()
    {

    }

    public void ExecuteCommand(ICommand command)
    {
        commands.Push(command);
        command.Execute();
    }

    public void UndoCommand() => commands.Pop().Undo();
}
