using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Singleton instance
    public static UIManager instance;
    
    [Header("Hit Screen")]
    [SerializeField] private Image gotHit; // Get the image necessary for hit effect
    [Range(0f, 1f)] [SerializeField] private float initialHitAlpha; // What alpha value it starts with when the player is hit before it begins reducing
    [Range(0f, 1f)] [SerializeField] private float deadAlpha; // What alpha value it stays with when the player dies
    private Color color;

    [Header("Health Bar")]
    [SerializeField] private RectTransform playerHealthBar; // Get the health bar of the player
    private RectTransform outerHealthBar;

    [Header("Respawn Timer")]
    [SerializeField] private TextMeshProUGUI respawnText; // Get the text component to provide the respawn timer

    [Header("Debug Mode")]
    [SerializeField] private bool showSwordSpeed = false; // Set this to true to enable text in showing the current speed of the sword
    [SerializeField] TextMeshProUGUI debugSpeedText; // Get the text component to provide the sword speed text to the designated debug element
    
    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Ensure only one instance of UIManager exists
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("There is another UIManager that already exists in this scene.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (showSwordSpeed) debugSpeedText.rectTransform.transform.parent.gameObject.SetActive(true);
        else debugSpeedText.rectTransform.transform.parent.gameObject.SetActive(false);
        
        respawnText.transform.parent.gameObject.SetActive(false);
        outerHealthBar = playerHealthBar.transform.parent.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the hit effect is active, reduce the alpha value gradually. If the player is dead, focus on relaying the respawn text
        if (gotHit.color.a > 0 && GameManager.instance.PlayerAlive)
        {
            ColorAlphaSet(gotHit.color.a - Time.deltaTime);
        }
        else if (!GameManager.instance.PlayerAlive)
        {
            respawnText.text = "Respawning<br>" + Mathf.Ceil(GameManager.instance.RespawnTimer);
        }
    }

    // Common function used to set the alpha value of the hit effect colour
    private void ColorAlphaSet(float alphaValue)
    {
        color = gotHit.color;
        color.a = alphaValue;
        gotHit.color = color;
    }

    // If the player is hit, update the health bar, and enable the hit effect by setting the alpha value
    public void PlayerHit()
    {
        playerHealthBar.localScale = new Vector3((float)GameManager.instance.CurrentHealth / (float)GameManager.instance.MaxHealth, 1f, 1f);
        ColorAlphaSet(initialHitAlpha);
    }

    // If the player is dead, set health bar to zero, enable the hit effect by setting the alpha value, and enable the respawn text
    public void Dead()
    {
        playerHealthBar.localScale = new Vector3(0f, 1f, 1f);
        ColorAlphaSet(deadAlpha);
        respawnText.transform.parent.gameObject.SetActive(true);
    }

    // If debug on speed is enabled, show the sword speed by enabling and relaying info to the text
    public void DebugSpeed(float swordSpeed)
    {
        if (debugSpeedText != null && showSwordSpeed)
        {
            debugSpeedText.text = "Speed:<br>" + Mathf.Abs(swordSpeed);
        }
    }
}
