using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int towerPrize = 10;
    [SerializeField] Renderer renderer;

    Color orginalColor;
    [SerializeField] bool canBePlaced = true;
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
}
