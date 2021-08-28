using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Bools")]
    public bool isLevelStart;
    public bool isLevelDone;
    public bool isLevelFail;

    [Header("Tags")]
    public string TagFinish;
    public string TagMatchingBox;
    public string TagLadderTrigger;
    public string TagStepsTrigger;
    public string TagNextGroundTrigger;
    public string TagLastWalkTrigger;
    public string TagRedBoxes;
    


    UIController UI;
    PlayerController Player;
    CameraController Camera;
    AIController AI;

    

    public static GameController instance;

    private void Awake()
    {
        if(!instance)
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
        UI = UIController.instance;
        Player = PlayerController.instance;
        Camera = CameraController.instance;
        AI = AIController.instance;
        

    }

    #region TapToStart

    public void TapToStartAction()
    {
        SendLevelStart();
    }
    void SendLevelStart()
    {
        UI.isLevelStart = true;
        isLevelStart = true;
        Camera.isLevelStart = true;
        Player.isLevelStart = true;
        AI.isLevelStart = true;

    }
    #endregion

    public void CompleteLevel()
    {
        UI.LevelComplated.SetActive(true);
        isLevelDone = true;
        UI.isLevelDone = true;
        Camera.isLevelDone = true;
        Player.isLevelDone = true;
        AI.isLevelDone = true;
    }

    


    // Update is called once per frame
    void Update()
    {
        
    }
}
