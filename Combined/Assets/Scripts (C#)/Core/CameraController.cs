using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Room camera
    [SerializeField] private float speed;
    [SerializeField] private GameObject[] rooms;
    [SerializeField] private Player player;
    private GameObject currentRoom;
    private Vector3 velocity = Vector3.zero;

    private void Update() {
        for (int i = 0; i < rooms.Length; i++)
        {
            float roomPositionX = rooms[i].transform.position.x;
            float roomPositionY = rooms[i].transform.position.y;
            float playerX = player.transform.position.x;
            float playerY = player.transform.position.y;
            // check if player is within the room bounds
            if ((playerX > roomPositionX - 15 && playerX < roomPositionX + 15) && //total width is 30
                (playerY > roomPositionY - 10 && playerY < roomPositionY + 10)) //total height is 20
            {
                currentRoom = rooms[i];
            }
        }

        transform.position = Vector3.SmoothDamp(
            transform.position, 
            new Vector3(currentRoom.transform.position.x, currentRoom.transform.position.y, transform.position.z), 
            ref velocity, 
            speed);
    }
}
