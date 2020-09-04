using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gM = null;

    void Awake()
    {
        if (gM == null)
        {
            gM = this;
            DontDestroyOnLoad(this);
        }
        else if (this != gM)
        {
            Destroy(gameObject);
        }
    }

    [Header("Object Lists")]
    public List<GameObject> stageOneObjs=new List<GameObject>();
    public List<GameObject> stageTwoObj=new List<GameObject>();

    // For Add a New Level Please Drag A Level Prefab To This List.
    [Header("Level List")]
    public List<GameObject> levelList = new List<GameObject>();

    [Header("Booleans")]
    public bool isMovingToCenter;
    public bool isMovingToTheSecondStage;
    private bool closeGate;

    [Header("Positions")]
    private Vector3 holeStartingPos;
    private Vector3 camStartingPos;
    private Vector3 gateStartingPos;

    [Header("GameObjects")]
    public GameObject hole;
    public GameObject gate;
    public GameObject activeLevel;
    public GameObject firstParent;
    public GameObject secondParent;

    [Header("Counters")]
    private float stageOneCountHolder;
    private float stageTwoCountHolder;

    public float flexibleCamSpeed;

    private void Start()
    {
        activeLevel = Instantiate(levelList[Game.level]);
        Game.firstStage = true;
        Game.secondStage = false;
        firstParent = null;
        secondParent = null;
        holeStartingPos= new Vector3(0, -0.05f, -4);
        camStartingPos = new Vector3(0, 14, -8.5f);
        gateStartingPos = new Vector3(0, 1, 6);
        AddObjectsToLists();
        CalculateFillAmount();
        LevelProgress.lP.SetLevelTexts();
    }

    private void AddObjectsToLists()
    {
        firstParent = activeLevel.transform.GetChild(0).gameObject;
        secondParent = activeLevel.transform.GetChild(2).gameObject;

        stageOneObjs.Clear();
        stageTwoObj.Clear();

        int childrenCount = firstParent.transform.childCount;
        for (int i = 0; i < childrenCount; ++i)
        {
            if (firstParent.transform.GetChild(i).tag == "Obtainable")
                stageOneObjs.Add(firstParent.transform.GetChild(i).gameObject);
        }

        stageOneCountHolder = stageOneObjs.Count;

        childrenCount = secondParent.transform.childCount;
        for (int i = 0; i < childrenCount; ++i)
        {
            if (secondParent.transform.GetChild(i).tag == "Obtainable")
                stageTwoObj.Add(secondParent.transform.GetChild(i).gameObject);
        }

        stageTwoCountHolder = stageTwoObj.Count;
    }
    
    public void RestartLevel()
    {
        foreach(GameObject levelPrefab in GameObject.FindGameObjectsWithTag("Level"))
        {
            Destroy(levelPrefab);
        }

        Time.timeScale = 1;
        hole.transform.position = holeStartingPos;
        activeLevel=Instantiate(levelList[Game.level]);
        AddObjectsToLists();
        Game.firstStage = true;
        Game.secondStage = false;
        CalculateFillAmount();
    }
    public void StageOneToStageTwo()
    {

        Game.firstStage = false;
        StartCoroutine(MoveHoleForSecondStage());
    }
    private IEnumerator MoveHoleForSecondStage()
    {
        hole.layer = LayerMask.NameToLayer("Default");
        hole.GetComponent<MoveHole>().isControllerActive = false;

        while (gate.transform.position.y > -1)
        {
            closeGate = true;
            yield return null;
        }
        closeGate = false;
        while (hole.transform.position.x != 0)
        {
            isMovingToCenter = true;
            yield return null;
        }

        isMovingToCenter = false;

        float holePosZ = hole.transform.position.z; // For calculating speed of camera accoring to hole distance to the target
        flexibleCamSpeed = 51 / (13 - holePosZ);

        while(hole.transform.position.z != 19 || Camera.main.transform.position.z !=14)
        {
            isMovingToTheSecondStage = true;

            if (hole.transform.position.z > 6)
            {
                hole.layer = LayerMask.NameToLayer("Ground3DCollider");
            }

            yield return null;
        }

        isMovingToTheSecondStage = false;
        hole.GetComponent<MoveHole>().isControllerActive = true;

        Game.secondStage = true;
    }
    public void LoadNextLevel()
    {
        foreach (GameObject levelPrefab in GameObject.FindGameObjectsWithTag("Level"))
        {
            Destroy(levelPrefab);
        }

        Game.firstStage = false;
        Game.secondStage = false;

        if (Game.level == levelList.Count-1) {
            Debug.Log("Bitti oyun");
        }
        else
        {
            Game.level++;
            Camera.main.transform.position = camStartingPos;
            hole.transform.position = holeStartingPos;
            gate.transform.position = gateStartingPos;
            activeLevel=Instantiate(levelList[Game.level]);
            AddObjectsToLists();
            CalculateFillAmount();
            LevelProgress.lP.SetLevelTexts();
            Game.firstStage = true;
        }

    }

    // Calculates level progress image's fill amount. 
    public void CalculateFillAmount()
    {
        float fill;

        if (Game.firstStage)
        {
            fill = 1 - (stageOneObjs.Count / stageOneCountHolder);
            Debug.Log(fill);
            LevelProgress.lP.FillImage(fill);
        }
        else if (Game.secondStage)
        {
            fill = 1 - (stageTwoObj.Count / stageTwoCountHolder);
            LevelProgress.lP.FillImage(fill);
        }
        else
        {
            LevelProgress.lP.FillImage(0);
        }

    }

    // Opens gate for stage-one to stage-two process.
    private void Update()
    {
        if (closeGate)
        {
            gate.transform.position -= new Vector3 (0, 1f * Time.deltaTime, 0);
        }
    }
}
