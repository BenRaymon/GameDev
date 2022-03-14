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

	/*

	NOTE: Git won't detect any modifications to the PlayerData.asset created in Scripts/Player Scripts/Data
		CAUSE: Unknown

	NOTE: Planning to add different accel/deccel while in the air later

	DEFAULT VALUES FOR LEVEL01MERGE (They are supposed to be constants, but are currently set as public
	so that they can be tweaked to improve the movement)
	GROUND_BUFFER = .02
	JUMP_FORCE = 20
	JUMP_QUEUE = 0.5
	COYOTE_TIME = 0.5

	GRAVITY = 2
	FALL_GRAVITY_MULT = 2

	MOVE_SPEED = 20
	RUN_ACCEL = 2
	RUN_DECCEL = 3

	STOP_POWER = 1.3
	TURN_POWER = 1
	ACCEL_POWER = 1.2

	AIR_DRAG = 0.4
	GROUND_DRAG = 0.4
	*/

}
