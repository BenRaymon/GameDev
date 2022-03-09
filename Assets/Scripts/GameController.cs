using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TMP_Text timer;
    private float timeRemaining = 3f;
    [SerializeField] private GameObject timeDisplay; // used to disable timer display

    [SerializeField] private GameObject platformGen; // Used to access platformgenerator object
    private PlatformGenerator platformGenScript; // Used to access platformgenerator script
    private string[] gameAges = {"Volcanic Platform", "Grass Platform", "END"};
    private int count = 0;

    public static string currentAge;

    void Awake()
    {
        platformGenScript = platformGen.GetComponent<PlatformGenerator>();
        platformGenScript.setTerrain(gameAges[count]); // initially sets the spawned platforms to volcanic
        currentAge = gameAges[count];
        count += 1;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(timeRemaining > 0f)
        {
            // changes display text
            float minutes = Mathf.FloorToInt(timeRemaining / 60);
            float seconds = Mathf.FloorToInt(timeRemaining % 60);
            timer.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));

            // counts down from the initial value of timeRemaining
            timeRemaining -= Time.deltaTime;
        }
        else
        {    
            currentAge = gameAges[count]; // sets current age
            Debug.Log("Change age to: " + gameAges[count]);

            if(currentAge != "END")
            {
                platformGenScript.setTerrain(gameAges[count]);
                count += 1;
                timeRemaining = 3f;
            }
            else
            {
                Debug.Log("WINNER!");
                Time.timeScale = 0f;
            }
        }
        */
    }
}
