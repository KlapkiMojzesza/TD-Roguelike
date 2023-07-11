using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCloseRangeAttack : MonoBehaviour
{
    [SerializeField] private int _closeRangeEnemyDamage = 10;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _animator.SetTrigger("attack");
            collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(_closeRangeEnemyDamage);
        }
    }
}
