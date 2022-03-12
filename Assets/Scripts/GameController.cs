using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TMP_Text timer;

    private float TIME_BETWEEN_AGES = 20f;
    private float timeRemaining = 0f;

    [SerializeField] private GameObject timeDisplay; // used to disable timer display
    [SerializeField] private GameObject proceduralGenerator;

    private string[] gameAges = {"Volcanic Terrain", "Grass Terrain", "END"};
    private int count = 0;
    public static string currentAge;

    void Awake()
    {
        timeRemaining = TIME_BETWEEN_AGES;

        currentAge = gameAges[count];
        count++;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAge != "END")
        {
            checkGame();
        }
        else
        {
            endGame();
        }
    }

    private void checkGame()
    {
        Debug.Log("Checking game");
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
            Debug.Log("Change age to: " + currentAge);

            proceduralGenerator.GetComponent<ProceduralGeneration>().setTerrain(currentAge);
            count += 1;
            timeRemaining = TIME_BETWEEN_AGES;
        }
    }

    private void endGame()
    {
        Debug.Log("WINNER!");
        //Time.timeScale = 0f;
    }
}
