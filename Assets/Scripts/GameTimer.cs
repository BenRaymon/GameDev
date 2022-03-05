using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public TMP_Text timer;
    private float timeRemaining = 10f;
    [SerializeField] private GameObject timeDisplay; // used to disable timer display
    [SerializeField] private GameObject platformGen; // Used to access platformgenerator script


    // Update is called once per frame
    void Update()
    {
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
            // Changes the generated platform to Grass
            platformGen.GetComponent<PlatformGenerator>().setTerrain("Grass Platform");
        }
    }
}
