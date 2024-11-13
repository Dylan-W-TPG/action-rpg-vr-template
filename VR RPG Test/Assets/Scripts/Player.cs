using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class Player : MonoBehaviour, IDamageable
{
    private CharacterController charCont;
    private SphereCollider sphereCol;

    [SerializeField] private float invincibilityTime = 1.5f; // How long the player can be invincible before it reverts to normal
    private float invinTime;

    [SerializeField] private DynamicMoveProvider dynamicMove; // Get the DynamicMoveProvider component
    [SerializeField] private ContinuousTurnProvider continuousTurn; // Get the ContinuousTurnProvider component
    [SerializeField] private GameObject sword; // Get the sword
    
    // Start is called before the first frame update
    void Start()
    {
        charCont = GetComponent<CharacterController>();
        sphereCol = GetComponent<SphereCollider>();
        invinTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        sphereCol.center = charCont.center; // Make sure the hit detection follows the centre of the CharacterController instead of transform
        if (invinTime > 0f) invinTime -= Time.deltaTime; // Run the timer if invincibility is active
    }

    // Hit detection
    void OnTriggerStay(Collider collider)
    {
        // If the enemy (who is not in a hurt state) hits the player while they are vulnerable, damage the player.
        if (collider.CompareTag("Enemy") && !collider.gameObject.GetComponent<Enemy>().IsHurt && invinTime <= 0f && GameManager.instance.PlayerAlive)
        {
            Damage();
        }
    }

    // Global function used by IDamageable: When the object is hit
    public void Damage()
    {
        GameManager.instance.PlayerHit(); // Let the game know that the player got hit
        invinTime = invincibilityTime; // Begin invincibility

        // If the player is dead, negate movement and turning, and disable the sword
        if (!GameManager.instance.PlayerAlive)
        {
            dynamicMove.moveSpeed = 0;
            continuousTurn.turnSpeed = 0;
            sword.SetActive(false);
        }
    }
}
