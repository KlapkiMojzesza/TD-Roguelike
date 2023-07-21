using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCloseRangeAttack : MonoBehaviour
{
    private EnemyMovement _enemyMovement;
    private Animator _animator;

    private void Start()
    {
        _enemyMovement = GetComponentInParent<EnemyMovement>();
        _animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _animator.SetTrigger("attack");
            collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(_enemyMovement.GetDamageToPlayer());
        }
    }
}
