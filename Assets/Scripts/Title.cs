using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playText;
    [SerializeField] private Button playButton;
    [SerializeField] private Color preTutorialColor;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("HasDoneTutorial") == 0)
        {
            playButton.enabled = false;
            playText.color = preTutorialColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
