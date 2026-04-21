using UnityEngine; // Gives you access to Unity's built-in classes
using UnityEngine.InputSystem; // Opens a separate, specialized toolbox for input (Differs from Old system as Unity simplifies Input settings)

public class PlayerMovement : MonoBehaviour
{
    // --- Inspector-tunable fields ---
    [SerializeField] private float Speed;                              // Horizontal movement speed
    [SerializeField] private float JumpPower;                          // Upward force applied on jump
    [SerializeField] private float WallJumpHorizontalForce;            // Push-off force when wall jumping with no directional input
    [SerializeField] private float WallJumpHorizontalWithInputForce;   // Reduced push-off when holding a direction during wall jump
    [SerializeField] private float WallJumpVerticalForce;              // Upward force on wall jump
    [SerializeField] private LayerMask GroundLayer;                    // Layer(s) the player can stand on
    [SerializeField] private LayerMask WallLayer;                      // Layer(s) the player can wall jump off

    // --- Private runtime state ---
    private Rigidbody2D Body;
    private Animator Animator;
    private BoxCollider2D BoxCollider;
    private float WallJumpTimer;  // Counts up from 0 after a wall jump; movement is suppressed while < 0.2f to prevent immediately re-grabbing the wall
    private float MoveInput;      // -1 (left), 0 (idle), or 1 (right) each frame

    private void Awake()
    {
        // Cache component references once at startup instead of calling GetComponent every frame
        Body = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        BoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // --- Horizontal input ---
        // Read A/D keys and convert to a -1/0/1 axis value
        MoveInput = Keyboard.current.dKey.isPressed ?  1f :
                    Keyboard.current.aKey.isPressed ? -1f :
                    0f;

        // Flip the sprite to face the direction of travel by scaling X negatively
        if (MoveInput > 0f)
            transform.localScale = Vector3.one;              // Facing right — default scale
        else if (MoveInput < 0f)
            transform.localScale = new Vector3(-1, 1, 1);   // Facing left — mirror on X axis

        // --- Animator sync ---
        Animator.SetBool("Running", MoveInput != 0);
        Animator.SetBool("Grounded", IsGrounded());

        // --- Movement & wall logic ---
        // After a wall jump, suppress normal movement briefly so the player
        // doesn't immediately stick back to the wall they just jumped from
        if (WallJumpTimer > 0.2f)
        {
            // Apply horizontal velocity while preserving current vertical velocity (gravity/jump arc)
            Body.linearVelocity = new Vector2(MoveInput * Speed, Body.linearVelocity.y);

            if (OnWall() && !IsGrounded())
            {
                // Disable gravity while touching a wall in the air so the player "sticks" briefly
                Body.gravityScale = 0;
                Body.linearVelocity = Vector2.zero; // Zero velocity to hold position against the wall
            }
            else
            {
                // Restore gravity when not wall-sliding
                Body.gravityScale = 6;
            }

            if (Keyboard.current.spaceKey.isPressed)
                Jump();
        }
        else
        {
            // Tick the post-wall-jump suppression timer up toward the 0.2f threshold
            WallJumpTimer += Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            // Standard ground jump — preserve horizontal velocity, apply full jump power upward
            Body.linearVelocity = new Vector2(Body.linearVelocity.x, JumpPower);
            Animator.SetTrigger("Jump"); // One-shot trigger; Animator resets it automatically
        }
        else if (OnWall())
        {
            // Wall jump — direction depends on whether the player is holding a key
            if (MoveInput == 0)
            {
                // No directional input: pop straight off the wall horizontally with no upward arc
                Body.linearVelocity = new Vector2(
                    -Mathf.Sign(transform.localScale.x) * WallJumpHorizontalForce, 0);

                // Mathf.Sign returns +1 or -1 based on which way the player faces;
                // negating it pushes the player away from the wall they are on
                transform.localScale = new Vector3(
                    -Mathf.Sign(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z);
            }
            else
            {
                // Directional input held: smaller horizontal push + upward arc for a climbing feel
                Body.linearVelocity = new Vector2(
                    -Mathf.Sign(transform.localScale.x) * WallJumpHorizontalWithInputForce,
                    WallJumpVerticalForce);
            }

            // Reset timer so movement suppression kicks in immediately after the wall jump
            WallJumpTimer = 0;
        }
    }

    // Returns true if a downward BoxCast from the player's collider hits the ground layer
    private bool IsGrounded()
    {
        RaycastHit2D Hit = Physics2D.BoxCast(
            BoxCollider.bounds.center,  // Cast origin — centre of the collider
            BoxCollider.bounds.size,    // Cast size — matches the collider exactly
            0,                          // No rotation
            Vector2.down,               // Cast downward
            0.1f,                       // Short distance — just enough to detect touching the floor
            GroundLayer);               // Only register hits on the ground layer

        return Hit.collider != null;
    }

    // Returns true if a sideways BoxCast (in the direction the player faces) hits the wall layer
    private bool OnWall()
    {
        RaycastHit2D Hit = Physics2D.BoxCast(
            BoxCollider.bounds.center,
            BoxCollider.bounds.size,
            0,
            new Vector2(transform.localScale.x, 0), // Cast in the direction the player is facing
            0.1f,
            WallLayer);

        return Hit.collider != null;
    }

    // Called by PlayerAttack — attack is only allowed when standing still on the ground
    public bool CanAttack()
    {
        return MoveInput == 0 && IsGrounded() && !OnWall();
    }
}