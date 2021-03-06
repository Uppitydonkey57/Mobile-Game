using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Advertisements;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private GameObject firstChunk;
    [SerializeField] private GameObject[] chunks;

    private List<GameObject> worldChunks;
    private Chunk previouseChunk;

    private float placePosition;

    [SerializeField] private TextMeshProUGUI scoreText;
    private Animator scoreAnimator;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private int[] multiplierThresholds;

    [SerializeField] private Animator loadAnimator;
    [SerializeField] private float sceneEndTime;

    private bool playerDead;

    private int score;
    public int Score { get { return score; } }
    private int multiplier = 1;

    [SerializeField] private string adsID = "4276955";
    [SerializeField] private bool isTestingAds;

    private void Start()
    {
        if (!Advertisement.isInitialized)
            Advertisement.Initialize(adsID, isTestingAds);

        placePosition = transform.position.x;
        
        if (chunks.Length > 0) CreateChunk(firstChunk);

        if (scoreText != null)
        {
            scoreText.text = score.ToString();

            scoreAnimator = scoreText.GetComponent<Animator>();
        }

        if (multiplierText != null) multiplierText.text = multiplier.ToString() + "x";

        if (chunks.Length > 0) for (int i = 0; i < 5; i++) CreateChunk(chunks[Random.Range(0, 5)]);

        StartCoroutine(SpawnChunks());
    }

    private void OnEnable()
    {
        PlayerController.PlayerDead += OnPlayerDead;
    }

    private void OnDisable()
    {
        PlayerController.PlayerDead -= OnPlayerDead;
    }

    IEnumerator SpawnChunks()
    {
        while (true)
        {
            if (chunks.Length > 0 && !playerDead) CreateChunk(chunks[Random.Range(0, chunks.Length)]);
            yield return new WaitForSeconds(1f);
        }
    }

    public void ResetScene()
    {
        StartCoroutine(LoadSceneCoroutine(SceneManager.GetActiveScene().name));
    }

    public void LoadScene(string name)
    {
        StartCoroutine(LoadSceneCoroutine(name));
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        loadAnimator.SetTrigger("EndScene");
        yield return new WaitForSeconds(sceneEndTime);

        Debug.Log(Advertisement.IsReady("Interstitial"));
        Debug.Log(Advertisement.IsReady("video"));
        Debug.Log(Advertisement.isInitialized);
        Debug.Log(Advertisement.isSupported);

        if (Advertisement.IsReady("Interstitial"))
        {
            Advertisement.Show("Interstitial");
        }

        while (Advertisement.isShowing) { yield return null; } 

        SceneManager.LoadScene(sceneName);
    }

    void CreateChunk(GameObject createChunk)
    {
        float xOffset = (previouseChunk != null) ? (createChunk.GetComponent<Chunk>().size.x - previouseChunk.size.x) / 2 : 0;
        Chunk currentChunk = Instantiate(createChunk, new Vector2(placePosition + xOffset, transform.position.y), Quaternion.identity).GetComponent<Chunk>();
        placePosition += createChunk.GetComponent<Chunk>().size.x;
        previouseChunk = currentChunk;

    }

    public void ChangeScore(int amount)
    {
        scoreAnimator.SetTrigger("Scored");
        score += amount * multiplier;
        scoreText.text = score.ToString();
    }

    public void ChangeMultiplier(float dashTime)
    {
        if (dashTime > multiplierThresholds[multiplier])
        {
            if (multiplier + 1 > 0)
            {
                multiplier += 1;
                multiplierText.text = multiplier.ToString() + "x";
            }
        }

        if (dashTime == 0)
        {
            
        }
    }

    void OnPlayerDead() 
    {
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        highScoreText.text = "HighScore: " + PlayerPrefs.GetInt("HighScore").ToString();

        playerDead = true; 
    }
}
