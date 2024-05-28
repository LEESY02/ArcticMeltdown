using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header ("Rooms")]
    [SerializeField] private GameObject topRoom;
    [SerializeField] private GameObject bottomRoom;
    [SerializeField] private GameObject leftRoom;
    [SerializeField] private GameObject rightRoom;
    [Header ("Extras")]
    [SerializeField] private CameraController cam;
    [SerializeField] private GameObject player;
    private GameObject roomToPan;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (topRoom == null) { // vertical door
                if (collision.transform.position.x < transform.position.x) {
                    roomToPan = rightRoom;
                } else if (collision.transform.position.x > transform.position.x) {
                    roomToPan = leftRoom;
                }
            } else if (leftRoom == null) { // horizontal door
                if (collision.transform.position.y < transform.position.y) {
                    roomToPan = topRoom;
                } else if (collision.transform.position.y >  transform.position.y) {
                    roomToPan = bottomRoom;
            }
        }          
            cam.MoveToNewRoom(roomToPan.transform);
            cam.SetRoom(roomToPan);
            roomToPan.GetComponent<Room>().Visit();
            //while(collision != null) GetComponent<BoxCollider2D>().isTrigger = true;
            //GetComponent<BoxCollider2D>().isTrigger = false;
            // } else {
            //     cam.MoveToNewRoom(previousRoom.transform);
            // }
        }
    }
}

