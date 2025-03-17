using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator transitionAmin;
    //public int maxHealth = 10;
    public int currentTao = 0;
    public Text TextHeart;
    private int initialTao; // Lưu điểm đầu màn
    //private int initialHealth; // Lưu máu đầu màn
    public int coins = 0;
    public Text TextCoin;
    private int initialCoins; // Lưu số coin ban đầu của màn chơi

    public int playerDame;
    private int initialPlayerDame;

    public bool haveRobot ;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (currentTao == 0)
            {
                currentTao = 10;
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
        //maxHealth = initialHealth;
        coins = initialCoins; // Khôi phục số coin về giá trị ban đầu
        //Debug.Log($"Reset màn về điểm đầu: Táo = {currentTao}, Máu = {maxHealth}, Coin = {coins}");
        UpdateUI();
        UpdateCoinUI();
    }

    public IEnumerator LoadLevel(int sceneIndex)
    {
        if (transitionAmin != null)
        {
            transitionAmin.SetTrigger("End");
            yield return new WaitForSeconds(1);
        }

        yield return SceneManager.LoadSceneAsync(sceneIndex);
        yield return null; // Đợi frame tiếp theo để đảm bảo UI đã load hoàn toàn

        if (transitionAmin != null)
        {
            transitionAmin.SetTrigger("Start");
        }

        FindAndUpdateUI();

        // Lưu lại giá trị đầu màn mới
        initialTao = currentTao;
        //initialHealth = maxHealth;
        initialCoins = coins;

        // 🟢 Đảm bảo UI của coin được cập nhật ngay sau khi load màn
        UpdateCoinUI();
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
        if(coins >=20 )
        {
            currentTao += health;
            coins -= 20;
            UpdateUI();
            UpdateCoinUI();
        }
    }
    public void AddDame(int dame)
    {
        if (coins >= 10)
        {
            playerDame += dame;
            coins -= 10;
            UpdateCoinUI();
        }
    }
    public void AddRobot()
    {
        if (coins >= 200)
        {
            haveRobot = true;
            coins -= 200;
            UpdateCoinUI();
        }
    }

    public void TakeDamage(int damage)
    {
        //maxHealth -= damage;
        currentTao -= damage;
        UpdateUI();
    }

    private void FindAndUpdateUI()
    {
        TextHeart = GameObject.FindWithTag("UIHeart")?.GetComponent<Text>();
        //TextCoin = GameObject.FindWithTag("UICoin")?.GetComponent<Text>();
        Debug.Log("Tìm thấy coin: " + TextCoin);
        Debug.Log("Tìm thấy UIHeart: " + TextHeart);
        UpdateUI();
        UpdateCoinUI();
    }

    private void UpdateUI()
    {
        if (TextHeart != null)
        {
            TextHeart.text = currentTao.ToString();
        }
    }
    public void increateDame()
    {
        playerDame++;
    }

    public void UpdateCoinUI()
    {
        if (TextCoin != null)
        {
            TextCoin.text = coins.ToString();
        }
    }
    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
        Destroy(gameObject);
    }
}
