using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerProjectile : MonoBehaviour
{
    float speed;
    float damage;
    int pierceThroughEnemiesAmount;
    Vector3 direction;

    IObjectPool<PlayerProjectile> _pool;

    public void SetPool(IObjectPool<PlayerProjectile> pool) => _pool = pool;

    public void Create(Vector3 direction, float speed, float damage, int pierceThroughEnemiesAmount)
    {
        this.direction = new Vector3(direction.x, 0f, direction.z).normalized;
        this.speed = speed;
        this.damage = damage;
        this.pierceThroughEnemiesAmount = pierceThroughEnemiesAmount;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (pierceThroughEnemiesAmount > 0)
            {
                pierceThroughEnemiesAmount--;
                collision.gameObject.GetComponent<IDamegeable>().TakeDamage(damage);
            }
            if (pierceThroughEnemiesAmount == 0)
            {
                _pool.Release(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall")) _pool.Release(this);
    }
}
