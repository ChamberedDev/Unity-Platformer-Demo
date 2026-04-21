using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private Animator animator;
    private BoxCollider2D boxCollider;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return; // If fireball hits something

        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0); // Move object on x-axis

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) // Check if fireball hit an object
    {
        hit = true;
        boxCollider.enabled = false; // Disable box collider
        animator.SetTrigger("Explode"); // Triggers explode animation
    }

    public void SetDirection(float _direction) // Direction of fireball when fired from left/right
    {
        lifetime = 0;
        direction = _direction; // WHAT IS THIS??
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;

        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate() // WHAT IS THIS??  
    {
        gameObject.SetActive(false);
    }
}