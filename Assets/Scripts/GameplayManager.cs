using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class GameplayManager : HomoBehaviour
{
    private Inputs inputs;

    private CommandInvoker invoker;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void OnDisable()
    {
        EnhancedTouch.Touch.onFingerDown -= OnTouch;
        EnhancedTouch.Touch.onFingerMove -= OnTouchMove;

        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();

        EnhancedTouch.Touch.onFingerDown += OnTouch;
        EnhancedTouch.Touch.onFingerMove += OnTouchMove;
    }

    private void OnTouch(Finger finger)
    {
        Ray ray = mainCam.ScreenPointToRay(finger.screenPosition);

        Physics.Raycast(ray, out RaycastHit hit);

        if (hit.transform.TryGetComponent(out Ingredient i))
        {
        }
    }

    private void OnTouchMove(Finger finger)
    {
    }
}