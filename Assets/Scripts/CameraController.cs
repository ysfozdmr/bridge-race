using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Bools")]
    public bool isLevelStart;
    public bool isLevelDone;
    public bool isLevelFail;

    [Header("Settings")]
    public Vector3 offSet;

    UIController UI;
    PlayerController Player;
    GameController GC;
    AIController AI;

    Transform PlayerTransform;
    Transform target;

    Vector3 targetPosition;
    Vector3 point;

    public float speedMod ;

    public static CameraController instance;

    public float smoothTime;

    private Vector3 velocity = Vector3.zero;

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
       
        PlayerTransform = PlayerController.instance.transform;

        target = PlayerTransform;

        

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        if (isLevelStart && !isLevelDone && !isLevelFail)
        {
            targetPosition = target.transform.position + offSet;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        if (isLevelDone)
        {
            point = target.transform.position;
            transform.LookAt(point);

            transform.RotateAround(point, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * speedMod);
        }
    }
}
