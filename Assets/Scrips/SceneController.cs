using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator transitionAmin;
    public int maxHealth = 10;
    public int currentTao = 0; 
    public Text TextHeart;
    private int initialTao; // Lưu điểm đầu màn
    private int initialHealth; // Lưu máu đầu màn

    public int playerDame;
    private int initialPlayerDame;

    public bool haveRobot ;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // Chỉ khởi tạo điểm nếu chưa có
            if (currentTao == 0)
            {
                currentTao = maxHealth;
                playerDame = 1;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FindAndUpdateUI();
    }

    public void NextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void PreviousLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadLevel(SceneManager.GetSceneByName(sceneName).buildIndex));
    }

    public void ResetToLevelStart()
    {
        currentTao = initialTao; // Khôi phục điểm đầu màn
        maxHealth = initialHealth;
        playerDame = initialPlayerDame;
        Debug.Log("Reset màn về điểm đầu: Táo = " + currentTao + ", Máu = " + maxHealth);
        UpdateUI();
    }
    public IEnumerator LoadLevel(int sceneIndex)
    {
        if (transitionAmin != null)
        {
            transitionAmin.SetTrigger("End");
            yield return new WaitForSeconds(1);
        }

        yield return SceneManager.LoadSceneAsync(sceneIndex);

        if (transitionAmin != null)
        {
            transitionAmin.SetTrigger("Start");
        }

        FindAndUpdateUI();

        // 🟢 Khi vào màn mới, lưu lại điểm khởi đầu
        initialTao = currentTao;
        initialHealth = maxHealth;
        initialPlayerDame = playerDame;
    }

    public void AddTao()
    {
        currentTao++;
        Debug.Log("Táo sau khi Add: " + currentTao);
        UpdateUI();
    }

    public void AddShop(int items)
    {
        currentTao += items;
        Debug.Log("Táo sau khi AddShop: " + currentTao);
        UpdateUI();
    }
    public void AddHealth(int health)
    {
        maxHealth += health;
        currentTao += health;
        UpdateUI();
    }
    public void AddDame(int dame)
    {
        playerDame += dame;
        UpdateUI();
    }
    public void AddRobot()
    {
        haveRobot = true;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        maxHealth -= damage;
        currentTao -= damage;
        UpdateUI();
    }

    private void FindAndUpdateUI()
    {
        // Tìm lại TextHeart mỗi khi load màn mới
        TextHeart = GameObject.FindWithTag("UIHeart")?.GetComponent<Text>();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (TextHeart != null)
        {
           // TextHeart.text = "Táo: " + currentTao.ToString();
            TextHeart.text = currentTao.ToString();
        }
    }
    public void increateDame()
    {
        playerDame++;
    }

}
