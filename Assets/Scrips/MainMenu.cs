using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    public void Play()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene("BotBackGround");
    }
    public void Quit()
    {
        //Time.timeScale = 1f;
        Application.Quit();
    }
}
