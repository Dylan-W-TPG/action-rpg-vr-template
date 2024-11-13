using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTriggerTest : MonoBehaviour, IDamageable
{
    [SerializeField] private Color isHitColor; // Colour when hit
    [SerializeField] private float isHitDuration = 0.6f; // How long it shows the hit colour before reverting
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
    
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
        initColor = render.material.color;
    }

    void Update()
    {
        // Set the colour to normal if the object is not hurt. Otherwise, run the timer.
        if (isHitTimer <= 0f)
        {
            render.material.color = initColor;
            isHurt = false;
        }
        else
        {
            isHitTimer -= Time.deltaTime;
            isHurt = true;
        }
    }

    // Global function used by IDamageable: When the object is hit
    public void Damage()
    {
        // Let it know that it got hit
        render.material.color = isHitColor;
        isHitTimer = isHitDuration;
    }
}
