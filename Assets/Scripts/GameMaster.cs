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
    private float score;

    private void Start()
    {
        placePosition = transform.position.x;
        CreateChunk(firstChunk);

        scoreText.text = score.ToString();
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
        score += amount;
        scoreText.text = score.ToString();
    }
}
