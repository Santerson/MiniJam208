using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header ("End Screen things")]
    [SerializeField] public float PlayTime = 0;
    [SerializeField] public float EnemiesKilled = 0;
    [SerializeField] public float SoulsCollected = 0;
    [SerializeField] public float SoulsCorrupted = 0;

    [Header("Varaint Enemies Changes")]
    [SerializeField] public float HealthChange = 0f;
    [SerializeField] public float SpeedChange = 0f;
    [SerializeField] public float DamageChange = 0f;

    [Header("Timers")]
    [SerializeField] float UpdateTimer = 60f;
    [SerializeField] float ResetUpdateTimer = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        ResetUpdateTimer = UpdateTimer;
    }

    private void Update()
    {
        
        if (SceneManager.GetSceneByName("Level").isLoaded)
        {
            PlayTime += Time.deltaTime;
            UpdateTimer -= Time.deltaTime;
            if (UpdateTimer <= 0f)
            {
                HealthChange++;
                SpeedChange++;
                DamageChange++;
                UpdateTimer = ResetUpdateTimer;
            }
        }

    }

    public void Reset()
    {
        PlayTime = 0f;
        EnemiesKilled = 0f;
        SoulsCorrupted = 0f;
        SoulsCollected = 0f;
        HealthChange = 0f;
        SpeedChange = 0f;
        DamageChange = 0f;
    }

    public void AddPickedUpSoul()
    {
        SoulsCollected++;
    }

    public void AddCorruptedSoul()
    {
        SoulsCorrupted++;
    }
}
