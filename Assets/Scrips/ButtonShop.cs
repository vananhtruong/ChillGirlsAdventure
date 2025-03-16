using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonShop : MonoBehaviour
{
    [SerializeField] GameObject buttonShop; // Panel Shop
    [SerializeField] SceneController sceneController; // Điều khiển scene

    private void Start()
    {
        // Tự động tìm SceneController nếu chưa gán
        if (sceneController == null)
        {
            sceneController = FindObjectOfType<SceneController>();
        }
    }

    // Khi nhấn vào Button trong Shop để mua item
    public void BuyItem(Button button)
    {
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            int itemValue = int.Parse(buttonText.text); // Lấy giá trị từ Text
            sceneController.AddShop(itemValue); // Cộng vào máu
        }
    }

    public void ShopButton()
    {
        buttonShop.SetActive(true);
        Time.timeScale = 0f;
    }
    public void CloseButton()
    {
        buttonShop.SetActive(false);
        Time.timeScale = 1f;
    }

}
