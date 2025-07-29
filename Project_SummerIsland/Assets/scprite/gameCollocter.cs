using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class gameCollocter : MonoBehaviour
{
    public float gameTime = 180f;           
    private float currentTime;

    public TextMeshProUGUI timerText;      
    public GameObject endPanel;
    public GameObject UI1; // scoretext
    public GameObject UI2; // orderText
    public GameObject UI3; // inventoryText
    public GameObject UI4; // time
    public GameObject UI5; // pause panel
    

    private bool gameEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = gameTime;
        endPanel.SetActive(false);        
        Time.timeScale = 1f;
        UI1.SetActive(true);
        UI2.SetActive(true);
        UI3.SetActive(true);
        UI4.SetActive(true);
        UI5.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnded) return;

        currentTime -= Time.deltaTime;
        currentTime = Mathf.Clamp(currentTime, 0, gameTime);

        timerText.text = "Time Left: " + Mathf.CeilToInt(currentTime) + "s";

        if (currentTime <= 0f)
        {
            EndGame();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UI5.SetActive(true);
            Time.timeScale = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            UI5.SetActive(false);
            Time.timeScale = 1;
        }
    }
    void EndGame()
    {
        gameEnded = true;
        Time.timeScale = 0f;               
        endPanel.SetActive(true);
        UI1.SetActive(false);
        UI2.SetActive(false);
        UI3.SetActive(false);
        UI4.SetActive(false);
    }

    public void backtoMeun()
    {
        SceneManager.LoadScene("StartGame");
    }
    public void restartGame()
    {
        SceneManager.LoadScene("Test");
    }
    public void backtoGame()
    {
        UI5.SetActive(false);
        Time.timeScale = 1;
    }
   
}
