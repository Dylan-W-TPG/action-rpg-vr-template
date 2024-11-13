using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int enemyHealth = 3; // Health of the enemy
    [SerializeField] private float detectRadius; // How far the enemy can detect in a radius
    [SerializeField] private bool willDetect= true; // Whether the enemy should keep detection, or ignore and simply chase from anywhere
    [SerializeField] private Color isHitColor; // Colour when hit
    [SerializeField] private float isHitDuration = 0.6f; // How long it shows the hit colour before reverting
    [SerializeField] private Color almostDeadColor; // Colour when almost dead (one hit away)
    private float isHitTimer;
    private bool isHurt;
    public bool IsHurt
    {
        get
        {
            return isHurt;
        }
    }
    
    private Renderer render;
    private Color initColor;

    [SerializeField] private Transform targetObject; // Target to where the enemy should start chasing
    private NavMeshAgent agent;
    private float initSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
        initColor = render.material.color;

        agent = GetComponent<NavMeshAgent>();
        initSpeed = agent.speed;
    }

    void Update()
    {
        // Set the object to its normal colour when not hit, but if it's almost dead, set that colour instead. If hit state is active, run the isHit timer.
        if (isHitTimer <= 0f)
        {
            if (enemyHealth == 1) render.material.color = almostDeadColor;
            else render.material.color = initColor;
            isHurt = false;
        }
        else
        {
            isHitTimer -= Time.deltaTime;
            isHurt = true;
        }

        // If the destination is within the detection radius, let the enemy start chasing. Otherwise, stay still. (Can be ignored and still chase if willDetect is false)
        if (!willDetect || (transform.position - targetObject.position).magnitude <= detectRadius)
        {
            agent.speed = initSpeed;
        }
        else
        {
            agent.speed = 0f;
        }

        // Reset the destination
        agent.SetDestination(targetObject.position);
    }

    // Global function used by IDamageable: When the object is hit
    public void Damage()
    {
        // Let it know that it got hit, and if it's dead, destroy it
        render.material.color = isHitColor;
        isHitTimer = isHitDuration;
        if (--enemyHealth <= 0) Destroy(this.gameObject);
    }
}
