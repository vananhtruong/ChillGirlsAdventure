using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScript : MonoBehaviour
{
    [SerializeField] bool goNextLevel; 
    [SerializeField] string levelName; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;

            // Xử lý đặc biệt cho Level 2
            if (currentLevel == 2) // Giả sử Level 2 có buildIndex là 2
            {
                if (goNextLevel) // Cờ ở cuối map
                {
                    UnlockNewLevel();
                    SceneController.instance.NextLevel(); // Sang Level 3
                }
                else 
                {
                    SceneController.instance.PreviousLevel(); // Quay về Level 1
                    // Thay "Level1" bằng tên scene thực tế của Level 1 trong build settings
                }
            }
            else // Xử lý cho các level khác
            {
                UnlockNewLevel();

                if (goNextLevel)
                {
                    SceneController.instance.NextLevel();
                }
                else
                {
                    int targetLevelIndex = SceneUtility.GetBuildIndexByScenePath(levelName);
                    int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

                    if (targetLevelIndex <= unlockedLevel)
                    {
                        SceneController.instance.LoadScene(levelName);
                    }
                    else
                    {
                        Debug.Log("Level này chưa được mở khóa!");
                    }
                }
            }
        }
    }

    void UnlockNewLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int reachedLevel = PlayerPrefs.GetInt("ReachedIndex", 1);
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (currentLevel > reachedLevel)
        {
            PlayerPrefs.SetInt("ReachedIndex", currentLevel);
            PlayerPrefs.SetInt("UnlockedLevel", Mathf.Max(unlockedLevel, currentLevel));
            PlayerPrefs.Save();
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        if (goNextLevel)
    //        {
    //            SceneController.instance.NextLevel();
    //        }
    //        else
    //        {
    //            SceneController.instance.LoadScene(levelName);

    //        }
    //    }
    //}

    //void UnlockNewLevel()
    //{
    //    if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
    //    {
    //        PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
    //        PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel",1) + 1);
    //        PlayerPrefs.Save();

    //    }   
    //}
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        UnlockNewLevel();
    //        SceneController.instance.NextLevel();
    //    }
    //}
}
