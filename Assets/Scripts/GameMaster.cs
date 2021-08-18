using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private GameObject firstChunk;
    [SerializeField] private GameObject[] chunks;

    private List<GameObject> worldChunks;

    private float placePosition;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private int[] multiplierThresholds;
    private int score;
    private int multiplier = 1;
    

    private void Start()
    {
        placePosition = transform.position.x;
        CreateChunk(firstChunk);

        scoreText.text = score.ToString();

        multiplierText.text = multiplier.ToString() + "x";
    }

    private void OnEnable()
    {
        PlayerController.PlayerDead += OnPlayerDead;
    }

    private void OnDisable()
    {
        PlayerController.PlayerDead -= OnPlayerDead;
    }

    private void Update()
    {
        CreateChunk(chunks[Random.Range(0, chunks.Length)]);
    }

    public void ResetScene()
    {
        //Replace this with an actual scene transition
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void CreateChunk(GameObject createChunk)
    {
        Instantiate(createChunk, new Vector2(placePosition, transform.position.y), Quaternion.identity);
        placePosition += createChunk.GetComponent<Chunk>().size.x;
    }

    public void ChangeScore(int amount)
    {
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
    }
}
