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

    public static event Action<float> OnEnemyPathCompleate;

    private WaypointManager waypointManager;
    [SerializeField] private float distanceToNextWaypoint = 0;
    [SerializeField] private int currentWaypoint;

    private void Start()
    {
        waypointManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaypointManager>();
    }

    private void Update()
    {
        RotateTowardsNextWaypoint();
        MoveTowardsNextWaypoint();
    }

    private void MoveTowardsNextWaypoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypointManager.waypoints[currentWaypoint].position, speed * Time.deltaTime);

        distanceToNextWaypoint = Vector3.Distance(transform.position, waypointManager.waypoints[currentWaypoint].position);

        if (distanceToNextWaypoint < 0.01f)
        {
            if (currentWaypoint < waypointManager.waypoints.Length - 1)
            {
                currentWaypoint++;
                distanceToNextWaypoint = Vector3.Distance(transform.position, waypointManager.waypoints[currentWaypoint].position);
            }
            else
            {
                OnEnemyPathCompleate?.Invoke(damage);
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

    public int GetCurrentWaypoint()
    {
        return currentWaypoint;
    }

    public float GetDistanceToNextWaypoint()
    {
        return distanceToNextWaypoint;
    }
}
