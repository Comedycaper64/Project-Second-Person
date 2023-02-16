using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    public Vector2 movementValue;
    public EventHandler<Ray> SwitchCameraEvent;

    private void Awake() 
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movementValue = context.ReadValue<Vector2>();
    }

    public void OnSwitchCamera(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray switchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            SwitchCameraEvent.Invoke(this, switchRay);
        }
    }
}
