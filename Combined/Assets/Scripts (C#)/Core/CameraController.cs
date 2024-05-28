using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Room camera
    [SerializeField] private float speed;
    [SerializeField] private GameObject room; // {get; private set;}
    //private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    // // Follow player
    // [SerializeField] private Transform player;
    // [SerializeField] private float aheadDistance;
    // [SerializeField] private float cameraSpeed;
    // private float lookAhead;

    private void Update() {
        // Room camera
        //if (GameObject.FindGameObjectWithTag("Player").transform.position.x > GameObject.FindGameObjectWithTag("Door").transform.position.x) {
            // transform.position = Vector3.SmoothDamp(
            // transform.position, 
            // new Vector3(currentPosX, transform.position.y, transform.position.z), 
            // ref velocity, 
            // speed); 

            transform.position = Vector3.SmoothDamp(
            transform.position, 
            new Vector3(room.transform.position.x , room.transform.position.y, transform.position.z), 
            ref velocity, 
            speed);

            // transform.position = new Vector3(
            //     room.transform.position.x, 
            //     room.transform.position.y,
            //     transform.position.z);

        //  }
              
        // // Follow player
        // transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        // lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    public void MoveToNewRoom(Transform newRoom) {
        //currentPosX = newRoom.position.x;
        SetRoom(newRoom.gameObject);
    }

    public GameObject GetRoom() {
        return this.room;
    }

    public void SetRoom(GameObject room) {
        this.room = room;
    }
}
