using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenUI : MonoBehaviour
{
    public TMP_Text totalTime;
    private AudioSource audioSource;
    [SerializeField] private AudioClip loseSound;

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
