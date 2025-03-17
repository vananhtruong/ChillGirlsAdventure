using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int damage = 1; // Số HP bị trừ khi chạm vào trap
    public float damageInterval = 1f; // Thời gian giữa các lần gây sát thương
    private bool isPlayerInTrap = false; // Kiểm tra player có đang ở trong trap không
    private float damageTimer = 0f; // Đếm thời gian để gây sát thương liên tục

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.PlayerTakeDamage(damage); // Gây sát thương ngay khi chạm vào
                isPlayerInTrap = true;
                damageTimer = damageInterval; // Reset timer
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isPlayerInTrap && other.CompareTag("Player"))
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0)
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.PlayerTakeDamage(damage);
                    damageTimer = damageInterval; // Reset timer để tiếp tục trừ HP sau khoảng thời gian
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrap = false; // Khi player rời trap, dừng gây sát thương
        }
    }
}
