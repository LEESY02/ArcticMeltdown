using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject previousRoom;
    [SerializeField] private GameObject nextRoom;
    [SerializeField] private CameraController cam;
    [SerializeField] private GameObject player;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (collision.transform.position.x < transform.position.x) {
                cam.MoveToNewRoom(nextRoom.transform);
                cam.SetRoom(nextRoom);
                nextRoom.GetComponent<Room>().Visit();
                //while(collision != null) GetComponent<BoxCollider2D>().isTrigger = true;
                //GetComponent<BoxCollider2D>().isTrigger = false;
            }
            // } else {
            //     cam.MoveToNewRoom(previousRoom.transform);
            // }
        }
    }

}
