using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public Timer resetTimer;
    public TextMeshProUGUI timerText;
    GameObject playerCopy;
    public int restarts = 0;
    public GameObject copy;

    public GameObject player;

    public GameObject startFairy;
    public List<ResetInteractable> resets = new List<ResetInteractable>();

    public static GameManager instance;

    bool _startGame;
    public bool startGame{
        get
        {
            return _startGame;
        }
        set{
            if(value)
            {
                resetTimer = new Timer(10);
            }
            _startGame = value;
        }
    }
    public Timer gameTime;

    public GameObject winScreen;
    public bool won = true;

    public bool resetting = false;

    public TextMeshProUGUI status, winCondition, time;
    // Start is called before the first frame update
    void Start()
    {
        if(instance) return;
        gameTime = new Timer(0);
        instance =this;
        Application.targetFrameRate = 100;
        //resetTimer = new Timer(10);
        timerText.text = "";
        resets = new List<ResetInteractable>();
        GetResets(transform);
        copy = Resources.Load<GameObject>("GameManager");
        copy.GetComponent<GameManager>().resets = new List<ResetInteractable>();
        copy.GetComponent<GameManager>().GetResets(copy.transform);
        startFairy.SetActive(false);
        playerCopy = Instantiate(player);
        playerCopy.SetActive(false);
    }

    public void CreateFairy(Vector3 position)
    {
        Debug.Log("START GAME");
        startFairy.SetActive(true);
        startFairy.transform.position = position;
        startFairy.GetComponent<FairyController>().BeginGameDialog();
    }

        // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.R)) Restart();
        //if(Input.GetKeyDown(KeyCode.F)) Screen.fullScreen = !Screen.fullScreen;
        //if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if(!startGame || won || resetting) return;
        if(resetTimer == null)
        {
           // if(Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1)||Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0  || Input.GetButton("Jump"))
            //{
            //    resetTimer = new Timer(10);
            //}

            if (UnityXRInputBridge.instance.GetVel(XRHandSide.LeftHand).magnitude > 0 || UnityXRInputBridge.instance.GetVel(XRHandSide.RightHand).magnitude > 0)
            {
                resetTimer = new Timer(10);
            }
        }
        else if(resetTimer.Finished() && !won)
        {   
            StartReset(); 
        }
        else if(resetTimer.GetTimeRemaining() > 0)
        {
            timerText.text= ""+((int)(resetTimer.GetTimeRemaining()*100))/100f;
        }
    }

    public void GetResets(Transform origin)
    {
        for(int i = 0; i < origin.childCount; i++)
        {
            // Debug.Log(i);
            GetResets(origin.GetChild(i));
        }
        if(origin.GetComponent<ResetInteractable>())
        {
            resets.Add(origin.GetComponent<ResetInteractable>());
        }
    }


    public void Win()
    {
        Win("...by evicting an unwanted guest!");
    }

    public AudioClip winSound, loseSound;
    public void PlayVictory()
    {
        GetComponent<AudioSource>().clip = winSound;
        GetComponent<AudioSource>().Play();
    }

    public void PlayDefeat()
    {
        GetComponent<AudioSource>().clip = loseSound;
        GetComponent<AudioSource>().Play();
    }
    public void Win(string condition)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        resetting = true;
        status.text = "You Won";
        winScreen.SetActive(true);
        winScreen.transform.localScale = Vector3.zero;
        winScreen.transform.DOScale(1, 0.5f);

        float tr = - gameTime.GetTimeRemaining();

        int minutes = (int)(tr/60);
        tr -= minutes * 60;
        int seconds = (int)tr;
        string secondsText = ((seconds<10)? "0":"") +seconds;
        time.text = "Total Time: "+minutes+":"+secondsText;
        winCondition.text = condition;
        won = true;
        PlayVictory();
    }

    public void Restart()
    {
        winScreen.transform.DOScale(0, 0.5f);
        RendererController.instance.SceneSwap();
    }

    
    public void Explode()
    {
        var gameObject = Instantiate(Resources.Load<GameObject>("ExplosionVFX"));
        Destroy(gameObject, 1);
        if(FairyController.instance.gameObject.activeSelf)
        {
            gameObject.transform.position = FairyController.instance.transform.position;
        }    
        else{
            gameObject.transform.position = MilkJarObject.instance.transform.position;
        }
        PlayerController.instance.ScreenShake();
    }
    public IEnumerator IReset()
    {
        timerText.text = "";
        resetting = true;
        RendererController.instance.DeathAnimation();
        var gameObject = Instantiate(Resources.Load<GameObject>("ExplosionVFX"));
        Destroy(gameObject, 1);
        if(FairyController.instance.gameObject.activeSelf)
        {
            gameObject.transform.position = FairyController.instance.transform.position;
        }    
        else{
            gameObject.transform.position = MilkJarObject.instance.transform.position;
        }
        PlayerController.instance.ScreenShake();
        yield return new WaitForSeconds(0.5f);
        ResetGame();
        yield return new WaitForSeconds(0.5f);
        resetting = false;
    }

    public void StartReset()
    {
        restarts++;
        StartCoroutine(IReset());
    }



    public void Lose(string condition)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        resetting = true;
        status.text = "You Lost";
        winScreen.SetActive(true);
        winScreen.transform.localScale = Vector3.zero;
        winScreen.transform.DOScale(1, 0.5f);

        float tr = - gameTime.GetTimeRemaining();

        int minutes = (int)(tr/60);
        tr -= minutes * 60;
        int seconds = (int)tr;
        string secondsText = ((seconds<10)? "0":"") +seconds;
        time.text = "Total Time: "+minutes+":"+secondsText;
        winCondition.text = condition;
        PlayDefeat();
        won = true;
    }


    public void ResetSweep()
    {
        for(int i = 0; i < resets.Count; i++)
        {
            if(!resets[i].GetLastInteraction())
            {
                SwitchAsset(i);
            }
            else if(resets[i].GetComponent<GrabInteractable>() && resets[i].GetComponent<GrabInteractable>().grabbed)
            {
                resets[i].GetComponent<GrabInteractable>().Release();
            }
        }
    }

    public void SwitchAsset(int position)
    {
        var newInstance = Instantiate(copy.GetComponent<GameManager>().resets[position].gameObject, resets[position].transform.parent);
        Destroy(resets[position].gameObject);
        resets[position] = newInstance.GetComponent<ResetInteractable>();
    }

    public void ResetGame(){
        ResetSweep();
        Destroy(player);
        player = Instantiate(playerCopy);
        player.SetActive(true);
        resetTimer = null;
    }
}
