using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D body;
    public float playerSpeed;
    public float jumpSpeed;
    public LayerMask groundlayer;
    public Animator animator;
    private float horizontalInput;
    public bool canMove=true;
    public Transform currentRespawnPoint;

    private BoxCollider2D boxcollider;
    private float highestYPosition;
    public float dizzyFallDistance = -0.4f; // Minimum distance to trigger dizzy
    private bool isFalling = false;
    private bool isDizzy = false;
    private bool jumpInitiated = false;
    public float difference = 0f;

    //wall jump
    public float wallJumpForce = 8f;
    public float wallSlideSpeed = 2f;
    public LayerMask wallLayer;
    public bool isWall = false;
    private float wallDirectionX; 

    //sound effect
    public AudioClip jumpClip;
    public AudioSource audioSource;

    //cooldown timer
    private float trapDamageCooldown = 1.0f; 
    private float lastTrapDamageTime = -Mathf.Infinity;
    //level completed
    public GameObject panel;

    //Joystick
    public Joystick joystick;

    void Start()
    {
        //currentRespawnPoint = transform; // Default: current starting position

        boxcollider = GetComponent<BoxCollider2D>();
        body=GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    bool IsTouchingWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxcollider.bounds.center, boxcollider.bounds.size, 0, Vector2.right*transform.localScale.x, 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxcollider.bounds.center, boxcollider.bounds.size, 0, Vector2.down, 0.1f, groundlayer);
        return raycastHit.collider != null;
    }
    void Jump()
    {
        if (!isGrounded()) return;
        audioSource.PlayOneShot(jumpClip);
        jumpInitiated = true;
        highestYPosition = transform.position.y;
        body.velocity = new Vector3(body.velocity.x, jumpSpeed);
       
    }
    // Update is called once per frame
    void Update()
    {
        if (isDizzy) return; // disable input while dizzy
        if (!canMove) return;
        //horizontalInput = joystick.Horizontal;
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity=new Vector2(horizontalInput * playerSpeed, body.velocity.y);

        if (Input.GetKey("space") && isGrounded())
        {
            Jump();
        }
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        animator.SetBool("walk",horizontalInput!=0);
        animator.SetBool("jump",!isGrounded());
        // CROUCH LOGIC
        if (Input.GetKey(KeyCode.C))
        {
            animator.SetBool("crouch", true);
            playerSpeed = 2f;
        }
        else
        {
            animator.SetBool("crouch", false);
            playerSpeed = 5f;
        }
        //FLYING KICK LOGIC
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isGrounded())
        {
            Debug.Log("Fly kick enabled");
            animator.SetTrigger("flykick");

        }
        //DIZZY LOGIC
        if (!isGrounded() && jumpInitiated)
        {
            if (!isFalling)
            {
                isFalling = true;
                highestYPosition = transform.position.y;
            }

            if (transform.position.y > highestYPosition)
            {
                highestYPosition = transform.position.y;
            }
           
            
        }

        // Landing
        if (isGrounded() && isFalling)
        {
            float fallDistance = highestYPosition - transform.position.y;
            difference = fallDistance;
            if (fallDistance > dizzyFallDistance)
            {
                StartCoroutine(TriggerDizzy());
            }

            // Reset tracking
            isFalling = false;
            jumpInitiated = false;
        }
        //CLIMB LOGIC
        isWall = IsTouchingWall();
        if (Input.GetKeyDown(KeyCode.Space) && isWall && !isGrounded())
        {
            WallJump();
        }
        //Player Rotation Reset
        if (Input.GetKey(KeyCode.P))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }
    IEnumerator TriggerDizzy()
    {
        isDizzy = true;
        canMove = false;
        animator.SetTrigger("dizzy");

        yield return new WaitForSeconds(1.5f);

        canMove = true;
        isDizzy = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("traps"))
        {
            // Check if enough time has passed since last trap damage
            if (Time.time >= lastTrapDamageTime + trapDamageCooldown)
            {
                GetComponent<Health>().TakeDamage(1);
                lastTrapDamageTime = Time.time; // reset the cooldown timer
            }
        }
        if (collision.gameObject.CompareTag("movingplatform"))
        {
            // Make the player a child of the platform so it moves together
            transform.SetParent(collision.transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // When player leaves the moving platform
        if (collision.gameObject.CompareTag("movingplatform"))
        {
            transform.SetParent(null);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("endGame"))
        {
            panel.SetActive(true);
            GameManager.Instance.SaveHighScore();
            GameManager.Instance.UpdateCollected();
            
            
            GameManager.Instance.LevelSelector_UnlockNextLevel();

            Time.timeScale = 0f;
        }
        if (collision.CompareTag("Checkpoint"))
        {
            GameManager.Instance.UpdateCollected();
            currentRespawnPoint = collision.transform;
            Debug.Log("Checkpoint reached: " + collision.name);
        }

    }

    void WallJump()
    {
        body.velocity = new Vector2(-wallDirectionX * wallJumpForce, wallJumpForce);

       // transform.localScale = new Vector3(-wallDirectionX, 1, 1);
    }

    public void Death()
    {
        animator.ResetTrigger("reset");
        animator.SetTrigger("death");
        body.isKinematic = true;
        canMove = false;
    }
    public void deactivateSelf()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(Respawn(1f));
    }

    public IEnumerator Respawn(float timeDelay) {
        yield return new WaitForSeconds(timeDelay);

        transform.position = currentRespawnPoint.position;

        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localScale= Vector3.one;

        body.isKinematic = false;
        //body.constraints = RigidbodyConstraints2D.FreezeAll;
        body.velocity = Vector3.zero;
        animator.SetTrigger("reset");
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        canMove = true;
    }
    public bool canAttack()
    {
        return  horizontalInput==0;
    }
    // --------------------
    // Mobile Button Methods
    // --------------------

    // Call this from your Jump button
    public void OnJumpButton()
    {
        if (isGrounded() && canMove)
            Jump();
    }

    // For crouch button: call this on PointerDown
    public void OnCrouchButtonDown()
    {
        animator.SetBool("crouch", true);
        playerSpeed = 5f;
    }

    public void OnCrouchButtonUp()
    {
        animator.SetBool("crouch", false);
        playerSpeed = 5f;
    }

    // For fire/attack button
}
