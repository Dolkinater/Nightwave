using System;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    #region variables

    public float speed = 10f;

    int currentCharacter = 0;
    public void SetCurrentCharacter(int characterIndex)
    {
        currentCharacter = characterIndex;

        Character1.SetActive(currentCharacter == 0);
        Character2.SetActive(currentCharacter == 1);
        Character3.SetActive(currentCharacter == 2);
    }
    
    public override void InstantiateValues() 
    { 
        base.InstantiateValues();
        SetCurrentCharacter(0);

        currentDashStock = dashStock;
    }

    public bool canMove = false;

    [Header("Jump Settings")]

    [HideInInspector] public Vector3 velocity;

    public float gravity = -70f;
    public float jumpHeight = 5f;

    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashTime;
    public int dashStock = 3;

    private int currentDashStock;
    private float dashStockResetTimer;
    public float dashCooldown = 5f;

    [HideInInspector] public Vector3 dashMomentum = Vector3.zero;

    #endregion

    #region states

    public PlayerStateIdle IdleState { get; private set; }
    public override StatesBase GetInitialState() { return IdleState; }

    public PlayerStateMove MoveState { get; private set; }
    public PlayerStateJump JumpState { get; private set; }
    public PlayerStateSlide SlideState { get; private set; }
    public PlayerStateDash DashState { get; private set; }

    public Char1StatePrimaryAttack c1_primary { get; private set; }
    public Char1StateSecondaryAttack c1_secondary { get; private set; }
    public Char1StateSpecialAttack c1_special { get; private set; }
    
    public override void InstantiateStates() 
    {
        base.InstantiateStates();

        IdleState = new PlayerStateIdle(this);
        MoveState = new PlayerStateMove(this);
        JumpState = new PlayerStateJump(this);
        SlideState = new PlayerStateSlide(this);
        DashState = new PlayerStateDash(this);

        c1_primary = new Char1StatePrimaryAttack(this);
        c1_secondary = new Char1StateSecondaryAttack(this);
        c1_special = new Char1StateSpecialAttack(this);
    }

    #endregion

    #region components

    [HideInInspector] public PlayerInputController inputController;
    [HideInInspector] public PlayerParticleController particleController;

    [HideInInspector] public CharacterController playerController;

    [Header("Character Section")]
    public GameObject Character1;
    public GameObject Character2;
    public GameObject Character3;

    [Header("Attack Section")]
    public GameObject c1Attack1;
    public GameObject c1Attack2;
    public GameObject c1SpAttack;

    public override void InstantiateComponents() 
    {
        playerController = GetComponent<CharacterController>();
        inputController = GetComponent<PlayerInputController>();
        particleController = GetComponentInChildren<PlayerParticleController>();

        c1Attack1.SetActive(false);
        c1Attack2.SetActive(false);
        c1SpAttack.SetActive(false);
    }

    #endregion

    public void PlayerDashCheck()
    {
        if (inputController.GetDashPressed())
        {
            if (currentDashStock < 1)
            {
                return;
            }
            else
            {
                currentDashStock--;
            }

            ChangeState(DashState);
            return;
        }
    }

    void PlayerDashStockUpdate()
    {
        if (currentDashStock < dashStock)
        {
            dashStockResetTimer += Time.deltaTime;
            Debug.Log("Timer increasing: " + dashStockResetTimer);
        }

        if (dashStockResetTimer >= dashCooldown)
        {
            dashStockResetTimer = 0;
            currentDashStock++;
        }
    }

    void PlayerMovement()
    {
        Vector3 moveDirection = new Vector3(inputController.moveInput.x, 0, inputController.moveInput.y);

        bool canRotate = moveDirection != Vector3.zero && canMove; // player should only move relative to the camera when movement input is received and when the player canMove is set to true

        Vector3 currentAngles = transform.eulerAngles;
        transform.eulerAngles = new Vector3(currentAngles.x, canRotate ? Camera.main.transform.eulerAngles.y : currentAngles.y, currentAngles.z);


        if (canMove == false) { moveDirection = Vector2.zero; }

        Vector3 moveRotation = transform.TransformDirection(moveDirection);
        Vector3 movement = moveRotation * speed + velocity;

        if (GetCurrentState() != DashState && DashState.jumpDash == true)
        {
            movement = moveRotation * speed + dashMomentum + velocity;
            dashMomentum = Vector3.Lerp(dashMomentum, Vector3.zero, Time.deltaTime * 3f);
            if (dashMomentum.magnitude < 0.1f)
            {
                dashMomentum = Vector3.zero;
                DashState.jumpDash = false;
            }
            if (velocity.y < -5f)
            {
                DashState.jumpDashAirborneFalling = true;
            }
            if (DashState.jumpDashAirborneFalling && playerController.isGrounded)
            {
                dashMomentum = Vector3.zero;
                DashState.jumpDashAirborneFalling = false;
            }
        }


        velocity.y += gravity * Time.deltaTime;

        playerController.Move(movement * Time.deltaTime);
    }

    public void CallAttack(int attackValue)
    {
        switch (currentCharacter)
        {
            case 0:
                switch (attackValue)
                {
                    case 0:
                        ChangeState(c1_primary);
                        break;
                    case 1:
                        ChangeState(c1_secondary);
                        break;
                    case 2:
                        ChangeState(c1_special);
                        break;
                }
                break;
            case 1:
                switch (attackValue)
                {
                    case 0:
                        Debug.Log("Character2: Attack1");
                        break;
                    case 1:
                        Debug.Log("Character2: Attack2");
                        break;
                    case 2:
                        Debug.Log("Character2: SpecialAttack");
                        break;
                }
                break;
            case 2:
                switch (attackValue)
                {
                    case 0:
                        Debug.Log("Character3: Attack1");
                        break;
                    case 1:
                        Debug.Log("Character3: Attack2");
                        break;
                    case 2:
                        Debug.Log("Character3: SpecialAttack");
                        break;
                }
                break;
        }
    }

    public override void UpdateFunctions() 
    {
        PlayerDashStockUpdate();
        PlayerMovement();

        if (playerController.isGrounded)
        {
            //velocity modifier

            if (velocity.y < 0f)
            {
                velocity.y = -2f;
            }
        }
    }
}
