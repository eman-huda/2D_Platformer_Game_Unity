using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField] public float startingHealth;
    public float currentHealth;
    private Animator anim;
    private bool dead;
    public GameObject panel;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            StartCoroutine(HandleHurt());
        }
        else
        {
            if (!dead)
            {
                dead = true;
                StartCoroutine(HandleDeath());
            }
        }
    }

    private IEnumerator HandleHurt()
    {
        anim.SetTrigger("hurt");

        PlayerMovement playerMove = GetComponent<PlayerMovement>();
        if (playerMove != null)
        {
            playerMove.canMove = false;
        }
        yield return new WaitForSeconds(0.8f);

        if (playerMove != null)
        {
            playerMove.canMove = true;
        }
        GetComponent<PlayerMovement>().deactivateSelf();
    }

    private IEnumerator HandleDeath()
    {
        PlayerMovement playerMove = GetComponent<PlayerMovement>();
        playerMove.Death();
        if (playerMove != null)
        {
            playerMove.enabled = false;
        }

       
        yield return new WaitForSeconds(1f);

        Time.timeScale = 0f;
        panel.SetActive(true);
    }

    public void AddHealth(float val)
    {
        currentHealth = Mathf.Clamp(currentHealth + val, 0, startingHealth);
    }
}
