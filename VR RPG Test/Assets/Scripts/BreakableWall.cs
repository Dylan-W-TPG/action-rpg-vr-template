using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IDamageable
{
    [SerializeField] private int wallHealth = 3; // Health of the wall
    [SerializeField] private Color isHitColor; // Colour when hit
    [SerializeField] private float isHitDuration = 0.6f; // How long it shows the hit colour before reverting
    [SerializeField] private Color almostDeadColor; // Colour when almost dead (one hit away)
    private float isHitTimer;
    
    private Renderer render;
    private Color initColor;


    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
        initColor = render.material.color;
    }

    void Update()
    {
        // Set the object to its normal colour when not hit, but if it's almost dead, set that colour instead. If hit state is active, run the isHit timer.
        if (isHitTimer <= 0f)
        {
            if (wallHealth == 1) render.material.color = almostDeadColor;
            else render.material.color = initColor;
        }
        else
        {
            isHitTimer -= Time.deltaTime;
        }
    }

    // Global function used by IDamageable: When the object is hit
    public void Damage()
    {
        // Let it know that it got hit, and if it's dead, destroy it
        render.material.color = isHitColor;
        isHitTimer = isHitDuration;
        if (--wallHealth <= 0) Destroy(this.gameObject);
    }
}
