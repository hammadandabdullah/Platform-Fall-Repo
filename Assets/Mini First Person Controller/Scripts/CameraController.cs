using UnityEngine;
using Unity.Netcode;

public class CameraController : NetworkBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    public float minYLimit = -30f;
    public float maxYLimit = 30f;

    Vector2 velocity;
    Vector2 frameVelocity;

    private float inputX;
    private float inputY;


    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        character = GetComponentInParent<CharacterMovement>().transform;
    }

    void Start()
    {
        if (!IsOwner)
        {
            Destroy(gameObject);
        }

        // Lock the mouse cursor to the game screen.
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, minYLimit, maxYLimit);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }

    public void SetInputX(float givenX)
    {
        inputX = givenX;
    }

    public void SetInputY(float givenY)
    {
        inputY = givenY;
    }
}

