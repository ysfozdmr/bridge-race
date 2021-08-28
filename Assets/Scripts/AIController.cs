using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*
 * to do list söyle kanka
 * 
 * sýrttakileri silme
 * basamak triggerlarý
 * 
 * ileri gitme boolu false olucak sýrttakiler bitince
 * 
 * geriye gitmek için bool ile hýz verilcek
 * 
 * player içinde lefttriggeredsteplist allsteplist yapýlacak
 * 
  */
public class AIController : MonoBehaviour
{
    [Header("Bools")]
    public bool isLevelStart;
    public bool isLevelDone;
    public bool isLevelFail;
    public bool isOnLadder;

    [Header ("Tags")]
    string TagRedBoxes;
    string TagLadderTrigger;
    string TagStepsTrigger;

    public NavMeshAgent redAgent;


    GameController GC;
    CameraController Camera;
    UIController UI;
    PlayerController Player;

    public GameObject redStep;

    Vector3 TempPosition;
    Vector3 redAITemp;

    public Transform RedBoxesStepContainer;
    public Transform leftLadderTrigger;
    public Transform rightLadderTrigger;

    public Material redAgentMaterial;

    int ladderSelection;
    public float distY;
    public float lerpSpeed;

    public List<GameObject> leftLadderTriggeredStep = new List<GameObject>();
    public List<GameObject> leftLadderAllSteps = new List<GameObject>();
    public List<GameObject> redStepContainerList = new List<GameObject>();
    public List<GameObject> redBoxes = new List<GameObject>();
    public List<GameObject> matchedRedBoxes = new List<GameObject>();
    List<GameObject> allStepsList;
    List<GameObject> triggeredStepsList;

    public static AIController instance;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartMethods();

        redAgent = this.GetComponent<NavMeshAgent>();
    }

    void StartMethods()
    {
        GC = GameController.instance;
        Camera = CameraController.instance;
        UI = UIController.instance;
        Player = PlayerController.instance;
        navMeshSettings();
        getTags();

        ladderSelection = Random.Range(0, 2);
    }

    void navMeshSettings()
    {
        
            int d = Random.Range(0, redBoxes.Count);

            while (!redBoxes[d] == enabled)
            {
                d = Random.Range(0, redBoxes.Count);
            }

            redAgent.SetDestination(redBoxes[d].transform.position);
            
    }
    void getTags()
    {
        TagRedBoxes = GC.TagRedBoxes;
        TagLadderTrigger = GC.TagLadderTrigger;
        TagStepsTrigger = GC.TagStepsTrigger;
        allStepsList = Player.allStepsList;
        triggeredStepsList = Player.triggeredStepsList;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(TagRedBoxes)){
            
            other.gameObject.SetActive(false);

            matchedRedBoxes.Add(other.gameObject);

            redStepContainerList.Add(redStep);
            StepSpawner();
            redBoxes.Remove(other.gameObject);

            int random = Random.Range(8, 11);

            int destinationEquation = random*matchedRedBoxes.Count;

            if (destinationEquation >= 60)
            {
                if (ladderSelection == 0)
                {
                    redAgent.SetDestination(rightLadderTrigger.position);
                }
                else
                {
                    redAgent.SetDestination(leftLadderTrigger.position);
                }
            }

        }
        if (other.gameObject.CompareTag(TagStepsTrigger))
        {
            if (matchedRedBoxes.Count > 0)
            {
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;

                

                redAITemp = transform.position;

                redAITemp.y += 0.5f;

                transform.position = Vector3.Lerp(transform.position, redAITemp, lerpSpeed);

                other.GetComponent<Renderer>().material.SetColor("_Color", redAgentMaterial.color);

                for(int i = 0; i < leftLadderAllSteps.Count; i++)
                {
                    if (!leftLadderTriggeredStep.Contains(other.gameObject))
                    {
                        matchedRedBoxes.RemoveAt(matchedRedBoxes.Count - 1);
                        leftLadderTriggeredStep.Add(other.gameObject);

                        redStepContainerList[redStepContainerList.Count - 1].SetActive(false);
                        redStepContainerList.RemoveAt(redStepContainerList.Count - 1);
                    }

                }
            }
        }
        if ( other.gameObject.CompareTag(TagLadderTrigger))
        {

            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            // söyle tekrar redStepContainer.Count>0 içine almamýz lazým 
            isOnLadder = true;
            for (int i = 0; i < matchedRedBoxes.Count; i++)
            {
                matchedRedBoxes[i].SetActive(true);
                redBoxes.Add(matchedRedBoxes[i]);
                matchedRedBoxes.RemoveAt(i);
                
            }
            if (redStepContainerList.Count == 0)
            {//geri dönüþü burda sýfýrla
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
            }
            
            
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(TagStepsTrigger))
        {
            if (redStepContainerList.Count == 0)
            {
                redAITemp = transform.position;

                redAITemp.y -= 0.5f;

                transform.position = Vector3.Lerp(transform.position, redAITemp, lerpSpeed);
            }
        }
    }
    void StepSpawner()
    {
        TempPosition = RedBoxesStepContainer.position;
        TempPosition.y += redStepContainerList.Count * distY;
        redStep = Instantiate(redStep, TempPosition, Quaternion.identity, RedBoxesStepContainer);
        

    }

    // Update is called once per frame
    void Update()
    {
       if (redAgent.remainingDistance < 0.2f )
       {
         navMeshSettings();
       }
        if (isOnLadder)
        {
            redAITemp = transform.position;
            redAITemp.z += 7f * Time.deltaTime;//

            transform.position = redAITemp;
        }
        
    }
}
