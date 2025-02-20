using System;
using Unity.VisualScripting;
using UnityEngine;

public delegate bool ValidateMove(Node _oldNode, Node _newNode);
public class RulesManager : IManager
{
    int ingridientAmount;
    GameObject winPannel;

    public RulesManager(ref ValidateMove _validateMove, ref Action<int> _onGetIngridientAmount, GameObject _winPannel )
    {
        _validateMove += CheckValidMove;
        _onGetIngridientAmount += GetIngridientAmount;
    }

    private bool CheckValidMove(Node _oldNode, Node _newNode)
    {
        if (_oldNode.isBread && !_newNode.isBread)
            return false;

        if (_newNode.IngridientStack.Count == 0)
            return false;

        if (_newNode.isBread && _oldNode.isBread && _newNode.IngridientStack.Count + _oldNode.IngridientStack.Count != ingridientAmount)
            return false;
        else if(_newNode.isBread && _oldNode.isBread)
        {
            winPannel.SetActive(true);
        }

        return true;
    }
    private void GetIngridientAmount(int _amount)
    {
        ingridientAmount = _amount + 2;
    }

   
    public void AwakeFunction() { }

    public void OnDisableFunction() { }

    public void OnEnableFunction() { }
}
