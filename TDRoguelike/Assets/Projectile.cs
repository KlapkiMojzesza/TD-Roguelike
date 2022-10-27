using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public void Create(Vector3 spawnPosition, Vector3 targetPosition)
    {
        Transform projectileTransform = Instantiate(GameAssets.i.projectile, spawnPosition, Quaternion.identity);

        Projectile projectile = projectileTransform.GetComponent<Projectile>();
        projectile.Setup(targetPosition);
    }

    private Vector3 targetPosition;

    private void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float moveSpeed = 40f;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        float destroyDistance = 1f;

        if (Vector3.Distance(transform.position, targetPosition) < destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
