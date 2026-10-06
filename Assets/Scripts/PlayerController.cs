using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashTime;

    public void Dash(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        StartCoroutine(DashCoroutine());
    }

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
            fakeMoveInput = 1f;
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

    public void Slide(InputAction.CallbackContext context)
    {
        
    }

    // Attack Section
    public void Attack1(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        if (currentCharacter == 1)
        {
            Debug.Log("Character1: Attack1");
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
    public void Attack2(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        if (currentCharacter == 1)
        {
            Debug.Log("Character1: Attack2");
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
    public void SpecialAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        if (currentCharacter == 1)
        {
            Debug.Log("Character1: SpecialAttack");
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

     void Update()
    {
        // This is getting the angles of the camera so the player can rotate with it
        Vector3 currentAngles = transform.eulerAngles;
        transform.eulerAngles = new Vector3(currentAngles.x, MainCamera.eulerAngles.y, currentAngles.z);


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

        
        
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
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

