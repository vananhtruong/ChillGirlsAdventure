using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image Hp; 

    public void UpdateHP(float currentHP, float maxHP)
    {
        Hp.fillAmount = currentHP/maxHP;
    }
}
