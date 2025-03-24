using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossHealthCheck : MonoBehaviour
{
    public BossAI boss;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject enterNameScreen;
    private bool isDead = false;
    private bool islv3 = false;
    public SceneController sceneController;
    public static BossHealthCheck instance;

    public TMP_InputField nameInputField;
    public Button okButton;
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
        enterNameScreen.SetActive(true);
        //
        okButton.onClick.RemoveAllListeners(); // Xóa các sự kiện cũ (tránh trùng lặp)
        okButton.onClick.AddListener(() =>
        {
            if (!string.IsNullOrEmpty(nameInputField.text)) 
            {
                GameTimer.Instance.SetPlayerName(nameInputField.text);
                enterNameScreen.SetActive(false); 
                StartCoroutine(ShowWinScreen());
            }
            else
            {
                Debug.Log("Chưa nhập tên kìa!");
            }
        });
        //
        //StartCoroutine(ShowWinScreen());
    }
    IEnumerator ShowWinScreen()
    {
        yield return new WaitForSeconds(1);
        if (winScreen != null)
        {
            GameTimer.Instance.StopAndSaveTime();
            winScreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("WinScreen chưa được gán trong Inspector!");
        } 
        
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
