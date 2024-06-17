using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] private Transform menuPosition;
    [SerializeField] private float durationBeforePanningUpwards;
    [SerializeField] private float panningSpeed;

    private float timer;
    private Vector3 velocity = Vector3.zero;
    private Tracker tracker;

    private void Awake()
    {
        timer = 0;
        tracker = FindObjectOfType<Tracker>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (LoadedBefore())
        {
            transform.position = new Vector3(menuPosition.position.x, menuPosition.position.y, transform.position.z);
        } 
        else if (timer > durationBeforePanningUpwards)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                new Vector3(menuPosition.position.x, menuPosition.transform.position.y, transform.position.z), 
                ref velocity, 
                panningSpeed);
        }
    }

    private bool LoadedBefore()
    {
        return tracker.loadCount > 0;
    }
}
