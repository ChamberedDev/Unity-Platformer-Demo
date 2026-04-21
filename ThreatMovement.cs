using UnityEngine;

public class Threat_Movement : MonoBehaviour
{
    // Combined code Threat_Horizontal.cs and Threat_Vertical.cs, promoting convenience in switching between horizontal threats and vertical threats through Inspector.
    [SerializeField] private float Damage;
    [SerializeField] private float speed;
    [SerializeField] private float MovementDistance;
    [SerializeField] private bool Horizontal; // Switching from Horizontal to Vertical threat
    private bool MovingLeft;
    private bool MovingUp;
    private float LeftEdge;
    private float RightEdge;
    private float TallestEdge;
    private float ShortestEdge;


    private void Awake()
    {
        TallestEdge = transform.position.y - MovementDistance; // Refers to how far up is the threat going to be
        ShortestEdge = transform.position.y + MovementDistance; // Refers to how far down is the threat going to be
        LeftEdge = transform.position.x - MovementDistance; // Refers to how far left is the threat going to be
        RightEdge = transform.position.x + MovementDistance; // Refers to how far right is the threat going to be
    }

    private void Update()
    {
        if(Horizontal)
        {
            if(MovingLeft)
            {
                if(transform.position.x > LeftEdge)
                {
                    transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
                }
                else
                {
                    MovingLeft = false;
                }
            }
            else 
            {
                if(transform.position.x < RightEdge)
                {
                    transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
                }
                else
                {
                    MovingLeft = true;
                }
            }
        }
        else
        {
            if(MovingUp)
            {
                if(transform.position.y > TallestEdge)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
                }
                else
                {
                    MovingUp = false;
                }
            }
            else 
            {
                if(transform.position.y < ShortestEdge)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
                }
                else
                {
                    MovingUp = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(Damage);
        }
    }
}
