using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenUI : MonoBehaviour
{
    private static WinScreenUI instance;

    public TMP_Text totalTime;
    public TMP_Text topTimesText;
    private AudioSource audioSource;
    [SerializeField] private AudioClip victory;
    

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

        if (victory != null)
        {
            audioSource.PlayOneShot(victory);
        }
        string getTotalTime = GameTimer.Instance.GetCurrentPlayTime();
        totalTime.text = "Total playing time: " + getTotalTime;
        string[] topTimes = GameTimer.Instance.GetTop5Times();
        topTimesText.text = "Top 5 shortest playtimes:\n";
        int i = 1;
        foreach (string time in topTimes)
        {
            //topTimesText.text += "Top "+i+":  "+time + "\n";
            //i++;
            string[] parts = time.Split(';'); 
            if (parts.Length < 3) continue;  

            string datePlayed = parts[0];    // Ngày chơi
            string playTime = parts[1];      // Thời gian chơi
            string playerName = parts[2];    // Tên người chơi

            topTimesText.text += $"Top {i}: {playerName} - {playTime} ({datePlayed})\n";
            i++;
        }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
