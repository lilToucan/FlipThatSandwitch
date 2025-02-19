using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class GameplayManager : HomoBehaviour
{
    private Inputs inputs;

    private CommandInvoker invoker;

    private Camera mainCam;

    private Vector3 mouseStartScreenPos;

    private Ingredient ingredientHit;

    public delegate Node GetNode(Vector2Int coordinates);
    public GetNode OnGetNodeRequest;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();

        EnhancedTouch.Touch.onFingerDown += OnTouch;
        EnhancedTouch.Touch.onFingerMove += OnTouchMove;
        EnhancedTouch.Touch.onFingerUp += OnTouchRelese;
    }


    private void OnDisable()
    {
        EnhancedTouch.Touch.onFingerDown -= OnTouch;
        EnhancedTouch.Touch.onFingerMove -= OnTouchMove;
        EnhancedTouch.Touch.onFingerUp -= OnTouchRelese;

        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
    }


    private void OnTouch(Finger finger)
    {
        mouseStartScreenPos = finger.screenPosition;
        Ray ray = mainCam.ScreenPointToRay(finger.screenPosition);
        Physics.Raycast(ray, out RaycastHit hit);
        hit.transform.TryGetComponent(out ingredientHit);
    }

    private void OnTouchMove(Finger finger)
    {
        
    }

    private void OnTouchRelese(Finger finger)
    {
        ingredientHit = null;
    }
}