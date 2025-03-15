using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject botPrefab;
    public float respawnDelay = 2f;

    public void Respawn(GameObject botPrefab, Vector3 deathPosition, Quaternion deathRotation)
    {
        StartCoroutine(RespawnCoroutine(deathPosition, deathRotation));
    }

    private IEnumerator RespawnCoroutine(Vector3 deathPosition, Quaternion deathRotation)
    {
        yield return new WaitForSeconds(respawnDelay);

        if (botPrefab != null)
        {
            Instantiate(botPrefab, deathPosition, deathRotation);
        }
        else
        {
            Debug.LogError("Bot Prefab không được gán trong EnemySpawner!");
        }
    }
}
