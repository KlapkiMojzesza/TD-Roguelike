using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectile : Projectile
{
    [SerializeField] private float _splashDamageLevel2 = 30f;
    [SerializeField] private float _splashRangeLevel2 = 5f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private CannonProjectileLevel _projectileLevel = CannonProjectileLevel.FirstLevel;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] Transform _objectToRotate;

    private Vector3 _randomRotation;
    private bool hitSomething = false;

    public override void Create(Transform target, float speed, float damage, int enemyPierce)
    {
        base.Create(target, speed, damage, enemyPierce);
        _randomRotation = new Vector3(Random.Range(0f, 180f), Random.Range(0f, 180f), Random.Range(0f, 180f)).normalized * _rotationSpeed;

    }

    protected override void Update()
    {
        base.Update();
        
        if (_projectileLevel == CannonProjectileLevel.SecondLevel ||
            _projectileLevel == CannonProjectileLevel.ThirdLevel)
        {
            _objectToRotate.Rotate(_randomRotation * Time.deltaTime);
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        //if (!gameObject.activeSelf) return;
        //if (hitSomething) return;

        if (collision.collider.CompareTag("Ground"))
        {
            HandleProjectileHit(null);
            gameObject.SetActive(false);
            _pool.Release(this);
            //hitSomething = true;
            return;
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            _target = null;
            collision.gameObject.GetComponent<IDamegeable>().TakeDamage(_damage);
            HandleProjectileHit(collision.collider);
            gameObject.SetActive(false);
            _pool.Release(this);
            //hitSomething = true;
        }
    }

    private void HandleProjectileHit(Collider enemyCollider)
    {
        switch (_projectileLevel)
        {
            case CannonProjectileLevel.FirstLevel:
                Debug.Log("Level 1 hit");
                //nothing
                break;
            case CannonProjectileLevel.SecondLevel:

                Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, _splashRangeLevel2, _enemyLayer);
                foreach (Collider enemy in enemiesInRange)
                {
                    if (enemy != enemyCollider) enemy.GetComponent<IDamegeable>().TakeDamage(_damage);
                }

                //splash damage
                break;
            case CannonProjectileLevel.ThirdLevel:
                Debug.Log("Level 3 hit");
                //more bombs
                break;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _splashRangeLevel2);
    }
}

public enum CannonProjectileLevel { FirstLevel, SecondLevel, ThirdLevel};
