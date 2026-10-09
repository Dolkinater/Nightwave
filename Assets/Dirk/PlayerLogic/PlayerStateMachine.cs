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

    public override void StartFunctions()
    {
        base.StartFunctions();

        if (CheckpointLogic.checkpoints <= 0) { return; }

        Debug.Log("position = " + CheckpointLogic.storedPlayerPosition + ";  forward = " + CheckpointLogic.storedFacingDirection);

        transform.position = CheckpointLogic.storedPlayerPosition;
        transform.forward = CheckpointLogic.storedFacingDirection;


        Debug.Log("position = " + transform.position + ";  forward = " + transform.forward);
    }

    public bool canMove = false;

    public float specialAbilityCooldown = 15f;
    float currentSpecialCooldown = 0f;
    public float GetCurrentSpecialCooldown() { return currentSpecialCooldown; }

    public bool GetCanUseSpecialAbility() { return currentSpecialCooldown <= 0; }
    public void ResetSpecialAbilityCooldown() { currentSpecialCooldown = specialAbilityCooldown; }

    [Header("Jump Settings")]

    [HideInInspector] public Vector3 velocity;

    public float gravity = -70f;
    public float jumpHeight = 5f;

    public float parryWindow = 1f;

    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashTime;
    public int dashStock = 3;

    private int currentDashStock;
    public int GetCurrentDashStock() { return currentDashStock; }

    private float dashStockResetTimer;
    public float dashCooldown = 5f;

    [HideInInspector] public Vector3 dashMomentum = Vector3.zero;

    [Header("Player Combat Values")]
    [Header("Character 1 - Rog")]
    public float lightAttack_speed = 4f;
    public float heavyAttack_speed = 4f;

    #endregion

    #region states

    public PlayerStateIdle IdleState { get; private set; }
    public override StatesBase GetInitialState() { return IdleState; }

    public PlayerStateMove MoveState { get; private set; }
    public PlayerStateJump JumpState { get; private set; }
    public PlayerStateSlide SlideState { get; private set; }
    public PlayerStateDash DashState { get; private set; }
    public PlayerStateParry ParryState { get; private set; }
    public PlayerStateDead DeadState { get; private set; }
    public override StatesBase GetDeathState()
    {
        return DeadState;
    }

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
        ParryState = new PlayerStateParry(this);
        DeadState = new PlayerStateDead(this);

        c1_primary = new Char1StatePrimaryAttack(this);
        c1_secondary = new Char1StateSecondaryAttack(this);
        c1_special = new Char1StateSpecialAttack(this);
    }

    #endregion

    #region components

    [HideInInspector] public PlayerInputController inputController;
    [HideInInspector] public PlayerParticleController particleController;

    [HideInInspector] public CharacterController playerController;
    [HideInInspector] public CapsuleCollider playerCollider;

    [HideInInspector] public Health health;

    [HideInInspector] public Animator animator;

    [HideInInspector] public ManualAudioCall manualAudioCall;

    [Header("Camera Rotation")]
    public GameObject orientation;

    [Header("Character Section")]
    public GameObject Character1;
    public GameObject Character2;
    public GameObject Character3;

    [Header("Attack Section")]
    public GameObject c1Attack1Left;
    public GameObject c1Attack1Right;
    public GameObject c1Attack2;
    public GameObject c1SpAttack;

    public override void InstantiateComponents() 
    {
        playerController = GetComponent<CharacterController>();
        inputController = GetComponent<PlayerInputController>();
        particleController = GetComponentInChildren<PlayerParticleController>();
        playerCollider = GetComponent<CapsuleCollider>();

        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();

        manualAudioCall = GetComponentInChildren<ManualAudioCall>();

        c1Attack1Left.SetActive(false);
        c1Attack1Right.SetActive(false);
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
        }

        if (dashStockResetTimer >= dashCooldown)
        {
            dashStockResetTimer = 0;
            currentDashStock++;
        }
    }

    public void PlayerRotation(bool forceUpdate)
    {
        Vector3 viewDirection = transform.position - new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        orientation.transform.forward = viewDirection.normalized;

        float xInput = inputController.moveInput.x;
        float yInput = inputController.moveInput.y;

        Vector3 inputDirection = orientation.transform.forward * yInput + orientation.transform.right * xInput;

        if (forceUpdate) // callers can manually force the player into the right direction regardless of whether "canMove" is true or false
        {
            if (inputDirection == Vector3.zero) { return; }

            transform.forward = inputDirection.normalized;
            return;
        }

        if (canMove == false) { inputDirection = Vector2.zero; }

        if (inputDirection != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, inputDirection.normalized, Time.deltaTime * 90f);
        }
    }

    void PlayerMovement()
    {
        PlayerRotation(false);

        Vector3 moveRotation = transform.forward;

        Vector3 movement = moveRotation * (speed * (canMove ? 1 : 0)) + velocity;

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

    public void CallAttack()
    {
        int attackValue = (inputController.GetAttackValue());

        if (attackValue == -1)
        {
            return; // no attack given
        }

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
                        if (!GetCanUseSpecialAbility()) { return; } // special ability cooldown hasn't reset

                        ResetSpecialAbilityCooldown();

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

    void SpecialAbilityTimerUpdate()
    {
        if (currentSpecialCooldown > 0)
        {
            currentSpecialCooldown -= Time.deltaTime;
        }
    }

    public override void UpdateFunctions() 
    {
        PlayerDashStockUpdate();
        PlayerMovement();
        SpecialAbilityTimerUpdate();

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
