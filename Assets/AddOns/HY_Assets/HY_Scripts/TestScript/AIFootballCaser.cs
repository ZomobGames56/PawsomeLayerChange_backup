using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFootballCaser : MonoBehaviour
{
    public Transform ball; // Assign the football transform in the Inspector
    public float moveSpeed = 5f;
    public float detectionDistance = 1f;
    public float rotationSpeed = 300f;

    private Quaternion targetRotation;

    void Start()
    {
        targetRotation = transform.rotation; // Initialize targetRotation
    }

    void Update()
    {
        // Check for nearby AI agents and handle avoidance
        if (AvoidOthers())
        {
            // If avoiding another AI, do not move towards the ball
            return;
        }

        // If no nearby agents, move towards the ball
        MoveTowardsBall();

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void MoveTowardsBall()
    {
        // Calculate direction to the ball
        Vector3 directionToBall = (ball.position - transform.position).normalized;

        // Update target rotation to face the ball
        targetRotation = Quaternion.LookRotation(directionToBall);

        // Move towards the ball
        transform.Translate(directionToBall * moveSpeed * Time.deltaTime, Space.World);
    }

    bool AvoidOthers()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider != this.GetComponent<Collider>())
            {
                // Calculate a direction away from the other AI
                Vector3 awayDirection = (transform.position - hitCollider.transform.position).normalized;

                // Update target rotation to face away from the other AI
                targetRotation = Quaternion.LookRotation(awayDirection);

                // Move away from the other AI
                transform.Translate(awayDirection * moveSpeed * Time.deltaTime, Space.World);
                return true; // Return true to indicate avoidance
            }
        }
        return false; // No avoidance needed
    }
}