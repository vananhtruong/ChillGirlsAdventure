using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    private float elapsedTime = 0f;
    private bool isRunning = true;
    public TMP_Text timeText;
    private string filePath;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        filePath = Application.persistentDataPath + "/GameTimes.txt";

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Lịch sử thời gian chơi:\n");
        }
        Debug.Log("GameTimes.txt path: " + filePath);

    }

    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Main Menu")
        {
            isRunning = false;
            elapsedTime = 0f;
            if (timeText != null) timeText.gameObject.SetActive(false);
            return;
        }
        else
        {
            isRunning = true;
            if (timeText != null) timeText.gameObject.SetActive(true);
        }

        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimeDisplay();
        }
    }

    void UpdateTimeDisplay()
    {
        int hours = Mathf.FloorToInt(elapsedTime / 3600);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timeText.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
    public string GetCurrentPlayTime()
    {
        int hours = Mathf.FloorToInt(elapsedTime / 3600);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
    public void StopAndSaveTime()
    {
        isRunning = false;
        SaveTimeToFile();
    }
    void SaveTimeToFile()
    {
        string timestamp = DateTime.Now.ToString("HH:mm dd/MM/yyyy");
        string timePlayed = $"{Mathf.FloorToInt(elapsedTime / 3600):D2}:{Mathf.FloorToInt((elapsedTime % 3600) / 60):D2}:{Mathf.FloorToInt(elapsedTime % 60):D2}";
        string log = $"{timestamp};{timePlayed}";

        File.AppendAllText(filePath, log + "\n");
    }

    public string[] GetTop5Times()
    {
        if (!File.Exists(filePath)) return new string[0];

        string[] lines = File.ReadAllLines(filePath);

        List<string> validLines = new List<string>();
        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(lines[i]))
            {
                validLines.Add(lines[i]);
            }
        }

        if (validLines.Count == 0) return new string[0];

        validLines.Sort((a, b) =>
        {
            TimeSpan timeA = TimeSpan.Parse(a.Split(';')[1]);
            TimeSpan timeB = TimeSpan.Parse(b.Split(';')[1]);
            return timeA.CompareTo(timeB);
        });

        return validLines.Count > 5 ? validLines.Take(5).ToArray() : validLines.ToArray();
    }

    public void PauseTimer() => isRunning = false;
    public void ResumeTimer() => isRunning = true;
}
