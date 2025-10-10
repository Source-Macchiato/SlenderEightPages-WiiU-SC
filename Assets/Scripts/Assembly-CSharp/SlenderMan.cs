using UnityEngine;

public class SlenderMan : MonoBehaviour
{
	[Header("Scripts")]
	[SerializeField] private SanityManager sanityManager;
	[SerializeField] private StaminaManager staminaManager;
	[SerializeField] private PauseManager pauseManager;
	[SerializeField] private FlashlightManager flashLightManager;
	[SerializeField] private SharedVar shared;

	[Header("Slender Man")]
	public Transform player;
	public GameObject SM;
	public PlayerScript view;
	public AudioSource dramatic;
	public int makejump;
	public int mightport;
	public bool justmoved;
	public Renderer model;
	public Vector3 chaser = Vector3.zero;
	public bool chasing;
	public Transform lhand;
	public Transform rhand;
	public Transform testobj;
	public TestScript tos;
	public int busymove;
	public float maxdeviation = 4f;
	public IntroScript startgame;
	public Transform playleft;
	public Transform playright;
	public LoseScript loser;

	// Constants //
	private const float BOUNDS_MAX = 149f;			// - Maximum boundary for Slender's movement
	private const float BOUNDS_MIN = -149f;			// - Minimum boundary for Slender's movement
	private const float SCARE_DISTANCE = 10f;		// - Distance within which a scare can be triggered
	private const float CATCH_DISTANCE = 2f;		// - Distance within which the player is caught
	private const float CHASE_TRIGGER_DIST = 30f;	// - Distance to trigger chase behavior
	private const float STAMINA_BOOST = 15f;		// - Stamina boost on first sighting
	private const int SCARE_DURATION = 600;			// - Duration of scare effect in frames
	
	// Private Variables cache //
	private Vector3 _cachedPosition;	// Cached position of Slender
	private Vector3 _slenderViewPoint;	// Viewpoint for sanity checks
	private float _distToPlayer;		// Distance to player
	private Transform _transform;		// Cached transform
	private int _totalProgress;         // Total game progress (pages + level)

	private void Awake()
	{
		_transform = transform; // Cache transform reference
	}

	private void FixedUpdate()
	{
		if (pauseManager.paused) return;

		// Cache position once
		_cachedPosition = _transform.position;
		_totalProgress = shared.pages + shared.level;

		if (justmoved)
		{
			justmoved = false;
			if (sanityManager.cansee)
				sanityManager.flicker = 3;
		}

		if (_totalProgress <= 0 || loser.timeleft != 0)
		{
			HandleIdleState();
			return;
		}

		_slenderViewPoint = _cachedPosition + Vector3.up * 0.99f;
		_distToPlayer = Vector3.Distance(_slenderViewPoint, player.position);

		// Check catch condition
		if (CheckCatchCondition()) return;

		// Check chase condition
		UpdateChaseState();

		// Handle final delay
		if (shared.finaldelay > 0)
		{
			if (--shared.finaldelay <= 0)
				busymove = 4;
			return;
		}

		// Main AI logic
		if (!sanityManager.cansee || _totalProgress >= 6)
		{
			if (!shared.caught)
				ProcessAIBehavior();
		}
		else
		{
			HandleIdleState();
		}

		// Handle model visibility for page 8
		if (shared.pages >= 8 && ((!shared.caught && model.enabled) || loser.timeleft > 1))
		{
			if (sanityManager.cansee)
				sanityManager.flicker = 3;
			model.enabled = false;
			_transform.position = new Vector3(0f, -200f, 0f);
		}
	}

	// Checks if the player can see Slender and updates sanity accordingly //
	public void CheckSanity()
	{
		_slenderViewPoint = _cachedPosition + Vector3.up * 1.33f;
		_distToPlayer = Vector3.Distance(_slenderViewPoint, player.position);

		// Calculate drain once
		float drainMultiplier = shared.daytime ? 1.5f : (flashLightManager.torch.enabled ? 1f : 2f);
		float drain = Mathf.Pow(2f, -_distToPlayer * drainMultiplier / 10f);

		sanityManager.cansee = false;

		// Check visibility from player to slender parts
		Vector3[] checkPoints = { _slenderViewPoint, lhand.position, rhand.position };
		RaycastHit hit;

		for (int i = 0; i < checkPoints.Length; i++)
		{
			if (Physics.Raycast(player.position, (checkPoints[i] - player.position).normalized, out hit))
			{
				if (hit.collider.gameObject == gameObject)
				{
					sanityManager.cansee = true;
					sanityManager.drain = drain;

					// First sighting logic
					if (!sanityManager.justsaw && _distToPlayer < SCARE_DISTANCE && shared.scared <= 0)
					{
						TriggerScare();
					}
					break; // No need to check other points
				}
			}
		}
	}

