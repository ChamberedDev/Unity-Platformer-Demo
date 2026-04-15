using UnityEngine;

public class CameraMovementFollowPlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    private float currentPositionX;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform trackingPlayer;

    private void Update()
    {
        transform.position = new Vector3(trackingPlayer.position.x, transform.position.y, transform.position.z);   
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPositionX = _newRoom.position.x;
    }
}
