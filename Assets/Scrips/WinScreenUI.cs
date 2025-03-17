using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenUI : MonoBehaviour
{
    public TMP_Text totalTime;
    public TMP_Text topTimesText;
    private AudioSource audioSource;
    [SerializeField] private AudioClip victory;
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
            topTimesText.text += "Top "+i+":  "+time + "\n";
            i++;
        }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
