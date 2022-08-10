using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -19.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;
    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {   
        // Returns true or false by these parameters, the object position, its distance from the ground
        // And the mask that it checks with
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Resetting the velocity each time the player is on the ground
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        // Movement Axis by default controlled by WASD
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical");

        // Moving the actual player 
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Jumping code
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Running code
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 24f;
        } 
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 12f;
        }
        
        // Gravity applier
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
