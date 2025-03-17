using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    public Sprite soundOnIcon; // Ảnh khi bật âm thanh
    public Sprite soundOffIcon; // Ảnh khi tắt âm thanh
    private Image buttonImage;
    private bool isMuted = false;

    private static SoundToggle instance; // Biến lưu trạng thái âm thanh duy nhất

    void Awake()
    {
        // Đảm bảo chỉ có một instance tồn tại
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Giữ object khi đổi scene
        }
        else
        {
            Destroy(gameObject); // Nếu đã có instance, hủy object mới
            return;
        }
    }

    void Start()
    {
        buttonImage = GetComponent<Image>();

        // Lấy trạng thái âm thanh đã lưu
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        UpdateAudio();
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
        UpdateAudio();
    }

    void UpdateAudio()
    {
        AudioListener.volume = isMuted ? 0 : 1;
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? soundOffIcon : soundOnIcon;
        }
    }
}
