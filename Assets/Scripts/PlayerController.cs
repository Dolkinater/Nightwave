using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float speed = 10f;

    private CharacterController playerController;
    private Vector2 moveInput;

    public Transform MainCamera;

    [Header("Characters")]
    public GameObject Character1;
    public GameObject Character2;
    public GameObject Character3;

    public int currentCharacter = 1;

    // Really annoying jump variables
    public bool isGrounded = true;
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

        velocity.y += gravity * Time.deltaTime;
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 moveRotation = transform.TransformDirection(moveDirection);

        Vector3 movement = moveRotation * speed + velocity;
        playerController.Move(movement * Time.deltaTime);
    }
}

