using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
	private Animator fadeAnimator;
	private GameObject player;
	private GameObject gameController;
	private GameObject generator;

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
		PlayerPrefs.SetInt("xCoord", (int)player.transform.position.x);
	}

	public void fadeIn()
	{		
		fadeAnimator.SetInteger("Fade", 1);
	}

	public void onFadeOutComplete()
	{
		SceneManager.LoadScene(3);
		fadeIn();
	}

	public void onFadeInComplete()
	{
		gameController.GetComponent<GameController>().toggleScoring();		
	}
}
