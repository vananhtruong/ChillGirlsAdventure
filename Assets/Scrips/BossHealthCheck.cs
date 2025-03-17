using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealthCheck : MonoBehaviour
{
    public BossAI boss;
    public GameObject winScreen;
    public GameObject loseScreen;
    private bool isDead = false;
    private bool islv3 = false;
    public SceneController sceneController;
    public static BossHealthCheck instance;

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
    void Update()
    {

        //if (boss != null && boss.currentHP <= 0 && !isDead 
        //    )
        //{
        //    isDead = true;
        //    StartCoroutine(ShowWinScreen());
        //}
        sceneController = FindAnyObjectByType<SceneController>();
        if (sceneController != null && sceneController.currentTao <= 0)
        {
            StartCoroutine(HandleDefeat());
        }
    }
    public void ShowWin()
    {
        StartCoroutine(ShowWinScreen());
    }
    IEnumerator ShowWinScreen()
    {
        yield return new WaitForSeconds(1);
        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("WinScreen chưa được gán trong Inspector!");
        } 
        GameTimer.Instance.StopAndSaveTime();
    }
    IEnumerator HandleDefeat()
    {
        yield return new WaitForSeconds(1f);
        if (loseScreen != null)
        {
            loseScreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("LoseScreen chưa được gán trong Inspector!");
        }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            Destroy(gameObject);
            sceneController = FindAnyObjectByType<SceneController>();
            Destroy(sceneController.gameObject);
        }
    }
}
