using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker
{
    Stack<ICommand> commands;

    public Stack<ICommand> Commands => commands;

    public CommandInvoker()
    {
        commands = new();
    }

    public void ExecuteCommand(ICommand command)
    {
        commands.Push(command);
        command.Execute();
    }

    public void UndoCommand() => commands?.Pop().Undo();
}
