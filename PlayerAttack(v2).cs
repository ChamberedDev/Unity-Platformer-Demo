using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float AttackCooldown = 0.5f; // Minimum seconds between attacks
    [SerializeField] private Transform FirePoint;
    [SerializeField] private GameObject[] Fireballs;
    private Animator Animator;
    private PlayerMovement PlayerMovement; // Reference used to check CanAttack() before firing
    private float CooldownTimer;           // Tracks elapsed time since the last attack

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        PlayerMovement = GetComponent<PlayerMovement>();

        // Pre-fill the timer so the player can attack immediately on game start
        // rather than waiting for a full cooldown to elapse first
        CooldownTimer = AttackCooldown;
    }

    private void Update()
    {
        // Always tick the timer up, every frame, regardless of input
        CooldownTimer += Time.deltaTime;

        // Attack conditions (all must be true):
        //   wasPressedThisFrame — fires once per click, not every frame the button is held
        //   CooldownTimer >= AttackCooldown — enforces the minimum gap between attacks
        //   CanAttack() — player must be grounded, idle, and not on a wall
        if (Mouse.current.leftButton.wasPressedThisFrame
            && CooldownTimer >= AttackCooldown
            && PlayerMovement.CanAttack())
        {
            Attack();
        }
    }

    private void Attack()
    {
        Animator.SetTrigger("Attack"); // One-shot trigger; Animator resets it automatically
        CooldownTimer = 0;             // Restart the cooldown window after each attack

        // Uses the first fireball available in the array and place it at the FirePoint
        Fireballs[FindFireball()].transform.position = FirePoint.position;
        // The first fireball available in the array, to link and command the Projectile.cs script, set the direction of where the fireball travels through where the Player is facing (+1 or -1)
        Fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireball()
    {
        for (int i = 0; i < Fireballs.Length; i++)
        {
            if (!Fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }

        return 0; // If all fireballs in the array is being used (eg. 6/6 fireballs), the program returns the array back to [0] and reuses it
    }
}