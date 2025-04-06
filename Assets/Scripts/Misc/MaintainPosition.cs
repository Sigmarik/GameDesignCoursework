using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainPosition : MonoBehaviour
{
    public float springConstant = 5.0f; // Spring constant (k)
    public float damping = 0.5f; // Damping factor
    public Vector3 equilibriumPosition; // Equilibrium position of the spring
    public float mass = 1.0f; // Mass of the rigid body

    private Rigidbody rb; // Reference to the Rigidbody component
    private Vector3 velocity; // Current velocity of the rigid body

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        equilibriumPosition = transform.position; // Set the equilibrium position to the initial position
    }

    void FixedUpdate()
    {
        // Calculate the displacement from the equilibrium position
        Vector3 displacement = transform.position - equilibriumPosition;

        // Calculate the spring force (F = -kx)
        Vector3 springForce = -springConstant * displacement;

        // Calculate the damping force (F_damping = -b * v)
        Vector3 dampingForce = -damping * velocity;

        // Calculate the net force
        Vector3 netForce = springForce + dampingForce;

        // Update the acceleration
        Vector3 acceleration = netForce / mass;

        // Update the velocity
        velocity += acceleration * Time.fixedDeltaTime;

        // Update the position of the rigid body
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
