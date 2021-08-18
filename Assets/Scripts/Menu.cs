using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI deathScoreText;
    [SerializeField] private TextMeshProUGUI deathHighScoreText;

    private GameMaster gm;

    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerController.PlayerDead += OnPlayerDeath;
    }

    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    private void OnDisable()
    {
        PlayerController.PlayerDead -= OnPlayerDeath;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlayerDeath()
    {
        deathMenu.SetActive(true);
        scoreText.gameObject.SetActive(false);

        deathScoreText.text = "Score: " + scoreText.text;
    }
}
