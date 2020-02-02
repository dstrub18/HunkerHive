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
    [SerializeField] public GameObject mask;
    [SerializeField] public GameObject gameMenu;
    [SerializeField] public GameObject instructions;
    [SerializeField] public Button startButton;
    [SerializeField] public Button startTimerButton;
    [SerializeField] public List<GameObject> furniture;
    public GameObject queenBee;
    public Transform queenBeeOrigin;


    [Space]
    [Header("Timer")]
    [SerializeField] public GameObject timerObject;
    [SerializeField] public TextMeshProUGUI timeText;
    [SerializeField] public bool prepPhase;
    [SerializeField] public bool stormPhase;
    [SerializeField] public bool recoopPhase;
    [SerializeField] public float time;
    [SerializeField] public float prepTime;
    [SerializeField] public float stormTime;
    [SerializeField] public float recoopTime;

    [Space]
    [Header("PhaseChanges")]
    [SerializeField] public TextMeshProUGUI phaseText;
    [SerializeField] public bool timeReset;
    [SerializeField] public BeeSelector beeSelector;
    [SerializeField] public GameObject phaseSign;
    [SerializeField] public GameObject windMachine;
    [SerializeField] public Animator queenBar;
    [SerializeField] public Animator queen;

    [Space]
    [Header("Furniture")]
    public List<Transform> furnitureOriginPositions;
    public List<float> furnitureHP;
    [SerializeField] public int deathThreshold;
    [SerializeField] private Slider slider;
    public List<FurnitureOutline> furnitureOutlines;


    [Space]
    [Header("Wind Physics")]
    [SerializeField] public GameObject capsule;
    [SerializeField] public GameObject rope;

    [SerializeField] public Rigidbody2D rb_capsule;
    [SerializeField] public Rigidbody2D rb_rope;

    [SerializeField] public Vector3  capsule_originalPosition;
    [SerializeField] public Vector3  rope_originalPosition;

    [SerializeField] public Quaternion capsule_originalRotation;
    [SerializeField] public Quaternion rope_originalRotation;


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

        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        queenBeeOrigin = queenBee.transform;
        mask.SetActive(true);
        gameMenu.SetActive(true);
        startButton.onClick.AddListener(ShowInstructions);
        startTimerButton.onClick.AddListener(StartGame);
        //for(int index = 0; index < furniture.Count; index++)
        //{
        //    //Transform tempTrans = furniture[index].transform;
        //    //furnitureOriginPositions.Add(tempTrans);
        //    furnitureHP.Add(furniture[index].GetComponent<Furniture>().hp);
        //}
        //slider.maxValue = furniture.Count;

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

    public void CheckIfDead()
    {
        Debug.Log("Check if died");
        slider.value = slider.value - 0.1f;
        if (slider.value <= 0.8)
        {
            queen.SetTrigger("queenMad");
        }

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
            //for (int index = 0; index < furniture.Count; index++)
            //{
            //    if (furniture[index].GetComponent<FurnitureTrigger>().repaired == true)
            //    {
            //        furniture[index].GetComponent<SpriteRenderer>().enabled = true;
            //        furniture[index].GetComponent<Furniture>().hp = furniture[index].GetComponent<Furniture>().maxHp;
            //        furniture[index].GetComponent<FurnitureTrigger>().repaired = false;
            //    }
            //}
            if (time <= 0.0f)
            {
                beeSelector.animator.SetTrigger("Hide");
                phaseSign.SetActive(false);
                timeReset = false;
                prepPhase = false;
                stormPhase = true;
                foreach (FurnitureOutline furnitureOutline in furnitureOutlines)
                {
                    furnitureOutline.TurnOFFRB(true);
                }
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


            queenBee.transform.position = queenBeeOrigin.position;
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
                //for (int index = 0; index < furniture.Count; index++)
                //{
                //    furniture[index].transform.position = furnitureOriginPositions[index].position;
                //    furniture[index].transform.rotation = furnitureOriginPositions[index].rotation;
                //    if(furniture[index].GetComponent<Furniture>().hp <= 0.0f)
                //    {
                //        furniture[index].GetComponent<SpriteRenderer>().enabled = false;
                //    }

                //}
                timeReset = false;
                prepPhase = true;
                slider.value = slider.maxValue;
                foreach(FurnitureOutline furnitureOutline in furnitureOutlines)
                {
                    furnitureOutline.TurnOFFRB(false);
                }
                recoopPhase = false;
            }
        }
    }
}