	// Triggers a scare event when the player first sees Slender
	private void TriggerScare()
	{
		if (!shared.mh)
		{
			dramatic.Play();
		}
		else
		{
			sanityManager.flicker = 3; // Flicker effect in MH mode
		}
		
		sanityManager.justsaw = true; 	// Mark that player just saw Slender
		shared.scared = SCARE_DURATION; // Set scare duration
		staminaManager.stamina = Mathf.Min(staminaManager.stamina + STAMINA_BOOST, staminaManager.maxstam); // Boost stamina
	}

	// Checks if the player is close enough to be caught by Slender
	private bool CheckCatchCondition()
	{
		RaycastHit hit;
		// Player within catch distance and has line of sight to Slender
		if (_distToPlayer <= CATCH_DISTANCE &&
			Physics.Raycast(player.position, (_slenderViewPoint - player.position).normalized, out hit) &&
			hit.collider.gameObject == gameObject)
		{
			// Reveal model if on final page and hidden
			if (shared.pages >= 8 && !model.enabled)
			{
				model.enabled = true;
				sanityManager.flicker = 3;
			}
			shared.caught = true; // Mark player as caught
			return true;
		}
		return false;
	}

	// Updates chase state based on player proximity and visibility
	private void UpdateChaseState()
	{
		// If player is within chase trigger distance
		if (_distToPlayer < CHASE_TRIGGER_DIST)
		{
			RaycastHit hit;
			Vector3 dir1 = (_slenderViewPoint - player.position).normalized;
			Vector3 dir2 = (lhand.position - player.position).normalized;
			Vector3 dir3 = (rhand.position - player.position).normalized;

			// Require visibility from player to all three key points
			if (Physics.Raycast(player.position, dir1, out hit) && hit.collider.gameObject == gameObject &&
				Physics.Raycast(playleft.position, dir2, out hit) && hit.collider.gameObject == gameObject &&
				Physics.Raycast(playright.position, dir3, out hit) && hit.collider.gameObject == gameObject)
			{
				chasing = true;
				chaser = player.position;
			}
		}
		else
		{
			chasing = false;
		}
	}

	// Main AI behavior processing //
	private void ProcessAIBehavior()
	{
		// Teleport logic
		if (model.isVisible && _totalProgress < 6)
		{
			mightport++;
			if ((mightport > 100 && Random.value <= 0.001f) || mightport >= 1100)
			{
				mightport = 0;
				if (Random.value <= 0.5f)
					busymove = 4;
			}
		}
		else
		{
			mightport = 0;
			makejump++;
			int jumpThreshold = 550 - _totalProgress * 50;

			if (makejump >= jumpThreshold && (!chasing || (_distToPlayer > 10f && Random.value <= 0.2f)))
			{
				makejump = 0;
				if (shared.pages >= 8)
					busymove = 3;
				else if (_distToPlayer > shared.maxrange || Random.value <= 0.1f)
					busymove = 4;
				else
					busymove = 3;
			}
		}

		// Execute movement
		ExecuteMovement();

		// Handle rotation
		UpdateRotation();
	}

	// Executes the current movement action based on busymove state //
	private void ExecuteMovement()
	{
		switch (busymove)
		{
			case 1:
				ExecuteTeleportFinish();
				break;
			case 2:
				ExecuteFarTeleportFinish();
				break;
			case 3:
				PrepareNearTeleport();
				break;
			case 4:
				PrepareFarTeleport();
				break;
			default:
				maxdeviation = 4f;
				break;
		}
	}

	// Finalizes a near teleport attempt //
	private void ExecuteTeleportFinish()
	{
		if (tos.valid && (tos.hidden || _totalProgress >= 6 || !shared.flraised))
		{
			Vector3 newPos = testobj.position;
			newPos.y = 1f;
			_transform.position = newPos;
			justmoved = true;
			busymove = 0;
			chasing = false;
		}
		else
		{
			busymove = 3;
			maxdeviation += 0.25f;
		}
		ResetTestObject();
	}

	// Finalizes a far teleport attempt //
	private void ExecuteFarTeleportFinish()
	{
		if (tos.valid)
		{
			bool canTeleport = (_totalProgress <= 5 && (tos.hidden || !shared.flraised)) ||
							   _totalProgress == 6 || shared.pages >= 8 ||
							   (!tos.hidden && _totalProgress == 7);

			if (canTeleport)
			{
				Vector3 newPos = testobj.position;
				newPos.y = 1f;
				_transform.position = newPos;
				justmoved = true;
				busymove = 0;
				chasing = false;
			}
			else
			{
				busymove = 4;
			}
		}
		else
		{
			busymove = 4;
		}
		ResetTestObject();
	}

