using System.IO; 
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int playerHeart; 
    public int playerGold;
    public static PlayerData instance;

    private string saveFilePath; 

    private void Awake()
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

        saveFilePath = Application.persistentDataPath + "/PlayerData.txt";

        LoadData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();
        }
    }

    public void SaveData()
    {
        string data = playerHeart + "\n" + playerGold;

        File.WriteAllText(saveFilePath, data);

        Debug.Log("Game data saved to: " + saveFilePath);
    }

    public void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string[] data = File.ReadAllLines(saveFilePath);

            if (data.Length >= 2)
            {
                playerHeart = int.Parse(data[0]);
                playerGold = int.Parse(data[1]);

                Debug.Log("Game data loaded: Heart = " + playerHeart + ", Gold = " + playerGold);
            }
        }
        else
        {
            playerHeart = 10; 
            playerGold = 0;   

            Debug.Log("No save file found. Initialized default values.");
        }
    }
}
