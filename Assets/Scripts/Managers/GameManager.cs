using System;
using UnityEngine;
public class GameManager : HomoBehaviour
{
    [SerializeField] private Ingredient breadPrefab;
    [SerializeField] private Ingredient ingridientPrefab;

    [SerializeField] private Vector2Int ingredientAmountRange;

    [SerializeField] private float ingredientSize = 1.5f;

    [SerializeField] private GameObject winPannel;

    IManager[] managers;

    Grid gameGrid;
    CommandInvoker invoker;

    private void ExecuteCommand(ICommand command) => invoker.ExecuteCommand(command);
    public void UndoCommand() => invoker.UndoCommand();

    private void Awake()
    {
        gameGrid = new(Vector3.zero, ingredientSize);
        invoker = new();
        
        RulesManager rulesManager = new(winPannel);
        InputManager inputManager = new(ExecuteCommand, GetNodeFromCoordinates);
        LevelManager levelManager = new(gameGrid, breadPrefab, ingridientPrefab, ingredientAmountRange);

        levelManager.OnIngridientAmountGenerated += rulesManager.GetIngridientAmount;
        inputManager.onValidateMove +=  rulesManager.CheckValidMove;
        rulesManager.onWin += levelManager.GetStack;
        rulesManager.onWin += inputManager.Won;
        inputManager.OnDeleteLevel += levelManager.DestroyLevel;

        managers = new IManager[]
        {
            rulesManager,
            inputManager,
            levelManager
        };

        StartGame();
    }
    public void StartGame()
    {
        gameGrid.Reset();
        invoker.Commands.Clear();
        foreach (var manager in managers)
        {
            manager.AwakeFunction();
        }
    }

    private void OnEnable()
    {
        foreach (var manager in managers)
        {
            manager.OnEnableFunction();
        }
    }

    private void OnDisable()
    {
        foreach (var manager in managers)
        {
            manager.OnDisableFunction();
        }
    }


    private Node GetNodeFromCoordinates(Vector2Int coordinates)
    {
        if (coordinates.x >= 4 || coordinates.x < 0 || coordinates.y >= 4 || coordinates.y < 0)
            return null;

        return gameGrid.NodeGrid[coordinates.x, coordinates.y];
    }

    public void ResetLevel()
    {
        while(invoker.Commands.Count != 0)
        {
            invoker.UndoCommand();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (gameGrid == null)
            gameGrid = new(Vector3.zero, ingredientSize);
        foreach (Node node in gameGrid.NodeGrid)
        {
            Gizmos.DrawWireCube(node.Position, Vector3.one);
        }
    }
#endif
}
