using System;
using UnityEngine;

public interface ICommand
{
    public void Execute();
    public void Undo();
}

public class FlipCommand : ICommand
{
    Ingredient ingredient;

    public FlipCommand(Ingredient _ingredient)
    {
        ingredient = _ingredient;
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
