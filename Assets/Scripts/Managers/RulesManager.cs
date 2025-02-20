using System;
using Unity.VisualScripting;
using UnityEngine;
public class RulesManager : IManager
{
    int ingridientAmount;
    GameObject winPannel;
    public Action<Node> onWin;

    public RulesManager( GameObject _winPannel)
    {
        winPannel = _winPannel;
        
    }

    public bool CheckValidMove(Node _oldNode, Node _newNode)
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
            onWin?.Invoke(_newNode);
        }

        return true;
    }
    public void GetIngridientAmount(int _amount)
    {
        ingridientAmount = _amount + 2;
    }

   
    public void AwakeFunction() { }

    public void OnDisableFunction() { }

    public void OnEnableFunction() { }
}
