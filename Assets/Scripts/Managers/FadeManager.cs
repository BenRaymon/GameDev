using UnityEngine;

public class FadeManager : MonoBehaviour
{
	private Animator fadeAnimator;
	private GameObject player;

	[SerializeField] private GameObject bossTeleporter;

    void Awake()
	{
		fadeAnimator = GetComponent<Animator>();		
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
		player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		player.GetComponent<ImprovedMovement>().canUpdateScore = false;
	}

	public void fadeIn()
	{
		//player = GameObject.FindGameObjectWithTag("Player");
		fadeAnimator.SetInteger("Fade", 1);
	}

	public void onFadeOutComplete()
	{
		Vector3 teleportPos = bossTeleporter.transform.position;
		player.transform.position = teleportPos;
		fadeIn();
	}

	public void onFadeInComplete()
	{
		player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
	}
}
