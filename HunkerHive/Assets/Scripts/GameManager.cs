using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
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
    [SerializeField] private Animator queenBar;
    [SerializeField] private Animator queen;

    [Space]
    [Header("Furniture")]
    private List<Transform> furnitureOriginPositions;
    private List<float> furnitureHP;
    [SerializeField] private int deathThreshold;
    [SerializeField] private Slider slider;


    [Space]
    [Header("Wind Physics")]
    [SerializeField] public GameObject capsule;
    [SerializeField] private GameObject rope;

    [SerializeField] private Rigidbody2D rb_capsule;
    [SerializeField] private Rigidbody2D rb_rope;

    [SerializeField] private Vector3  capsule_originalPosition;
    [SerializeField] private Vector3  rope_originalPosition;

    [SerializeField] private Quaternion capsule_originalRotation;
    [SerializeField] private Quaternion rope_originalRotation;


    [Space]
    [Header("Music Variables")]

    public AudioSource repairLoopSource;
    public AudioSource stormMusic;

    public AudioMixerSnapshot inRepairPhase;
    public AudioMixerSnapshot inStormPhase;

    [Range (0.5f, 4.0f)]
    public float transitionTime;

    
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
            furnitureHP.Add(furniture[index].GetComponent<Furniture>().hp);
        }
        slider.maxValue = furniture.Count;

        rb_capsule = capsule.GetComponent<Rigidbody2D>();
        rb_rope = rope.GetComponent<Rigidbody2D>();
        rb_capsule.bodyType = RigidbodyType2D.Static;
        rb_rope.bodyType = RigidbodyType2D.Static;

        capsule_originalPosition = capsule.transform.position;
        rope_originalPosition = rope.transform.position;

        capsule_originalRotation = capsule.transform.rotation;
        rope_originalRotation = capsule.transform.rotation;


        repairLoopSource.Play();
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

        rb_capsule.bodyType = RigidbodyType2D.Static;
        rb_rope.bodyType = RigidbodyType2D.Static;
        rb_capsule.velocity = Vector3.zero;
        rb_rope.velocity = Vector3.zero;
    }

    private void StartTimer() 
    {
        timerObject.SetActive(true);
        prepPhase = true;
        beeSelector.animator.SetTrigger("Show");
    }

    private void CheckIfDead()
    {
        var zeroCount = 0;
        foreach(float hp in furnitureHP)
        {
            if(hp <= 0)
            {
                zeroCount++;
            }
        }
        if(zeroCount >= deathThreshold)
        {
            queen.SetTrigger("queenMad");
        }
        slider.value = furniture.Count - zeroCount;
    }

    private void Update()
    {
        if (prepPhase) 
        {
            rb_capsule.bodyType = RigidbodyType2D.Static;
            rb_rope.bodyType = RigidbodyType2D.Static;
            rb_capsule.velocity = Vector3.zero;
            rb_rope.velocity = Vector3.zero;

            phaseText.text = "Prep Phase!";
            phaseSign.SetActive(true);
            if (!timeReset) 
            { 
                time = prepTime;
                timeReset = true;
            }
            time -= Time.deltaTime;
            timeText.text = Mathf.Round(time).ToString();
            for (int index = 0; index < furniture.Count; index++)
            {
                if (furniture[index].GetComponent<FurnitureTrigger>().repaired == true)
                {
                    furniture[index].SetActive(true);
                    furniture[index].GetComponent<Furniture>().hp = furniture[index].GetComponent<Furniture>().maxHp;
                    furniture[index].GetComponent<FurnitureTrigger>().repaired = false;
                }
            }
            if (time <= 0.0f)
            {
                beeSelector.animator.SetTrigger("Hide");
                phaseSign.SetActive(false);
                timeReset = false;
                prepPhase = false;
                stormPhase = true;
                inStormPhase.TransitionTo(transitionTime);
                stormMusic.Play();
                queenBar.SetTrigger("Show");
            }
        }

        if (stormPhase)
        {


            rb_capsule.bodyType = RigidbodyType2D.Dynamic;
            rb_rope.bodyType = RigidbodyType2D.Dynamic;


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
                queenBar.SetTrigger("Hide");
                windMachine.SetActive(false);
                phaseSign.SetActive(false);
                timeReset = false;
                recoopPhase = true;
                stormPhase = false;
                inRepairPhase.TransitionTo(transitionTime);
            }

        }

        if (recoopPhase) 
        {
            capsule.transform.position = capsule_originalPosition;
            rope.transform.position = rope_originalPosition;

            capsule.transform.rotation = capsule_originalRotation;
            rope.transform.rotation = rope_originalRotation;



            rb_capsule.bodyType = RigidbodyType2D.Static;
            rb_rope.bodyType = RigidbodyType2D.Static;
            rb_capsule.velocity = Vector3.zero;
            rb_rope.velocity = Vector3.zero;
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
