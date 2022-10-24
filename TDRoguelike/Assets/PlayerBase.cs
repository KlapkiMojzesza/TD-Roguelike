using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerBase : MonoBehaviour
{
    [SerializeField] float baseHealth = 100;
    [SerializeField] TMP_Text damageText;

    private void Start()
    {
        damageText.text = baseHealth.ToString();
    }

    public void TakeDamage(float damage)
    {
        baseHealth -= damage;
        damageText.text = baseHealth.ToString();
    }
}
