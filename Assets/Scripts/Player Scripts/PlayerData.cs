using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
	[Header("Ground Buffer")]
	[Tooltip("Used for extending the player's ground check raycast further")]
    public float GROUND_BUFFER; // used for extra height test for grounding check

	[Header("Jump")]
	public float JUMP_FORCE;
	[Tooltip("Used for jump queueing, enabling the player to jump before landing.")]
	public float JUMP_QUEUE;
	[Tooltip("Allows the player to jump after they have left the ground if pressed within the specified time frame")]
	[Range(0, 0.5f)] public float COYOTE_TIME;
	
	[Header("Gravity")]
	public float GRAVITY;
	public float FALL_GRAVITY_MULT;

	[Header("Run")]
	public float MOVE_SPEED;
	public float RUN_ACCEL;
	public float RUN_DECELL;
	[Space(5)]
	[Range(.5f, 2f)] public float STOP_POWER;	
	[Range(.5f, 2f)] public float TURN_POWER ;
	[Range(.5f, 2f)] public float ACCEL_POWER;

	[Header("Drag")]
	public float AIR_DRAG;
	public float GROUND_DRAG;
}
