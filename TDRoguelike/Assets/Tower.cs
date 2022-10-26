using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int towerPrize = 10;

    [SerializeField] bool canBePlaced = true;

    public int GetTowerPrize()
    {
        return towerPrize;
    }

    public bool CanBePlaced()
    {
        return canBePlaced;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {   
            canBePlaced = false;
        } 
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            canBePlaced = true;
        }
    }
}
