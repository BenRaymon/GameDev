public class PlayerData
{
    public float GROUND_BUFFER;
	public float JUMP_FORCE;
	public float JUMP_QUEUE;
	public float COYOTE_TIME;
	public float GRAVITY;
	public float FALL_GRAVITY_MULT;
	public float MOVE_SPEED;
	public float RUN_ACCEL;
	public float RUN_DECELL;
	public float STOP_POWER;	
	public float TURN_POWER ;
	public float ACCEL_POWER;
	public float AIR_DRAG;
	public float GROUND_DRAG;

	public PlayerData(){
		//DEFAULTS
		GROUND_BUFFER = .02f;
		JUMP_FORCE = 20f;
		JUMP_QUEUE = 0.5f;
		COYOTE_TIME = 0.5f;
		GRAVITY = 2f;
		FALL_GRAVITY_MULT = 2f;
		MOVE_SPEED = 20f;
		RUN_ACCEL = 2f;
		STOP_POWER = 1.3f;
		TURN_POWER = 1f;
		ACCEL_POWER = 1.2f;
		AIR_DRAG = 0.4f;
		GROUND_DRAG = 0.4f;
	}

}
