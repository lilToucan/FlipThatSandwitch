using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;


public class GameplayManager : IManager
{
    private Camera mainCam;

    private Ingredient ingredientHit;

    public delegate Node GetNode(Vector2Int coordinates);
    public GetNode OnGetNodeRequest;

    public delegate void ExecuteComand(ICommand command);
    public ExecuteComand OnExecuteCommandRequest;
    bool isMovingDone = true;

    public GameplayManager(ExecuteComand _executeCommand, GetNode _getNode)
    {
        OnExecuteCommandRequest += _executeCommand;
        OnGetNodeRequest += _getNode;
    }

    public void AwakeFunction()
    {
        mainCam = Camera.main;
    }

    public void OnEnableFunction()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();

        EnhancedTouch.Touch.onFingerDown += OnTouch;
        EnhancedTouch.Touch.onFingerMove += OnTouchMove;
        EnhancedTouch.Touch.onFingerUp += OnTouchRelese;
    }


    public void OnDisableFunction()
    {
        EnhancedTouch.Touch.onFingerDown -= OnTouch;
        EnhancedTouch.Touch.onFingerMove -= OnTouchMove;
        EnhancedTouch.Touch.onFingerUp -= OnTouchRelese;
        OnExecuteCommandRequest -= OnExecuteCommandRequest;
        OnGetNodeRequest -= OnGetNodeRequest;



        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
    }


    private void OnTouch(Finger finger)
    {
        Debug.Log(finger.index);
        Ray ray = mainCam.ScreenPointToRay(finger.screenPosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 10f);
        Physics.Raycast(ray, out RaycastHit hit);
        if (hit.transform == null)
            return;

        hit.transform.TryGetComponent(out ingredientHit);
    }

    private void OnTouchMove(Finger finger)
    {
        if (!isMovingDone ||ingredientHit == null )
            return;

        Vector2 touchStartPos = finger.currentTouch.startScreenPosition;
        Vector2 touchEndPos = finger.currentTouch.screenPosition;

        if (Vector2.Distance(touchStartPos, touchEndPos) < 30f)
            return;

        Vector2 touchDirection = (touchEndPos- touchStartPos).normalized;
        Vector2 dirNorm = touchDirection;

        // checks if the direction is diagonal 
        if (touchDirection.x == 0.5f)
            return;

        isMovingDone = false;

        Node oldNode = OnGetNodeRequest?.Invoke(ingredientHit.Coordinates);

        if (Mathf.Abs(touchDirection.x) > Mathf.Abs(touchDirection.y))
        {
            dirNorm.y = 0;
        }
        else
        {
            dirNorm.x = 0;
        }

        dirNorm = dirNorm.normalized;
        float dot = Vector2.Dot(dirNorm, touchDirection);

        if (dot < 0.7f ) // the dot needs to be > 70 
            return;

        Vector2Int dir = new(oldNode.Coordinates.x + (int)dirNorm.x, oldNode.Coordinates.y + (int)dirNorm.y);
        Debug.Log(dir);
        Node newNode = OnGetNodeRequest?.Invoke(dir);

        FlipStackCommand flip = new(oldNode, newNode, 0.3f);
        OnExecuteCommandRequest?.Invoke(flip);

    }

    private void OnTouchRelese(Finger finger)
    {
        isMovingDone = true;
        ingredientHit = null;
    }
}
