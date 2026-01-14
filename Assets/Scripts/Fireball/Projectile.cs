using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float movementSpeed = 10f;
    public Animator animator;
    public float direction;
    public AudioClip explosionClip;
    public AudioSource audioSource;
    // Start is called before the first frame update
    public float hitCooldown = 1f;       // 1 second between hits
    public float lastHitTime = -10f;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        movementSpeed = 10f;
        // Reset rotation & velocity so it always fires straight
        transform.rotation = Quaternion.identity;

        // If Rigidbody2D is attached, reset velocity
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movementSpeed * Time.deltaTime * direction, 0f, 0f);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // play explosion and animation
        audioSource.PlayOneShot(explosionClip);
        animator.SetTrigger("explode");

        // prevent further movement
        movementSpeed = 0f;

        // DAMAGE ENEMY
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Animator>().SetTrigger("die");
            collision.gameObject.SetActive(false);
        }

        // DAMAGE PLAYER (with cooldown) 
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastHitTime > hitCooldown)
            {
                lastHitTime = Time.time;

                Health playerHealth = collision.gameObject.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(1);
                }
            }
        }

        // disable after explosion
        StartCoroutine(DisableAfterExplosion());
    }


    IEnumerator DisableAfterExplosion()
    {
        yield return new WaitForSeconds(1f); // Wait for explosion animation
        gameObject.SetActive(false);
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y,
            transform.localScale.z);
    }
}