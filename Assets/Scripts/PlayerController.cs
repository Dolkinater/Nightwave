using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public bool canMove = true;
    public bool isMoving;
    public StateMachine SM;
    public float speed = 10f;

    [HideInInspector] public CharacterController playerController;

    [HideInInspector] public Vector2 moveInput;

    public Transform MainCamera;

    [Header("Characters")]
    public GameObject Character1;
    public GameObject Character2;
    public GameObject Character3;

    public int currentCharacter = 1;

    // Really annoying jump variables
    public bool isGrounded = true;
    [Header("Jump Settings")]
    public float gravity = -70f;
    private Vector3 velocity;
    private bool jumpPressed = false;
    public float jumpHeight = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<CharacterController>();
        Character2.SetActive(false);
        Character3.SetActive(false);
    }

    // Movement Section
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (canMove)
        {
            isMoving = true;
            
        }
        if (context.canceled)
        {
            isMoving = false;
        }

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        if (playerController.isGrounded)
        {
            jumpPressed = true;
        }
        if (groundedDash)
        {
            jumpPressed = true;
        }
    }

    #region Dashing + Sliding

    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashTime;
    private bool canDash = true;
    public int dashStock = 3;
    public float dashCooldown = 5f;
    public void Dash(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        if (!isSliding && canDash && !isDashing)
        {
            StartCoroutine(DashCoroutine());
            dashStock--;
            if (dashStock == 2 && !rechargeDashStarted)
            {
                rechargeDashStarted = true;
                StartCoroutine(RechargeDash());
            }

        }

    }

    private bool rechargeDashStarted = false;
    private bool isDashing = false;
    public bool groundedDash = false;
    public bool jumpDash = false;
    public bool jumpDashAirborneFalling = false;

    private Vector3 dashMomentum = Vector3.zero;
    
    IEnumerator DashCoroutine()
    {
        isDashing = true;
        jumpDash = false;
        float gravityValue = gravity;
        gravity = 0f;
        velocity.y = 0f;
        bool noMovementDash = false;
        float fakeMoveInput = 0f;
        if (moveInput == Vector2.zero)
        {
            fakeMoveInput = 2f;
            noMovementDash = true;
        }
        if (isGrounded)
        {
            groundedDash = true;
        }
        if (noMovementDash)
        {
            Vector3 moveDirection = new Vector3(moveInput.x, 0, fakeMoveInput);
            Vector3 moveRotation = transform.TransformDirection(moveDirection);
            float startTime = Time.time;
            while (Time.time < startTime + dashTime)
            {
                if (groundedDash && jumpPressed)
                {
                    jumpDash = true;
                    dashMomentum *= 2f;
                    break;
                }
                playerController.Move(moveRotation * dashSpeed * Time.deltaTime);
                dashMomentum = moveRotation * dashSpeed;
                yield return null;
            }
        }
        else
        {
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            Vector3 moveRotation = transform.TransformDirection(moveDirection);
            float startTime = Time.time;
            while (Time.time < startTime + dashTime)
            {
                if (groundedDash && jumpPressed)
                {
                    jumpDash = true;
                    dashMomentum *= 2f;
                    break;
                }
                playerController.Move(moveRotation * dashSpeed * Time.deltaTime);
                dashMomentum = moveRotation * dashSpeed;
                yield return null;
            }
        }

        gravity = gravityValue;
        groundedDash = false;
        isDashing = false;
    }

    IEnumerator RechargeDash()
    {
        if (rechargeDashStarted)
        {
            while (dashStock < 3)
            {
                yield return new WaitForSeconds(dashCooldown);
                dashStock++;
            }
            rechargeDashStarted = false;
        }
    }


    private bool isSliding = false;
    private bool canSlide = true;
    private Vector3 slideDirection = Vector3.zero;
    public void Slide(InputAction.CallbackContext context)
    {
        if (canSlide)
        {
            if (context.performed && !isSliding)
            {
                moveX = 0;
                moveY = 0;
                canMove = false;
                slideDirection = transform.forward;
                playerController.height = 1;
                isSliding = true;
                canAttack = false;

            }



            if (context.canceled)
            {
                playerController.height = 2;
                isSliding = false;
                canMove = true;
                slideDirection = Vector3.zero;
                moveInput = storedInput;
                canAttack = true;
            }
        }
    }
    #endregion

    #region Attacks

    // Attack Section
    [Header("Attack Section")]
    public GameObject c1Attack1;
    public GameObject c1Attack2;
    public GameObject c1SpAttack;
    private bool canAttack = true;
    private bool canSpecialAttack = true;
    public void Attack1(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        if (currentCharacter == 1)
        {
            if (canAttack)
            {
                StartCoroutine(C1Attack1Execute());
                Debug.Log("Character1: Attack1");
            }
        }
        if (currentCharacter == 2)
        {
            Debug.Log("Character2: Attack1");
        }
        if (currentCharacter == 3)
        {
            Debug.Log("Character3: Attack1");
        }
    }

    IEnumerator C1Attack1Execute()
    {
        DisablePlayer();
        c1Attack1.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        c1Attack1.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        EnablePlayer();
    }



    public void Attack2(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        if (currentCharacter == 1)
        {
            if (canAttack)
            {
                StartCoroutine(C1Attack2Execute());
                Debug.Log("Character1: Attack2");
            }
        }
        if (currentCharacter == 2)
        {
            Debug.Log("Character2: Attack2");
        }
        if (currentCharacter == 3)
        {
            Debug.Log("Character3: Attack2");
        }
    }

    IEnumerator C1Attack2Execute()
    {
        DisablePlayer();
        c1Attack2.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        c1Attack2.SetActive(false);
        yield return new WaitForSeconds(0.8f);
        EnablePlayer();
    }
    public void SpecialAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        if (currentCharacter == 1)
        {
            if (canAttack && canSpecialAttack)
            {
                StartCoroutine(C1SpAttackExecute());
                Debug.Log("Character1: SpecialAttack");
            }
           
        }
        if (currentCharacter == 2)
        {
            Debug.Log("Character2: SpecialAttack");
        }
        if (currentCharacter == 3)
        {
            Debug.Log("Character3: SpecialAttack");
        }
    }

    IEnumerator C1SpAttackExecute()
    {
        canSlide = false;
        canSpecialAttack = false;
        canAttack = false;
        c1SpAttack.SetActive(true);
        speed = speed / 2;
        yield return new WaitForSeconds(2f);
        c1SpAttack.SetActive(false);
        speed = speed * 2;
        canAttack = true;
        yield return new WaitForSeconds(15f);
        canSpecialAttack = true;
        canSlide = true;
    }

    private void DisablePlayer()
    {
        canSlide = false;
        canAttack = false;
        canMove = false;
    }

    private void EnablePlayer()
    {
        canSlide = true;
        canAttack = true;
        canMove = true;
    }

    #endregion


    // Character Switch Section
    public void onCharacterSwitch1(InputAction.CallbackContext context)
    {
        currentCharacter = 1;
        Character1.SetActive(true);
        Character2.SetActive(false);
        Character3.SetActive(false);
    }

    public void onCharacterSwitch2(InputAction.CallbackContext context)
    {
        currentCharacter = 2;
        Character1.SetActive(false);
        Character2.SetActive(true);
        Character3.SetActive(false);
    }

    public void onCharacterSwitch3(InputAction.CallbackContext context)
    {
        currentCharacter = 3;
        Character1.SetActive(false);
        Character2.SetActive(false);
        Character3.SetActive(true);
    }

    float moveX;
    float moveY;
    private Vector2 storedInput;
     void Update()
    {

        moveX = moveInput.x;
        moveY = moveInput.y;

        // This is getting the angles of the camera so the player can rotate with it
        Vector3 currentAngles = transform.eulerAngles;
        transform.eulerAngles = new Vector3(currentAngles.x, MainCamera.eulerAngles.y, currentAngles.z);


        if (isSliding)
        {
            playerController.Move(slideDirection * 10f * Time.deltaTime);
            storedInput = moveInput;
            moveX = 0;
            moveY = 0;
        }
        if (!canMove)
        {
            moveX = 0;
            moveY = 0;
        }

        // Jump stuff that apparently HAS to be in update, otherwise it only jumps after movement
        isGrounded = playerController.isGrounded;

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;
        }

        if (dashStock < 1)
        {
            canDash = false;
        }
        else
        {
            canDash = true;
        }

        Vector3 moveDirection = new Vector3(moveX, 0, moveY);
        Vector3 moveRotation = transform.TransformDirection(moveDirection);
        Vector3 movement = moveRotation * speed + velocity;
        if (!isDashing && jumpDash)
        {
            movement = moveRotation * speed + dashMomentum + velocity;
            dashMomentum = Vector3.Lerp(dashMomentum, Vector3.zero, Time.deltaTime * 3f);
            if (dashMomentum.magnitude < 0.1f)
            {
                dashMomentum = Vector3.zero;
                jumpDash = false;
            }
            if (velocity.y < -5f)
            {
                jumpDashAirborneFalling = true;
            }
            if (jumpDashAirborneFalling && isGrounded)
            {
                dashMomentum = Vector3.zero;
                jumpDashAirborneFalling = false;
            }
        }
        velocity.y += gravity * Time.deltaTime;
        playerController.Move(movement * Time.deltaTime);
        
    }
}