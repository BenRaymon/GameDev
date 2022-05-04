using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
	private Animator fadeAnimator;
	private GameObject player;
	private GameObject gameController;
	private GameObject generator;
	[SerializeField] private GameObject proceduralGenerator;

    void Awake()
	{
		fadeAnimator = GetComponent<Animator>();
		gameController = GameObject.FindGameObjectWithTag("GameController");
	}
	
	void Update()
	{
		if(Input.GetMouseButtonDown(1))
		{
			fadeOut();
		}

		if(player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player");
		}
	}

	public void fadeOut()
	{
		fadeAnimator.SetInteger("Fade", 0);
		if(SceneManager.GetActiveScene().name == "Level01")
		{
			int newCoord = proceduralGenerator.GetComponent<ProceduralGeneration>().getXCoord();
			PlayerPrefs.SetInt("xCoord", newCoord);
			Debug.Log("Saving xCoord: " + newCoord);
		}
	}

	public void fadeIn()
	{		
		fadeAnimator.SetInteger("Fade", 1);
	}

	public void onFadeOutComplete()
	{
		if(SceneManager.GetActiveScene().name == "BossArena")
		{
			SceneManager.LoadScene("Level01");
		}
		else
		{
			SceneManager.LoadScene(3);
		}
	}

	public void onFadeInComplete()
	{
		gameController.GetComponent<GameController>().toggleScoring();		
	}
}
