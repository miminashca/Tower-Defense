using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A MoveBehaviour implementation that uses Unity's NavMeshAgent to navigate enemies across a NavMesh.
/// Inherits base event subscriptions and logic from MoveBehaviour, but updates the agent's destination and speed.
/// </summary>
public class NavMeshMoveBehaviour : MoveBehaviour
{
    /// <summary>
    /// A reference to the NavMeshAgent responsible for pathfinding and movement.
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// In addition to the base Awake logic, ensures this GameObject has a NavMeshAgent component,
    /// creating one if necessary.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        if (!GetComponent<NavMeshAgent>())
        {
            agent = gameObject.AddComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        }
        else
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }
    
    /// <summary>
    /// Sets the target position for the NavMeshAgent and updates its destination.
    /// </summary>
    /// <param name="position">The position the agent should move to.</param>
    public override void SetTargetPosition(Vector3 position)
    {
        base.SetTargetPosition(position);
        if(agent.isOnNavMesh) agent.destination = position;
    }

    /// <summary>
    /// Invoked once the entity has reached its target. Calls the base stop logic and then resets the agent's path.
    /// </summary>
    protected override void Stop()
    { 
        base.Stop();
        if(agent.isOnNavMesh) agent.ResetPath();
    }

    /// <summary>
    /// Updates the NavMeshAgent's speed in tandem with the base FixedUpdate logic,
    /// ensuring the agent moves at the correct rate based on the current time scale and debuffs.
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        agent.speed = speed;
    }
}