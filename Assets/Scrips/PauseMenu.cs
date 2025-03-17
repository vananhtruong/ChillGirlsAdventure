using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private SceneController sceneController;

    private void Awake()
    {
        // Tìm SceneController thông qua instance
        sceneController = SceneController.instance;
        if (sceneController == null)
        {
            Debug.LogError("SceneController instance không được tìm thấy! Đảm bảo SceneController đã được khởi tạo.");
        }
    }

    public void Restart()
    {
        if (sceneController != null)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Nếu là Level 1 (index 1), reset về giá trị ban đầu (10)
            if (currentSceneIndex == 1)
            {
                sceneController.currentTao = 10;
                //sceneController.maxHealth = 10;
            }
            else
            {
                // Ở các level khác, reset về điểm khởi đầu của level hiện tại
                sceneController.ResetToLevelStart();
            }

            sceneController.StartCoroutine(sceneController.LoadLevel(currentSceneIndex));
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogError("Không thể restart: SceneController không được tìm thấy!");
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    //public void Restart()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //    Time.timeScale = 1f;
    //}

}
