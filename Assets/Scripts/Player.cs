using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public event System.Action OnReachedEndOfLevel;

	public UnityEngine.UI.Slider slowSlider;
	public AudioSource wienner;

	private bool activated = false;
	private float timeAmt = 3;
	private float time;

	public float moveSpeed = 7;
	public float smoothMoveTime = .1f;
	public float turnSpeed = 8;

	float angle;
	float smoothInputMagnitude;
	float smoothMoveVelocity;
	Vector3 velocity;

	new Rigidbody rigidbody;
	bool disabled;

	void Start() {
		time = timeAmt;

		rigidbody = GetComponent<Rigidbody> ();
		Patrol.OnPatrolHasSpottedPlayer += Disable;
		Guardian.OnGuardianHasSpottedPlayer += Disable;
		Sleepy.OnSleepyHasCollidedPlayer += Disable;
	}

	void Update () {
		Vector3 inputDirection = Vector3.zero;
		if (!disabled) {
			inputDirection = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized;
		}
		float inputMagnitude = inputDirection.magnitude;
		smoothInputMagnitude = Mathf.SmoothDamp (smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

		float targetAngle = Mathf.Atan2 (inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
		angle = Mathf.LerpAngle (angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);

		velocity = transform.forward * moveSpeed * smoothInputMagnitude;


		if (Input.GetKey (KeyCode.Space)) {
			activated = true;
			Time.timeScale = 0.65f;

			if (activated == true && time > 0) {
				//time -= Time.deltaTime;
				slowSlider.value -= Time.deltaTime;
			}
		} else {
	      Time.timeScale = 1f;
			activated = false;
			slowSlider.value += Time.deltaTime;
			/*time = 3;
			slowSlider.value = time;*/
		}
	}

	void OnTriggerEnter(Collider hitCollider) {
		if (hitCollider.tag == "Finish") {
			Disable ();
			wienner.Play ();
			if (OnReachedEndOfLevel != null) {
				OnReachedEndOfLevel ();
			}
		}
	}

	void Disable() {
		disabled = true;
	}

	void FixedUpdate() {
		rigidbody.MoveRotation (Quaternion.Euler (Vector3.up * angle));
		rigidbody.MovePosition (rigidbody.position + velocity * Time.deltaTime);
	}

	void OnDestroy() {
		Patrol.OnPatrolHasSpottedPlayer -= Disable;
		Guardian.OnGuardianHasSpottedPlayer -= Disable;
		Sleepy.OnSleepyHasCollidedPlayer -= Disable;
	}
}
