using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenUI : MonoBehaviour
{
    private static LoseScreenUI instance;
    public TMP_Text totalTime;
    private AudioSource audioSource;
    [SerializeField] private AudioClip loseSound;
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in allAudioSources)
        {
            if (source != audioSource) // Không dừng chính nó
            {
                source.Stop();
            }
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (loseSound != null)
        {
            audioSource.PlayOneShot(loseSound);
        }
        string getTotalTime = GameTimer.Instance.GetCurrentPlayTime();
        if (totalTime != null)
        {
            totalTime.text = "Total playing time: " + getTotalTime;
        }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
