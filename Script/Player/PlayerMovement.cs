using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEditor.Callbacks;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] public float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    [SerializeField] float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    public float pStamina,maxStamina = 100;
    public float dValue,dValueJump;
    public float increaseCooldownTime,nextIncreaseTime;
    public Slider slider;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public float fallJumpForce = 2.5f;

    /*[Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    public bool isCrouching { get; private set; }*/

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    //[SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    public bool moving,runing;

    Rigidbody rb;

    RaycastHit slopeHit;

    public TextMeshProUGUI texts;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //startYScale = transform.localScale.y;
        //isCrouching = false;
        slider.maxValue = maxStamina;
        pStamina = maxStamina;
        slider.value = pStamina;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        ControlSpeed();
        speedControl();
        increaseStamina();
        
        if (Input.GetKeyDown(jumpKey) && isGrounded && pStamina > 0)
        {
            Jump();pStamina -= dValueJump;
        }
        if(rb.velocity.y < 0 && !isGrounded){
                rb.velocity += Vector3.up * Physics.gravity.y * (fallJumpForce - 1 ) * Time.deltaTime;
            }

        /*if(Input.GetKeyDown(crouchKey) && isGrounded){
            Crouch();
        }
        if(Input.GetKeyUp(crouchKey) && isGrounded){
            stopCrouch();
        }*/

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        float currentSpeed = rb.velocity.magnitude;
        texts.SetText(currentSpeed + "");
    }
    
    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void Jump()
    {
        if (isGrounded && pStamina > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    /*void Crouch(){
        isCrouching = true;
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale,transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }*/
    /*void stopCrouch(){
        isCrouching = false;
        transform.localScale = new Vector3(transform.localScale.x, startYScale,transform.localScale.z);
    }*/

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded && pStamina > 0)
        {
            runing = true;
            pStamina -= dValue * Time.deltaTime;
            slider.value = pStamina;
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            runing = false;
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }

        //if(Input.GetKey(crouchKey) && isGrounded && isCrouching)
        //{
        //    moveSpeed = Mathf.Lerp(moveSpeed, crouchSpeed, acceleration * Time.deltaTime);
        //}
        //else 
        //{
        //     moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        //}       
    }

    void speedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x, 0,rb.velocity.z);
        if(flatVel.magnitude > moveSpeed){
            Vector3 LimitedVel = flatVel.normalized * moveSpeed;
            rb.velocity =  new Vector3(LimitedVel.x, rb.velocity.y,LimitedVel.z);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            Vector3 jumpSpeed = new Vector3(rb.velocity.x, 0,rb.velocity.z);
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }


    void increaseStamina()
    {
        if(rb.velocity.magnitude < 5 && pStamina <= 100){
            pStamina += dValue * Time.deltaTime;
            slider.value = pStamina;
        }
    }
}