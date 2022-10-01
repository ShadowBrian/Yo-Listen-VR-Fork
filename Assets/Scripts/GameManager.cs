using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Timer resetTimer;
    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 100;
        resetTimer = new Timer(10);

        timerText.text = "10";
    }

    // Update is called once per frame
    void Update()
    {
        if(resetTimer.Finished())
        {
            Debug.Log("Reset");
            ResetGame();
            
        }
        else
        {
            timerText.text= ""+((int)(resetTimer.GetTimeRemaining()*100))/100f;
        }
    }

    public void ResetGame(){
        SceneManager.LoadScene(0);
        //TODO BETTER RESET
    }
}
