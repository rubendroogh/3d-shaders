using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Movment stuff
    [Space]
    [Header("------------ Movement settings")]
    public float movementSpeed = .05f;
    [Range(0.0f, 0.2f)]
    public float walkSmoothModifier = 0.05f;

    private float playerHeight = 2;
    private new Rigidbody rigidbody;
    private Vector3 walkingVelocity = new Vector3(0, 0, 0);
    private Vector2 rotation;
    private bool grounded;
    private bool moving;

    // Camera stuff
    [Space]
    [Header("------------ Camera settings")]
    [Range(1.0f, 15f)]
    public float lookSensitivity = 5f;
    [Range(0.0f, 0.2f)]
    public float headbobIntensity = 0.1f;
    [Range(0.05f, 1f)]
    public float headbobTime = 0.5f;

    private new Camera camera;
    private Vector3 headbobSpeed = new Vector3(0, 0, 0);


   
    

    private void Start()
    {
        this.camera = GetComponentInChildren<Camera>();
        this.rigidbody = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        this.CrouchControl();
        this.MouseControl();
        this.JumpControl();
    }

    void FixedUpdate()
    {
        this.MovementControl();
        this.Headbob();
        this.GroundedChecks();
    }

    /// <summary>
    /// Takes in movement buttons and moves player.
    /// </summary>
    void MovementControl()
    {
        this.walkingVelocity = new Vector3(0, 0, 0);
        this.moving = false;

        if (Input.GetKey(KeyCode.W))
        {
            walkingVelocity += this.transform.forward;
            this.moving = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            walkingVelocity -= this.transform.right;
            this.moving = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            walkingVelocity += this.transform.right;
            this.moving = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            walkingVelocity -= this.transform.forward;
            this.moving = true;
        }

        walkingVelocity.Normalize();
        this.transform.position += walkingVelocity * movementSpeed;
    }

    /// <summary>
    /// Checks if player is grounded and adjust accordingly
    /// </summary>
    void GroundedChecks()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, this.playerHeight / 2 + 0.1f))
        {
            this.rigidbody.useGravity = false;
            this.rigidbody.velocity = new Vector3(0, 0, 0);
            //this.camera.transform.position;
            if (Mathf.Abs(this.transform.position.y - hit.point.y + 0.001f + (playerHeight / 2)) < this.walkSmoothModifier)
            {
                float test = this.transform.position.y - hit.point.y + 0.001f + (playerHeight / 2);
                Mathf.Lerp(hit.point.y + 0.001f + (playerHeight / 2), this.transform.position.y, test / -.1f - 0.001f);
            }
            this.grounded = true;
        }
        else
        {
            this.rigidbody.useGravity = true;
            this.grounded = false;
        }
    }

    /// <summary>
    /// Sets headbob position when moving.
    /// </summary>
    void Headbob()
    {
        if (this.moving == true && this.grounded)
        {
            if (Mathf.Abs(this.headbobSpeed.y) <= 0.1f * headbobTime)
            {
                this.headbobIntensity = this.headbobIntensity * -1;
            }

            Vector3 target = new Vector3(0, this.headbobIntensity, 0);

            this.camera.transform.localPosition = Vector3.SmoothDamp(this.camera.transform.localPosition, target, ref this.headbobSpeed, this.headbobTime);
        }
    }

    /// <summary>
    /// Takes in mouse controll and rotates camera
    /// </summary>
    void MouseControl()
    {
        rotation.x += Input.GetAxis("Mouse X") * lookSensitivity;
        rotation.y -= Input.GetAxis("Mouse Y") * lookSensitivity;

        rotation.x = Mathf.Repeat(rotation.x, 360);
        rotation.y = Mathf.Clamp(rotation.y, -90, 90);

        camera.transform.rotation = Quaternion.Euler(rotation.y, rotation.x, 0);
        this.transform.rotation = Quaternion.Euler(0, rotation.x, 0);
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Checks for space being pressed then adds upward velocity
    /// </summary>
    void JumpControl()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.grounded == true)
        {
            //this.transform.position += this.transform.up * movementSpeed;
            this.rigidbody.AddForce(this.transform.up * 1500);
        }
    }

    /// <summary>
    /// Checks if control is currently pressed and makes you crouch.
    /// </summary>
    void CrouchControl()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            this.movementSpeed = .02f;
            this.playerHeight = 1;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            this.movementSpeed = .05f;
            this.playerHeight = 2;
        }
    }
}
