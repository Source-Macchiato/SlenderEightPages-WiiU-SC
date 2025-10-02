using System;
using UnityEngine;

[Serializable]
[AddComponentMenu("Character/FPS Input Controller")]
[RequireComponent(typeof(CharacterMotor))]
public class FPSInputController : MonoBehaviour
{
	private CharacterMotor motor;
	private PlayerController playerController;

	public virtual void Awake()
	{
		motor = GetComponent<CharacterMotor>();
		playerController = FindObjectOfType<PlayerController>();
	}

	public virtual void Update()
	{
		Vector3 vector = new Vector3(playerController.direction.x, 0f, playerController.direction.y);
		if (vector != Vector3.zero)
		{
			float magnitude = vector.magnitude;
			vector /= magnitude;
			magnitude = Mathf.Min(1f, magnitude);
			magnitude *= magnitude;
			vector *= magnitude;
		}
		motor.inputMoveDirection = transform.rotation * vector;
	}
}
