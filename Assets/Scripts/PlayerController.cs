using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    #region Definations and Settings
    [Header("Bools")]
    public bool isLevelStart;
    public bool isLevelDone;
    public bool isLevelFail;
    public bool isMovingUp;
    public bool leftDoorOpenBool;
    public bool rightDoorOpenBool;
    public bool isLastStep;

    [Header("Tags")]
    string TagFinish;
    string TagMatchingBox;
    string TagLadderTrigger;
    string TagStepsTrigger;
    string TagNextGroundTrigger;
    string TagLastWalkTrigger;


    [Header("Speed Settings")]
    public float speed;
    public float distY;
    public float lerpSpeed;

    
    [Header ("Vector Settings")]
    public Vector3 pos ;
    public Vector3 TempPosition;
    public Vector3 PlayerStepPosition;
    public Vector3 Deneme=Vector3.zero;

    
    

    [Header("Materials")]
    public Material playerMaterial;

    [Header ("GameObject Settings")]
    public GameObject Collectable;
    public GameObject Step;
    public GameObject sContainer;
    public GameObject leftDoor;
    public GameObject rightDoor;

    [Header ("Transforms")]
    public Transform StepContainer;

    [Header("Lists")]
    public List<GameObject> CollectedBoxes = new List<GameObject>() ;
    public List<GameObject> allStepsList = new List<GameObject>();
    public List<GameObject> triggeredStepsList = new List<GameObject>();
    public List<GameObject> stepContainerList = new List<GameObject>();
    List<GameObject> leftLadderAllSteps;
    List<GameObject> leftLadderTriggeredStep;

    UIController UI;
    GameController GC;
    CameraController Camera;
    AIController AI;

    public Rigidbody rb;

    public static PlayerController instance;
    #endregion

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
        Camera = CameraController.instance;
        UI = UIController.instance;
        AI = AIController.instance;
        getTags();
        Collectable = GameObject.FindWithTag("MatchingBox");
        

    }

    void getTags()
    {
        TagFinish = GC.TagFinish;
        TagMatchingBox = GC.TagMatchingBox;
        TagLadderTrigger = GC.TagLadderTrigger;
        TagStepsTrigger = GC.TagStepsTrigger;
        TagNextGroundTrigger = GC.TagNextGroundTrigger;
        TagLastWalkTrigger = GC.TagLastWalkTrigger;
        leftLadderAllSteps = AI.leftLadderAllSteps;
        leftLadderTriggeredStep = AI.leftLadderTriggeredStep;
    }
    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagFinish))
        {
            GC.CompleteLevel();

        }
        else if (other.CompareTag(TagMatchingBox))
        {
            other.gameObject.SetActive(false);

            CollectedBoxes.Add(other.gameObject);

            StepSpawner();
            stepContainerList.Add(Step);

            for (int i = 0; i < allStepsList.Count; i++)
            {
                if (!triggeredStepsList.Contains(other.gameObject))
                {

                    stepContainerList[stepContainerList.Count - 1].SetActive(true);

                }

            }


        }
        else if (other.CompareTag(TagLadderTrigger))
        {

            for (int i = 0; i < CollectedBoxes.Count; i++)
            {
                CollectedBoxes[i].gameObject.SetActive(true);
            }


        }
        else if (other.CompareTag(TagStepsTrigger))
        {

            if (CollectedBoxes.Count > 0 || triggeredStepsList.Contains(other.gameObject))
            {
                if (isMovingUp)
                {
                    other.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    PlayerStepPosition = transform.position;

                    PlayerStepPosition.y += 0.5f;
                    transform.position = Vector3.Lerp(transform.position, PlayerStepPosition, lerpSpeed);

                    other.GetComponent<Renderer>().material.SetColor("_Color", playerMaterial.color);

                    for (int i = 0; i < allStepsList.Count; i++)
                    {
                        if (!triggeredStepsList.Contains(other.gameObject))
                        {
                            CollectedBoxes.RemoveAt(CollectedBoxes.Count - 1);
                            triggeredStepsList.Add(other.gameObject);

                            stepContainerList[stepContainerList.Count - 1].SetActive(false);
                            stepContainerList.RemoveAt(stepContainerList.Count - 1);

                        }

                    }




                }

            }

            if (triggeredStepsList.Count == allStepsList.Count)
            {
                leftDoor.GetComponent<Animator>().SetBool("leftDoorOpenBool", true);
                rightDoor.GetComponent<Animator>().SetBool("rightDoorOpenBool", true);

                isLastStep = true;
            }

        }
        else if (other.gameObject.CompareTag(TagNextGroundTrigger))
        {
            if (Input.GetKey("s"))
            {
                speed = 0f;
            }
            speed = 7f;
            PlayerStepPosition = transform.position;

            PlayerStepPosition.y += 0.5f;
            transform.position = Vector3.Lerp(transform.position, PlayerStepPosition, lerpSpeed);


            triggeredStepsList.Clear();
            isLastStep = false;
            
        }
        else if (other.gameObject.CompareTag(TagLastWalkTrigger))
        {
            
            PlayerStepPosition = transform.position;
            PlayerStepPosition.z += 4f;
            transform.position = Vector3.Lerp(transform.position,PlayerStepPosition,lerpSpeed);
            

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagStepsTrigger))
        {
            
            if (!isMovingUp)
            {
                PlayerStepPosition = transform.position;
                PlayerStepPosition.y -= 0.5f;
                transform.position = Vector3.Lerp(transform.position, PlayerStepPosition, lerpSpeed);
            }
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TagStepsTrigger))
        {
            if (triggeredStepsList.Contains(other.gameObject)) {
                if (CollectedBoxes.Count == 0)
                {
                    if (Input.GetKey("w") && !isLastStep)
                    {
                        speed = 0f;
                    }
                    else
                    {
                        speed = 7f;
                    }
                }
            }
        }
    }
    
    #endregion
    

    void StepSpawner()
    {
        
        TempPosition = StepContainer.position;
        TempPosition.y += stepContainerList.Count * distY;
        Step = Instantiate(Step,TempPosition,Quaternion.identity,StepContainer);
        
    }

    void Moving()
    {
        if (isLevelStart && !isLevelDone && !isLevelFail)
        {

            pos = transform.position;


            if (Input.GetKey("w"))
            {
                 pos.z += speed * Time.deltaTime;
                
                isMovingUp = true;
            }
             if (Input.GetKey("s"))
            {
                pos.z -= speed * Time.deltaTime;
                
                isMovingUp = false;
            }
             if (Input.GetKey("d"))
            {

                pos.x += speed * Time.deltaTime;
                
            }
             if (Input.GetKey("a"))
            {
                pos.x -= speed * Time.deltaTime;
            }


            transform.position = pos;
        }
    }
   



    // Update is called once per frame
    void FixedUpdate()
    {
        Moving();



    }
        
    
}
