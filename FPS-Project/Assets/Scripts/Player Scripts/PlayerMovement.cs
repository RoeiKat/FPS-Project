using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6f;
    public float normalSpeed = 6f;
    public float sprintSpeed = 12f;
    public bool canSprint = true;
    public bool isSprinting = false;
    public float gravity = -19.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Animator weaponController;
    ActiveWeapon activeWeapon;

    AudioSource sfx;
    public AudioClip[] audioClips;

    bool isGrounded;
    Vector3 velocity;

    void Start()
    {
        sfx = GetComponent<AudioSource>();
        activeWeapon = GetComponent<ActiveWeapon>();
        AudioListener.pause = false;
        sfx.clip = audioClips[0];
        sfx.enabled = false;
    }

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
    
        if(Input.GetKey(KeyCode.W) && isGrounded || Input.GetKey(KeyCode.A) && isGrounded || Input.GetKey(KeyCode.S) && isGrounded || Input.GetKey(KeyCode.D) && isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                sfx.clip = audioClips[1];
                sfx.Play();
            } 
            if(Input.GetKeyUp(KeyCode.LeftShift)){
                sfx.clip = audioClips[0];
                sfx.Play();
            }
            sfx.enabled = true;
        } else 
        {
            sfx.enabled = false;
        }

        // Jumping code
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Running code
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && canSprint)
        {
            speed = sprintSpeed;
            StartCoroutine(sprintingHandler());
        }
        else
        {
            isSprinting = false;
            speed = normalSpeed;
            weaponController.SetBool("isSprinting", false);
        }

        IEnumerator sprintingHandler()
        {
        isSprinting = true;
        activeWeapon.canShoot = false;
        weaponController.SetBool("isSprinting", true);
        yield return new WaitForSeconds(0.5f);
        activeWeapon.canShoot = true;
        }
        // Gravity applier
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    
    }
}
