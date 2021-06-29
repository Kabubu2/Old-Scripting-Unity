using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
	
	//A makeshift movement controller
	
    private Rigidbody rb;
    public GameObject groundCheck;
    public float jumpForce;
    public Camera viewCamera;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private Vector3 moveDirection;
    public float moveSpeed;

    private void FixedUpdate()
    {
        move();
    }
    private void Update()
    {
        
        jump();
        look();
    }
    float xRot_camera = 0f;
    public float mouseSensitivity;

    private void look()
    {
        float x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRot_camera -= y;
        xRot_camera = Mathf.Clamp(xRot_camera, -90, 90);

        viewCamera.transform.localRotation = Quaternion.Euler(xRot_camera, 0, 0);
        transform.Rotate(Vector3.up * x);

    }
    private void move()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.MovePosition(transform.position + Time.fixedDeltaTime * moveSpeed * transform.TransformDirection(moveDirection));
    }
    private void jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (Physics.CheckSphere(groundCheck.transform.position, .1f, ~(1 << LayerMask.NameToLayer("Player"))))
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
