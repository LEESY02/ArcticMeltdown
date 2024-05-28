using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHealthbar;
    [SerializeField] private Image currentHealthBar;

    private void Start() {
        totalHealthbar.fillAmount = playerHealth.GetStartingHealth() / 10;
    }

    private void Update() {
        currentHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }

}
