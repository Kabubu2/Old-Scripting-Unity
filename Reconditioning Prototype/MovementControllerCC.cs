using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementControllerCC : MonoBehaviour
{
	
	//This was the character controller. This handled everything the player did.
    private CharacterController cc;
    public GameObject groundCheck, headCheck;
    public Camera viewCamera;
    //public int Energy;
    private void Start()-
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cc = GetComponent<CharacterController>();
        height = cc.height;
        crouchHeight = height * .1f;
        groundCheckHeight = groundCheck.transform.localPosition.y;
        crouchgroundCheckHeight = groundCheck.transform.localPosition.y * .25f;
    }
    public bool canClimb;
    private void FixedUpdate()
    {
        StartCoroutine("groundLogic");
        if (!canClimb)
        {
            move();
        }
    }
    private void Update()
    {
        mouseSensitivity += Input.GetAxis("Mouse ScrollWheel")*10;
        look();
        
    }

    float xRot_camera = 0f;
    public float mouseSensitivity;
	
    private void groundLogic()
    {
        grounded = Physics.CheckSphere(groundCheck.transform.position, .11f, ~(1 << LayerMask.NameToLayer("Player")));
    }
    private void look()
    {  

        float x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        float y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;

        
        if (!GetComponent<PlayerUse>().heldObject) 
        {
            xRot_camera -= y;
            xRot_camera = Mathf.Clamp(xRot_camera, -90, 90);
        }
        else
        {
            xRot_camera -= y;
            xRot_camera = Mathf.Clamp(xRot_camera, -40, 40);
        }
        viewCamera.transform.localRotation = Quaternion.Euler(xRot_camera, 0, 0);
        transform.Rotate(Vector3.up * x);

    }

    public float jumpHeight;
    public float jumpMultiplier;

    private float height;
    private float crouchHeight;
    private float groundCheckHeight;
    private float crouchgroundCheckHeight;
    private float crouchgroundSpeed = 3;
    private bool grounded;

    public int bHopExecutionerTimer, bHopExecutionerTimerMax = 50;
    public int DashExecuteTimer, DashExecuteTimerMax = 200;
    public int DashTimer, DashTimerMax = 200;
    private bool Dashed;
    public ParticleSystem dashEffect;


    public float moveSpeed_current;
    public float moveSpeed_walk;
    public float moveSpeed_sprint;
    public float moveSpeed_crouch;
    public float moveSpeed_dashMultiplier;
    public float moveSpeedCurrent;
    public float SlideTimer;

    private Vector3 moveDirection;

    private void move()
    {
        Vector3 newGC = groundCheck.transform.localPosition;
		//The player could slide, this was a way to get under objects quickly. It only happens after a certain speed is hit.
        if (SlideTimer != 0)
        {
            SlideTimer--;
        }
        if (Input.GetButton("Crouch")) 
        {
                moveSpeed_current = moveSpeed_crouch;
                cc.height = Mathf.Lerp(cc.height, crouchHeight, 20 * Time.fixedDeltaTime) ;
                newGC.y = Mathf.Lerp(newGC.y, crouchgroundCheckHeight, crouchgroundSpeed);
                viewCamera.transform.parent.transform.localScale = new Vector3(1, .5f, 1);
            //test

            float currentIntertia = (cc.velocity.x + cc.velocity.z) / 4;
            if (currentIntertia > 1.5f)
            {
                if (SlideTimer == 0)
                {
                    moveSpeed_current = moveSpeed_sprint * 3;
                    SlideTimer = 1000;
                }
            }
            //testend
        }
        else if (Input.GetButton("Sprint"))
        {
            if (!Physics.CheckSphere(headCheck.transform.position, .5f, ~(1 << LayerMask.NameToLayer("Player"))))
            {
                moveSpeed_current = moveSpeed_sprint;
                cc.height = Mathf.Lerp(cc.height, height, 20 * Time.fixedDeltaTime);
                newGC.y = Mathf.Lerp(newGC.y, groundCheckHeight, crouchgroundSpeed);
                viewCamera.transform.parent.transform.localScale = new Vector3(1, 1f, 1);
            }
            else if (Mathf.RoundToInt(cc.height) != height)
            {
                moveSpeed_current = moveSpeed_crouch;
                cc.height = Mathf.Lerp(cc.height, cc.height, 20 * Time.fixedDeltaTime);
                newGC.y = Mathf.Lerp(newGC.y, newGC.y, crouchgroundSpeed);
                viewCamera.transform.parent.transform.localScale = new Vector3(1, .5f, 1);
            }
        }
        else 
        {
            if (!Physics.CheckSphere(headCheck.transform.position, .5f, ~(1 << LayerMask.NameToLayer("Player"))))
            {
                moveSpeed_current = moveSpeed_walk;
                cc.height = Mathf.Lerp(cc.height, height, 20 * Time.fixedDeltaTime);
                newGC.y = Mathf.Lerp(newGC.y, groundCheckHeight, crouchgroundSpeed);
                viewCamera.transform.parent.transform.localScale = new Vector3(1, 1f, 1);
            }
            else if (Mathf.RoundToInt(cc.height) != height)
            {
                moveSpeed_current = moveSpeed_crouch;
                cc.height = Mathf.Lerp(cc.height, cc.height, 20 * Time.fixedDeltaTime);
                newGC.y = Mathf.Lerp(newGC.y, newGC.y, crouchgroundSpeed);
                viewCamera.transform.parent.transform.localScale = new Vector3(1, .5f, 1);
            }
            
        }

        groundCheck.transform.localPosition = Vector3.Lerp(groundCheck.transform.localPosition, newGC, 20 * Time.fixedDeltaTime);

        if (cc.velocity.x != 0 || cc.velocity.z != 0)
        {
            moveSpeedCurrent = Mathf.Lerp(moveSpeedCurrent, moveSpeed_current, 2 * Time.fixedDeltaTime);
        }
        if (cc.velocity.y != 0)
        {
            if (!cc.isGrounded)
            {
                moveSpeedCurrent = Mathf.Lerp(moveSpeedCurrent, .005f, 4 * Time.fixedDeltaTime);
            }
            
        }
        if (cc.velocity.x == 0 && cc.velocity.z == 0)
        {
            moveSpeedCurrent = Mathf.Lerp(moveSpeedCurrent, .005f, 4 * Time.fixedDeltaTime);
        }

        if (DashExecuteTimer == 0)
        {
            if (Input.GetButton("Dash") && !GetComponent<PlayerUse>().heldObject)
            {
                Dashed = true;
                DashExecuteTimer = DashExecuteTimerMax;
            }
        }
        else DashExecuteTimer--;
        if (Dashed)
        {
            if (DashTimer == 0)
            {
                DashTimer = DashTimerMax;
                jumpMultiplier = moveSpeed_dashMultiplier;
                dashEffect.Play();
            }
            else
            {
                if (DashTimer == 1)
                {
                    Dashed = false;
                    dashEffect.Stop();
                }
                DashTimer--;
            }
            
        }
        else
        {
			//I added bhop cancelation at some point but ended up encouraging movement mastery and added a initial sprint jump to get the movement going.
            if (Input.GetButton("Jump"))
            {
                if (bHopExecutionerTimer == 0 && Input.GetAxis("Vertical") > 0 && Input.GetButton("Sprint"))
                {
                    jumpMultiplier = 3;
                    bHopExecutionerTimer = bHopExecutionerTimerMax;
                }
            }
            else
            {
                if (bHopExecutionerTimer != 0)
                {
                    bHopExecutionerTimer--;
                }
                jumpMultiplier = 1;
            }
        }
        
        moveDirection.x = Input.GetAxis("Horizontal") * moveSpeedCurrent * jumpMultiplier;
        moveDirection.y = jump();
        moveDirection.z = Input.GetAxis("Vertical") * moveSpeedCurrent * jumpMultiplier;

        cc.Move(transform.TransformDirection(moveDirection));
    }
	
	//The player could dash. It was different from the bhop sprint but with a dash ability you could cancel gravity and head in a specific direction regardless of velocity.
    public bool checkAbilitiesDash { get { return Dashed; } }
    private float jump()
    {
        if (grounded || cc.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                return jumpHeight;
            }
            else { return 0; }
        }
        else
        {
            return Mathf.Lerp(moveDirection.y, Physics.gravity.y, .035f * Time.deltaTime);
        }
    }
}
