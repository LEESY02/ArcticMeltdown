using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Room : MonoBehaviour
{
    [SerializeField] public GameObject startingObject;
    [SerializeField] public GameObject previousRoomDoor;
    [SerializeField] public float durationToGoBack;
    private float time;
    public bool visited {get; private set;}

    private void Awake() {
        visited = false;
        time = 0;
    }

    private void Update() {
        if (visited) {
            time += Time.deltaTime;
            if (time > durationToGoBack) previousRoomDoor.GetComponent<Collider2D>().isTrigger = false;
        }
    }

    public void Visit() {
        visited = true;
    }
}
