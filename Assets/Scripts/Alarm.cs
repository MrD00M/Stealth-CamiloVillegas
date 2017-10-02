using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm: MonoBehaviour {

	public static event System.Action OnAlarmHasSpottedPlayer;

	public float timeToSpotPlayer = .5f;

	public Light spotlight;
	public float viewDistance;
	public LayerMask viewMask;

	float viewAngle;
	float playerVisibleTimer;

	public Transform target;  
	private Vector3 initialPosition;
	private Quaternion initialRotation;
	public AudioSource alarm;

	Transform player;
	Color originalSpotlightColour;

	void Start() {

		player = GameObject.FindGameObjectWithTag ("Player").transform;
		viewAngle = spotlight.spotAngle;
		originalSpotlightColour = spotlight.color;
	}

	void Update ()
	{
		if (CanSeePlayer ()) {
			playerVisibleTimer += Time.deltaTime;
			alarm.Play ();
		} else {
			playerVisibleTimer -= Time.deltaTime;
			alarm.Stop ();
		}

		playerVisibleTimer = Mathf.Clamp (playerVisibleTimer, 0, timeToSpotPlayer);
		spotlight.color = Color.Lerp (originalSpotlightColour, Color.red, playerVisibleTimer / timeToSpotPlayer);

		if (playerVisibleTimer >= timeToSpotPlayer) {
			if (OnAlarmHasSpottedPlayer != null) {
				OnAlarmHasSpottedPlayer ();
			}
		}
	}


	bool CanSeePlayer() {
		if (Vector3.Distance(transform.position,player.position) < viewDistance) {
			Vector3 dirToPlayer = (player.position - transform.position).normalized;
			float angleBetweenGuardAndPlayer = Vector3.Angle (transform.forward, dirToPlayer);
			if (angleBetweenGuardAndPlayer < viewAngle / 2f) {
				if (!Physics.Linecast (transform.position, player.position, viewMask)) {
					return true;
				}
			}
		}
		return false;
	}

void OnDrawGizmos() {
	Gizmos.color = Color.red;
	Gizmos.DrawRay (transform.position, transform.forward * viewDistance);
}

}