using UnityEngine;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; } // Get variable from any other script, private makes it only set in this script
    private Animator animator;
    private bool Dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if(currentHealth > 0)
        {
            // Player is Hurt
            animator.SetTrigger("Hurt");

            //iframes
        }
        else
        {
            // Player is Dead

            if(!Dead)
            {
                animator.SetTrigger("Die");
                GetComponent<PlayerMovement>().enabled = false; // EXPLAIN
                Dead = true;
            }
        }
    }
}
