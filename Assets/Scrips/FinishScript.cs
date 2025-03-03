using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScript : MonoBehaviour
{
    //[SerializeField] bool goNextLevel;
    //[SerializeField] string levelName;
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        if (goNextLevel)
    //        {
    //            ManagemantSence.instance.NextLevel();
    //        }
    //        else
    //        {
    //            ManagemantSence.instance.LoadScene(levelName);

    //        }
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnlockNewLevel();
            ManagemantSence.instance.NextLevel();
        }
    }
    void UnlockNewLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel",1) + 1);
            PlayerPrefs.Save();

        }   
    }
}
