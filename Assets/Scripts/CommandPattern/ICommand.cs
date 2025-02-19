using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    public void Execute();
    public void Undo();
}

public class FlipStackCommand : ICommand
{
    
    Dictionary<Ingredient,Vector3> undoIngridients;
    Node oldNode;
    Node newNode;
    float ingridientHight;

    public FlipStackCommand(Node _oldNode, Node _newNode, float _ingridientHight)
    {
        oldNode = _oldNode;
        newNode = _newNode;
        undoIngridients = new Dictionary<Ingredient, Vector3>(oldNode.IngridientStack.Count );
        ingridientHight = _ingridientHight;
    }

    public void Execute()
    {
        while (oldNode.IngridientStack.Count != 0)
        {
            Vector3 pos = Vector3.zero;
            var ing = oldNode.IngridientStack.Pop();
            undoIngridients.Add(ing, ing.transform.position);



            if (newNode.IngridientStack.Count == 0)
                pos = newNode.Position;
            else
                pos = newNode.IngridientStack.Peek().transform.position + Vector3.one * ingridientHight;

            ing.transform.position = pos;

            ing.Coordinates = newNode.Coordinates;
            newNode.IngridientStack.Push(ing);
        }
        
    }

    public void Undo()
    {

        foreach (var undoIngredient in undoIngridients)
        {
            newNode.IngridientStack.Remove(undoIngredient.Key);
            oldNode.IngridientStack.Push(undoIngredient.Key);
            undoIngredient.Key.Coordinates = oldNode.Coordinates;
            undoIngredient.Key.transform.position = undoIngredient.Value;
        }

    }
}
