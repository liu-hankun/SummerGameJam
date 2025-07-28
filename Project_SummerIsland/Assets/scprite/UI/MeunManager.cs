using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeunManager : MonoBehaviour
{
    public GameObject CreditMenu;
    public GameObject ConrtrolMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Test");
    }

    public void openCredit()
    {
        CreditMenu.SetActive(true);
    }
    public void openControl()
    {
        ConrtrolMenu.SetActive(true);
    }
  
    public void backMeun()
    {
        CreditMenu.SetActive(false);
        ConrtrolMenu.SetActive(false);
    }
}
