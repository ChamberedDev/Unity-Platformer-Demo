using UnityEngine;

public class CameraMovementFollowPlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool TrackingMode;
    [SerializeField] private bool TrackAhead;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float CameraSpeed;
    private float lookAhead;
    private float currentPositionX;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform trackingPlayer;

    private void Update()
    {
        if (TrackingMode == true) // Player Tracking Camera
        {
            if (TrackAhead == true) // Ahead of Player Camera
            {
                transform.position = new Vector3(trackingPlayer.position.x + lookAhead, transform.position.y, transform.position.z); 
                lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * trackingPlayer.localScale.x), Time.deltaTime * CameraSpeed); //
            }
            else // Directly on Player
            {
                transform.position = new Vector3(trackingPlayer.position.x, transform.position.y, transform.position.z); 
            }
        }

        else // Room Camera
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPositionX, transform.position.y, transform.position.z), ref velocity, speed);
        }
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPositionX = _newRoom.position.x;
    }
}
