using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public float distance = 2f;
    public float sensitivity = 3f;

    public float minimumYLook = -20f;
    public float maximumYLook = 85f;

    private float currentXLook;
    private float currentYLook;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // I need to come back to this and use the new input system for the mouse
        // But this was just so much easier, there is framework for it in the inputsystem
        currentXLook += Input.GetAxis("Mouse X") * sensitivity;
        currentYLook -= Input.GetAxis("Mouse Y") * sensitivity;

        currentYLook = Mathf.Clamp(currentYLook, minimumYLook, maximumYLook);

        Quaternion rotation = Quaternion.Euler(currentYLook, currentXLook, 0);

        RaycastHit check;
        float currentDistance = distance;

        if (Physics.Raycast(player.transform.position, (this.transform.position - player.transform.position).normalized, out check, distance, LayerMask.GetMask("Walls")))
        {
            currentDistance = check.distance;   
        }

        Vector3 direction = new Vector3(0, 0, -currentDistance);


        Vector3 position = player.position + (rotation * direction);

        transform.position = position;
        transform.LookAt(player.position);

    }
}