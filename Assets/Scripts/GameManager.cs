using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] AudioSource[] audioSources = new AudioSource[8];
    [SerializeField] AudioSource SoulCorruptSFX;

    [Header ("End Screen things")]
    [SerializeField] public float PlayTime = 0;
    [SerializeField] public float EnemiesKilled = 0;
    [SerializeField] public float SoulsCollected = 0;
    [SerializeField] public float SoulsCorrupted = 0;

    public bool EnableBlood { get; private set; } = true;

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
    }

    private void Start()
    {
        if (SceneManager.GetSceneByName("MainMenu").isLoaded)
        {
            audioSources[0].Play();
        }
        if (SceneManager.GetSceneByName("Level").isLoaded)
        {
            audioSources[1].Play();
            audioSources[0].Stop();
        }
    }

    private void Update()
    {
        
        if (SceneManager.GetSceneByName("Level").isLoaded)
        {
            PlayTime += Time.deltaTime;
            audioSources[0].Stop();
        }

    }

    public void Reset()
    {
        PlayTime = 0f;
        EnemiesKilled = 0f;
        SoulsCorrupted = 0f;
        SoulsCollected = 0f;
    }

    public void AddPickedUpSoul()
    {
        SoulsCollected++;
    }

    public void AddCorruptedSoul()
    {
        SoulsCorrupted++;
        Instantiate(SoulCorruptSFX, FindFirstObjectByType<PlayerMovemenmt>().transform.position, Quaternion.identity);
    }

    public void ChangeBloodStatus()
    {
        EnableBlood = !EnableBlood;
    }

    public bool GetBloodStatus()
    {
        return EnableBlood;
    }
}
