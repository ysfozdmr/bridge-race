using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour

{

    [Header("Bools")]
    public bool isLevelStart;
    public bool isLevelDone;
    public bool isLevelFail;

    GameController GC;
    PlayerController Player;
    CameraController Camera;
    AIController AI;

    public static UIController instance;

    public GameObject PanelTapToStart;

    public GameObject LevelComplated;

   

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }


    
    void Start()
    {

       StartMethods();

        }

    void StartMethods()
    {
        GC = GameController.instance;
        Player = PlayerController.instance;
        Camera = CameraController.instance;
        ShowTapToStart();
        

    }
    void ShowTapToStart()
    {
        PanelTapToStart.SetActive(true);
    }

    void CloseTapToStart()
    {
        PanelTapToStart.SetActive(false);
    }

    public void ButtonActionTapToStart()
    {
        CloseTapToStart();
        GC.TapToStartAction();

    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