	// Prepares a near teleport attempt //
	private void PrepareNearTeleport()
	{
		Vector3 targetPos;
		bool isNightUnlit = shared.pages >= 8 || (!flashLightManager.torch.enabled && !shared.daytime);
		float minDist = isNightUnlit ? 2f : 8f;
		
		for (int i = 0; i < 30; i++)
		{
			Vector2 randomDir = Random.insideUnitCircle;
			targetPos = _slenderViewPoint + new Vector3(randomDir.x * 30f, 0f, randomDir.y * 30f);
			
			if (!IsInBounds(targetPos)) continue;
			
			float distFromPlayer = Vector3.Distance(player.position, targetPos);
			float distFromCurrent = Vector3.Distance(_slenderViewPoint, targetPos);
			
			if (distFromPlayer > minDist && ValidateTeleportPosition(distFromCurrent, distFromPlayer))
			{
				SetupTestObject(targetPos);
				busymove = 1;
				return;
			}
			maxdeviation += 0.25f;
		}
		maxdeviation += 0.25f;
	}

	// Validates if the teleport position meets distance criteria //
	private bool ValidateTeleportPosition(float distFromCurrent, float distFromPlayer)
	{
		if (_distToPlayer > 30f)
			return distFromCurrent >= 20f && distFromCurrent + distFromPlayer - maxdeviation <= _distToPlayer;
		else
			return distFromCurrent >= _distToPlayer - 10f && distFromCurrent <= _distToPlayer + 10f &&
				   distFromCurrent + distFromPlayer - maxdeviation <= _distToPlayer;
	}

	// Prepares a far teleport attempt //
	private void PrepareFarTeleport()
	{
		for (int i = 0; i < 30; i++)
		{
			Vector2 randomDir = Random.insideUnitCircle.normalized;
			Vector3 targetPos = player.position + new Vector3(randomDir.x * shared.maxrange, 2.3f, randomDir.y * shared.maxrange);

			if (IsInBounds(targetPos) && Vector3.Distance(player.position, targetPos) > shared.minrange)
			{
				SetupTestObject(targetPos);
				busymove = 2;
				return;
			}
		}
	}

	// Updates Slender's rotation based on current state //
	private void UpdateRotation()
	{
		if (justmoved)
		{
			if (sanityManager.cansee)
				sanityManager.flicker = 3;
			LookAtPlayer();
		}
		else if (chasing && !model.isVisible && !shared.caught)
		{
			// Chase movement
			Quaternion rot = Quaternion.LookRotation(_slenderViewPoint - chaser, Vector3.up);
			rot.x = rot.z = 0f;
			_transform.rotation = rot;
			_transform.Translate(_transform.forward * ((_totalProgress * -0.5f + 0.5f) * Time.deltaTime), Space.World);

			if (Vector3.Distance(_slenderViewPoint, chaser) <= 0.75f)
				chasing = false;
		}
		else if (!sanityManager.cansee)
		{
			LookAtPlayer();
		}
	}

	// Handles idle state when player cannot see Slender //
	private void HandleIdleState()
	{
		mightport = 0;
		busymove = 0;
		if (!sanityManager.cansee)
			LookAtPlayer();
	}

	// Rotates Slender to face the player //
	private void LookAtPlayer()
	{
		Quaternion rot = Quaternion.LookRotation(_cachedPosition - player.position, Vector3.up);
		rot.x = rot.z = 0f;
		_transform.rotation = rot;
	}

	// Checks if a position is within the defined boundaries //
	private bool IsInBounds(Vector3 pos)
	{
		return pos.x < BOUNDS_MAX && pos.x > BOUNDS_MIN &&
			   pos.z < BOUNDS_MAX && pos.z > BOUNDS_MIN;
	}

	// Sets up the test object for teleport validation //
	private void SetupTestObject(Vector3 position)
	{
		testobj.position = position;
		Quaternion rot = Quaternion.LookRotation(position - player.position, Vector3.up);
		rot.x = rot.z = 0f;
		testobj.rotation = rot;
		tos.testing = true;
		tos.valid = true;
		tos.hidden = true;
	}

	// Resets the test object after teleport attempt //
	private void ResetTestObject()
	{
		testobj.position = new Vector3(0f, -50f, 0f);
		tos.testing = false;
		tos.valid = true;
		tos.hidden = true;
	}
}