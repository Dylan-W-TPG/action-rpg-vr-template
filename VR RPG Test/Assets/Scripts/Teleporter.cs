using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private string sceneName; // Name of the target scene
    
    // Detection
    void OnTriggerEnter(Collider collider)
    {
        // Relay the scene name to the game if the player moves into the zone
        if (collider.CompareTag("Player"))
        {
            GameManager.instance.LoadScene(sceneName);
        }
    }
}
