using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager instance;

    [SerializeField] private int maxHealth = 3; // Health of the player
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }
    private int currentHealth;
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
        }
    }
    private bool playerAlive = true;
    public bool PlayerAlive
    {
        get
        {
            return playerAlive;
        }
    }
    [SerializeField] private int respawnTimer = 5;  // How long the respawn timer should go for until it restarts
    private float respTimer;
    public float RespawnTimer
    {
        get
        {
            return respTimer;
        }
    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Initiate();
    }

    void Update()
    {
        // If the player is dead, run the respawn timer until it reloads
        if (!playerAlive)
        {
            if (respTimer <= 0f) ReloadScene();
            else respTimer -= Time.deltaTime;
        }
    }

    // Initiating function for every game start and scene load
    private void Initiate()
    {
        currentHealth = maxHealth;
        playerAlive = true;
    }

    // If the player is hit, reduce the health
    public void PlayerHit()
    {
        if (--currentHealth <= 0) Dead(); // If the health is zero, it's dead
        else
        {
            UIManager.instance.PlayerHit(); // Let the UI Manager know that player got hit
            Debug.Log("You got hit! " + currentHealth + " left in health.");
        }
    }

    // If the player is dead, begin respawning.
    public void Dead()
    {
        Debug.Log("Game over...");
        playerAlive = false;
        respTimer = respawnTimer;
        UIManager.instance.Dead(); // Let the UI Manager know that player died
    }

    // Reload the scene by loading itself
    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    // Load a given scene by name
    public void LoadScene(string sceneName)
    {
        Initiate(); // First reset the game states before starting
        SceneManager.LoadScene(sceneName);
    }
}
