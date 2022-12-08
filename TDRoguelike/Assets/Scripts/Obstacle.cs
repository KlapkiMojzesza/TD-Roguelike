using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int removePriceAdjuster = 0;

    [Header("To Attach")]
    [SerializeField] private ObstacleScriptableObject _obstacleData;

    private int _removePrice;

    private void Start()
    {
        _removePrice = _obstacleData.RemovePrice + removePriceAdjuster;
    }

    public ObstacleScriptableObject GetObstacleData()
    {
        return _obstacleData;
    }

    public int GetRemovePrice()
    {
        return _removePrice;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
