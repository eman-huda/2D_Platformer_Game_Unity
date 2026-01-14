using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Collectable : MonoBehaviour
{
   
    public AudioClip collectSound;
    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private int CalcScoreValue()
    {
        if (CompareTag("banana"))
        {
            return 1;
        }
        if (CompareTag("apple"))
        {
            return 5;
        }
        if (CompareTag("stawberry"))
        {
            return 10;
        }
        if (CompareTag("cherry"))
        {
            return 20;
        }
        if (CompareTag("melon"))
        {
            return 50;
        }
        return 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Play sound and animation
            if (audioSource != null && collectSound != null)
                audioSource.PlayOneShot(collectSound);

            if (animator != null)
                animator.SetTrigger("collected");

            // Add to global score
            GameManager.Instance.AddScore(CalcScoreValue());


            // Disable collider and hide object after short delay
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(DisableAfterAnimation());
        }
    }
   
    private System.Collections.IEnumerator DisableAfterAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
