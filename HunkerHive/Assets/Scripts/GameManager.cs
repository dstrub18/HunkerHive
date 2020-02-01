using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [Header("Main Menu")]
    [SerializeField] private GameObject mask;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject instructions;
    [SerializeField] private Button startButton;
    [SerializeField] private Button startTimerButton;
    [SerializeField] private List<GameObject> furniture;
    private List<Transform> furnitureOriginPositions;



    [Space]
    [Header("Timer")]
    [SerializeField] private GameObject timerObject;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private bool prepPhase;
    [SerializeField] private bool stormPhase;
    [SerializeField] private bool recoopPhase;
    [SerializeField] private float time;
    [SerializeField] private float prepTime;
    [SerializeField] private float stormTime;
    [SerializeField] private float recoopTime;

    [Space]
    [Header("PhaseChanges")]
    [SerializeField] private GameObject phaseSign;
    [SerializeField] private TextMeshProUGUI phaseText;
    [SerializeField] private bool timeReset;
    [SerializeField] private BeeSelector beeSelector;
    [SerializeField] private GameObject windMachine;

    private 
    

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        mask.SetActive(true);
        gameMenu.SetActive(true);
        startButton.onClick.AddListener(ShowInstructions);
        startTimerButton.onClick.AddListener(StartGame);
        for(int index = 0; index < furniture.Count; index++)
        {
            furnitureOriginPositions.Add(furniture[index].transform);
        }
    }

    private void ShowInstructions() 
    {
        instructions.SetActive(true);
    }

    private void StartGame()
    {
        gameMenu.SetActive(false);
        instructions.SetActive(false);
        mask.SetActive(false);
        // Start Timer.
        timerObject.SetActive(true);
        StartTimer();
        // Show selection.
    }

    private void StartTimer() 
    {
        timerObject.SetActive(true);
        prepPhase = true;
        beeSelector.animator.SetTrigger("Show");
    }

    private void Update()
    {
        if (prepPhase) 
        {
            phaseText.text = "Prep Phase!";
            phaseSign.SetActive(true);
            if (!timeReset) 
            { 
                time = prepTime;
                timeReset = true;
            }
            time -= Time.deltaTime;
            timeText.text = Mathf.Round(time).ToString();
            if (time <= 0.0f)
            {
                beeSelector.animator.SetTrigger("Hide");
                phaseSign.SetActive(false);
                timeReset = false;
                prepPhase = false;
                stormPhase = true;
            }
        }

        if (stormPhase)
        {
            phaseText.text = "Storm Phase!";
            windMachine.SetActive(true);
            phaseSign.SetActive(true);
            if (!timeReset) 
            { 
                time = stormTime;
                timeReset = true;
            }
            time -= Time.deltaTime;
            timeText.text = Mathf.Round(time).ToString();
            if (time <= 0.0f)
            {
                windMachine.SetActive(false);
                phaseSign.SetActive(false);
                timeReset = false;
                recoopPhase = true;
                stormPhase = false;
            }

        }

        if (recoopPhase) 
        {
            phaseText.text = "Recoop Phase!";
            phaseSign.SetActive(true);
            if (!timeReset) 
            { 
                time = recoopTime;
                timeReset = true;
            }
            time -= Time.deltaTime;
            timeText.text = Mathf.Round(time).ToString();
            if (time <= 0.0f)
            {
                beeSelector.PopulateHive();
                beeSelector.animator.SetTrigger("Show");
                phaseSign.SetActive(false);
                for (int index = 0; index < furniture.Count; index++)
                {
                    furniture[index].transform.position = furnitureOriginPositions[index].position;
                    furniture[index].transform.rotation = furnitureOriginPositions[index].rotation;
                    if(furniture[index].GetComponent<Furniture>().hp <= 0.0f)
                    {
                        furniture[index].SetActive(false);
                    }
                }
                timeReset = false;
                prepPhase = true;
                recoopPhase = false;
            }
        }
    }
}
