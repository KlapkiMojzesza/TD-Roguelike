using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerBase : MonoBehaviour
{
    [SerializeField] float startBaseHealth = 100;
    [SerializeField] TMP_Text damageText;

    float baseHealth;
    public static event Action OnBaseDestroyed;

    private void Start()
    {
        baseHealth = startBaseHealth;
        damageText.text = baseHealth.ToString();
    }

    public void TakeDamage(float damage)
    {
        baseHealth -= damage;
        if (baseHealth <= 0)
        {
            baseHealth = 0f;
            DestroyPlayerBase();
        }

        damageText.text = baseHealth.ToString();
    }

    private void DestroyPlayerBase()
    {
        OnBaseDestroyed?.Invoke();
        baseHealth = startBaseHealth;
        damageText.text = baseHealth.ToString();
    }
}
