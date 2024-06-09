using UnityEngine;
using Unity.Netcode;

public class PlayerInitialPosition : NetworkBehaviour
{
    public static bool canMove = false;
    /* public Animator anim;

     public float speed = 5f;
     public float rotationSpeed = 720f; // Speed at which the player rotates to face the movement direction


     public float jumpForce;
     public float jumpTime;
     public float jumpTimeCounter;
     *//*this bool is to tell us whether you are on the ground or not
      * the layermask lets you select a layer to be ground; you will need to create a layer named ground(or whatever you like) and assign your
      * ground objects to this layer.
      * The stoppedJumping bool lets us track when the player stops jumping.*//*
     public bool grounded;
     public LayerMask whatIsGround;
     public bool stoppedJumping;
     private Rigidbody rb;

     *//*the public transform is how you will detect whether we are touching the ground.
      * Add an empty game object as a child of your player and position it at your feet, where you touch the ground.
      * the float groundCheckRadius allows you to set a radius for the groundCheck, to adjust the way you interact with the ground*//*

     public Transform groundCheck;
     public float groundCheckRadius;

     public Transform camTransform;*/

    /*    void Start()
        {
            rb = GetComponent<Rigidbody>();
            //sets the jumpCounter to whatever we set our jumptime to in the editor
            jumpTimeCounter = jumpTime;
            canMove = false;
        }*/

    void Start()
    {
        canMove = true;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            transform.position = SpawnPoints.Instance.spawnPoints[Random.Range(0, SpawnPoints.Instance.spawnPoints.Length)].position;
            GameManager.Instance.SetActiveCountDownTimer(true);
        }
    }

   /* void Update()
    {
        if (!IsOwner) return;
        if (!canMove) return;

        HandleMovement();

        Collider[] overLapSpheresColliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, whatIsGround);
        grounded = overLapSpheresColliders.Length > 0;

        //if we are grounded...
        if (grounded)
        {
            //the jumpcounter is whatever we set jumptime to in the editor.
            jumpTimeCounter = jumpTime;
        }
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        //I placed this code in FixedUpdate because we are using phyics to move.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //and you are on the ground...
            if (grounded)
            {
                //jump!
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                stoppedJumping = false;
            }
        }

        if ((Input.GetKey(KeyCode.Space)) && !stoppedJumping)
        {
            //and your counter hasn't reached zero...
            if (jumpTimeCounter > 0)
            {
                //keep jumping!
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
        }


        if (Input.GetKeyUp(KeyCode.Space))
        {
            //stop jumping and set your counter to zero.  The timer will reset once we touch the ground again in the update function.
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }
    }

    /////
    void HandleMovement()
    {
        // Get input from the player
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction based on input
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // Set Animation
        float blend = movementDirection.magnitude;
        anim.SetFloat("Blend", blend);

        // If there is any movement input
        if (movementDirection.magnitude >= 0.1f)
        {
            // Determine the target rotation based on the camera's forward direction
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

            // Rotate the player smoothly to face the target direction
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));

            // Move the player in the direction it's currently facing
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);
        }
    }
    *//*    private Vector3 movementDirection;
        void HandleMovement()
        {
            // Get input from the player
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calculate the movement direction based on input
            movementDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

            //Set Animation
            float blend = movementDirection.magnitude;
            anim.SetFloat("Blend", blend);

            // If there is any movement input
            if (movementDirection.magnitude >= 0.1f)
            {
                // Determine the target rotation based on the camera's forward direction
                float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
                Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

                // Rotate the player smoothly to face the target direction
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Move the player in the direction it's currently facing
                Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
            }
        }*//*
    /////

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }*/
}