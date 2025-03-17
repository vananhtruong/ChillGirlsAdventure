using System.Collections;
using UnityEngine;

public class BossHealthCheck : MonoBehaviour
{
    public BossAI boss;
    public GameObject winScreen;
    public Player player;
    public GameObject loseScreen;
    private bool isDead = false;

    void Update()
    {
        if (boss != null && boss.currentHP <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(ShowWinScreen());
        }
        if (player != null && player.maxHealth <= 0)
        {
            StartCoroutine(HandleDefeat());
        }
    }

    IEnumerator ShowWinScreen()
    {
        yield return new WaitForSeconds(1);
        winScreen.SetActive(true); // Show Win Screen
        GameTimer.Instance.StopAndSaveTime(); // Save data
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
}
