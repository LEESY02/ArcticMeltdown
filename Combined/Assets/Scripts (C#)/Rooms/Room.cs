using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Room : MonoBehaviour
{
    [SerializeField] public GameObject[] doors;

    private Enemy[] enemies;
    private int noOfEnemies;
    private Transform currentRoom;
    // public bool visited {get; private set;}

    private void Start() 
    {
        // visited = false;
        currentRoom = gameObject.transform;
        enemies = FindObjectsOfType<Enemy>();
        noOfEnemies = GetNoOfEnemiesInRoom();
    }

    private void Update() 
    {
        Debug.Log(enemies.Length);
        // Debug.Log(noOfEnemies + " enemies.");
        // UpdateEnemyCount(); //constantly update how many enemies are alive in current room;
        // if (noOfEnemies == 0)
        // {
        //     DeactivateDoors();
        // }
    }

    // public void Visit() {
    //     visited = true;
    // }

    private int GetNoOfEnemiesInRoom()
    {
        int count = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (IsInRoom(enemies[i])) count++;
        }
        return count;
    }

    private bool IsInRoom(Enemy enemy)
    {
        Transform enemyPos = enemy.transform;

        return (enemyPos.position.x < currentRoom.position.x + 15 && enemyPos.position.x > currentRoom.position.x - 15)
             && (enemyPos.position.y < currentRoom.position.y + 10 && enemyPos.position.y > currentRoom.position.y - 10);
    }

    private void DeactivateDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].SetActive(false);
        }
    }

    private void UpdateEnemyCount()
    {
        
    }

}
