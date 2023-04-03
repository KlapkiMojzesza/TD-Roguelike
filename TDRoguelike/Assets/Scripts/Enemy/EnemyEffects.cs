using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffects : MonoBehaviour
{
    private EnemyMovement _enemyMovement;
    private float _pylonSlowTimer = 0f;

    private void Start()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        _pylonSlowTimer -= Time.deltaTime;

        if (_pylonSlowTimer <= 0f)
        {
            _enemyMovement.PylonRemoveSlow();
            _pylonSlowTimer = Mathf.Infinity;
        }
    }

    public void PylonSlowEnemy(GameObject enemy, float slowPercentage, float slowTime)
    {
        _enemyMovement.PylonSlowEnemy(slowPercentage);
        _pylonSlowTimer = slowTime;
        //show visual effect (icon or color change)
    }

    public void MinotaurSpeedEnemy(GameObject enemy, float speedPercentage)
    {
        _enemyMovement.UpgradeSpeed(speedPercentage);
        //show boost icon
    }
    
}
