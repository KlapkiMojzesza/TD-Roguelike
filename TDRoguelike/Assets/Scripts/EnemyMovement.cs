using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float damage = 25;
    [SerializeField] float speed;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float lookDirectionOffset = -90f;

    private WaypointManager waypointManager;
    private PlayerBase playerBase;
    public static event Action<GameObject> OnEnemyDeath;

    private int currentWaypoint;

    private void Start()
    {
        waypointManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaypointManager>();
        playerBase = GameObject.FindGameObjectWithTag("PlayerBase").GetComponent<PlayerBase>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypointManager.waypoints[currentWaypoint].position, speed * Time.deltaTime);

        RotateTowardsNextWaypoint();

        if (Vector3.Distance(transform.position, waypointManager.waypoints[currentWaypoint].position) < 0.01f)
        {
            if (currentWaypoint < waypointManager.waypoints.Length - 1)
            {
                currentWaypoint++;
            }
            else
            {
                playerBase.TakeDamage(damage);
                OnEnemyDeath?.Invoke(gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void RotateTowardsNextWaypoint()
    {
        Vector3 lookDirection = waypointManager.waypoints[currentWaypoint].position - transform.position;
        float angle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg - lookDirectionOffset;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), rotateSpeed);
    }
}
