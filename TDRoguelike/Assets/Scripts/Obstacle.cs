using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private ObstacleScriptableObject _obstacleData;

    public ObstacleScriptableObject GetObstacleData()
    {
        return _obstacleData;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
