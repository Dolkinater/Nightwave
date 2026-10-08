using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputController : MonoBehaviour
{
    public Vector2 moveInput;

    /*void OnMove(InputValue value)
    {
        Debug.Log("Response");
    }

    public void OnMove() //InputAction.CallbackContext context)
    {
        Debug.Log("HJJJJJJJJJJJ");
        Debug.Log("Move");
        moveInput = context.ReadValue<Vector2>();
    }*/

    private bool jumpPressed = false;
    public bool GetJumpPressed() 
    {
        bool getVal = jumpPressed;
        jumpPressed = false;
        return getVal;
    }
    private float jumpBuffer;

    public void OnJump() //InputAction.CallbackContext context)
    {
        /*if (!context.performed)
         {
             return;
         }*/

        jumpPressed = true;
        jumpBuffer = 0.5f;
    }

    private bool dashPressed = false;
    public bool GetDashPressed() 
    {
        bool getVal = dashPressed;
        dashPressed = false;   
        return getVal; 
    }
    private float dashBuffer;

    public void OnDash() //InputAction.CallbackContext context)
    {
        /*if (!context.performed)
        {
            return;
        }*/

        dashPressed = true;
        dashBuffer = 0.5f;
    }

    private bool slidePressed = false;
    public bool GetSlidePressed() 
    {
        bool getVal = slidePressed;
        slidePressed = false;
        return getVal;
    }
    private float slideBuffer;

    public void OnSlide() //InputAction.CallbackContext context)
    {
        /*if (!context.performed)
        {
            return;
        }*/

        slidePressed = true;
        slideBuffer = 0.5f;
    }

    int characterSwapped = -1;
    public int GetSwapValue()
    {
        int getVal = characterSwapped;
        characterSwapped = -1;
        return getVal; 
    }
    private float swapBuffer;

    public void OnCharacterSwitch1() //InputAction.CallbackContext context)
    {
        /*if (!context.performed)
        {
            return;
        }*/

        characterSwapped = 0;
        swapBuffer = 0.5f;
    }

    public void OnCharacterSwitch2() //InputAction.CallbackContext context)
    {
        /*if (!context.performed)
        {
            return;
        }*/

        characterSwapped = 1;
        swapBuffer = 0.5f;
    }

    public void OnCharacterSwitch3() //InputAction.CallbackContext context)
    {
        /*if (!context.performed)
        {
            return;
        }*/

        characterSwapped = 2;
        swapBuffer = 0.5f;
    }

    int attackSelected = -1;
    public int GetAttackValue()
    {
        int getVal = attackSelected;
        attackSelected = -1;
        return getVal;
    }
    private float attackBuffer;

    public void OnAttack1() //InputAction.CallbackContext context)
    {
        /*if (!context.performed)
        {
            return;
        }*/

        attackSelected = 0;

        attackBuffer = 0.5f;
    }
    public void OnAttack2() //InputAction.CallbackContext context)
    {
        /*if (!context.performed)
        {
            return;
        }*/

        attackSelected = 1;

        attackBuffer = 0.5f;
    }
    public void OnSpecialAttack() //InputAction.CallbackContext context)
    {
        /*if (!context.performed)
        {
            return;
        }*/

        attackSelected = 2;

        attackBuffer = 0.5f;
    }

    void Update()
    {
        UpdateBuffers();
        UpdateInputs();
    }

    void UpdateInputs()
    {
        int inputX = 0;
        int inputY = 0;

        if (Keyboard.current.wKey.isPressed) { inputY += 1; }
        if (Keyboard.current.sKey.isPressed) { inputY -= 1; }
        if (Keyboard.current.aKey.isPressed) { inputX -= 1; }
        if (Keyboard.current.dKey.isPressed) { inputX += 1; }

        moveInput = new Vector2(inputX, inputY);

        if (Keyboard.current.digit1Key.wasPressedThisFrame) { OnCharacterSwitch1(); }

        if (Keyboard.current.digit2Key.wasPressedThisFrame) { OnCharacterSwitch2(); }

        if (Keyboard.current.digit3Key.wasPressedThisFrame) { OnCharacterSwitch3(); }

        if (Mouse.current.leftButton.wasPressedThisFrame) { OnAttack1(); }

        if (Mouse.current.rightButton.wasPressedThisFrame) { OnAttack2(); }

        if (Keyboard.current.rKey.wasPressedThisFrame) { OnSpecialAttack(); }

        if (Keyboard.current.spaceKey.wasPressedThisFrame) { OnJump(); }

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame) { OnDash(); }

        if (Keyboard.current.leftCtrlKey.wasPressedThisFrame) { OnSlide(); }
    }

    void UpdateBuffers()
    {
        if (dashBuffer > 0)
        {
            dashBuffer -= Time.deltaTime;
        }
        else
        {
            dashPressed = false;
        }

        if (jumpBuffer > 0)
        {
            jumpBuffer -= Time.deltaTime;
        }
        else
        {
            jumpPressed = false;
        }

        if (slideBuffer > 0)
        {
            slideBuffer -= Time.deltaTime;
        }
        else
        {
            slidePressed = false;
        }

        if (swapBuffer > 0)
        {
            swapBuffer -= Time.deltaTime;
        }
        else
        {
            characterSwapped = -1;
        }

        if (attackBuffer > 0)
        {
            attackBuffer -= Time.deltaTime;
        }
        else
        {
            attackSelected = -1;
        }
    }
}
