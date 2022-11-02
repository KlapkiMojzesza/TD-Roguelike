using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int towerPrize = 10;

    [Header("To Attach")]
    [SerializeField] Renderer renderer;
    [SerializeField] GameObject towerHitBox;

    Color orginalColor;
    bool canBePlaced = true;
    Material myMaterial;

    private void Awake()
    {
        myMaterial = renderer.material;
        orginalColor = myMaterial.color;
    }

    public int GetTowerPrize()
    {
        return towerPrize;
    }

    public bool CanBePlaced()
    {
        return canBePlaced;
    }

    public void SetTowerColor()
    {
        Color color = CanBePlaced() ? Color.green : Color.red;
        myMaterial.color = color;
    }

    public void SetOrginalColor()
    {
        myMaterial.color = orginalColor;
    }

    public void PlaceTower()
    {
        towerHitBox.SetActive(true);
    }

    private void OnCollisionStay(Collision collision)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            canBePlaced = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            canBePlaced = true;
        }
    }
}
