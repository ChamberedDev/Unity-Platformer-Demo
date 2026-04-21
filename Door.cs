using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraMovement cam; // Alter between CameraMovementRoom and CameraMovementFollowPlayer, depending on what you using

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x) // If true, player is moving towards the right, camera will move to next room
            {
                cam.MoveToNewRoom(nextRoom);
            }
            else // If false, camera goes back to previous room (think about it like moving on x axis -1 and +1)
            {
                cam.MoveToNewRoom(previousRoom);
            }
        }
    }
}
