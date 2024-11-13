using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwordHead : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Transform of player
    
    [SerializeField] private float cooldownTimer = 1f; // Cooldown time until sword recharges for the next attack
    [SerializeField] private Color cooldownColor; // Colour of the cooldown state
    private float coolTimer;

    private Renderer render;
    private Collider collide;
    private Color initColor;

    [SerializeField] private float speedRequirement = 1f; // How fast the sword should go to allow attacking
    private float speed;
    private float playerSpeed;
    private Vector3 prevPos;
    private Vector3 playerPrevPos;
    private float finalSpeed;

    
    // Start is called before the first frame update
    void Start()
    {
        coolTimer = 0f;
        render = GetComponent<Renderer>();
        collide = GetComponent<Collider>();
        initColor = render.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        // If the sword is recharged, return the collider and colour to normal
        if (coolTimer >= 0f)
        {
            coolTimer -= Time.deltaTime;
        }
        else
        {
            collide.isTrigger = false;
            render.material.color = initColor;
        }

        /*
            Speed being calculated in a traditional sense. It takes the previous and current position as differences in magnitude and calculates that as speed.
            However, with the player moving as well, it should negate part of that speed so that only the sword speed is calculated relative to the player.
        */
        speed = (transform.position.magnitude - prevPos.magnitude) / Time.deltaTime;
        prevPos = transform.position;
        playerSpeed = (playerTransform.position.magnitude - playerPrevPos.magnitude) / Time.deltaTime;
        playerPrevPos = playerTransform.position;
        finalSpeed = speed - playerSpeed;

        // Relay speed information to UIManager
        UIManager.instance.DebugSpeed(finalSpeed);
    }

    // Sword collision detection
    void OnCollisionStay(Collision collision)
    {
        /*
            Check if the collision is valid. It should be false if one of the following points is invalid:
            - Hitting the player
            - Is not an IDamageable object
            - Speed is lower than requirement
        */
        if (!collision.collider.CompareTag("Player") && collision.collider.TryGetComponent<IDamageable>(out IDamageable damageable) && Mathf.Abs(finalSpeed) >= speedRequirement)
        {
            // Set sword into a cooldown state by making the collision a non-working trigger and set its colour.
            coolTimer = cooldownTimer;
            collide.isTrigger = true;
            render.material.color = cooldownColor;

            // Initiate the damage function to the IDamageable object.
            damageable.Damage();
        }
    }
}
