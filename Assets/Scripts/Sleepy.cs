using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleepy : MonoBehaviour {

	public static event System.Action OnSleepyHasCollidedPlayer;

	private UnityEngine.AI.NavMeshAgent agent;

	public Transform target;  
	private Vector3 initialPosition;
	private Quaternion initialRotation;


	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
		initialRotation = transform.rotation;

		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();

		Alarm.OnAlarmHasSpottedPlayer += Follow;
	}
	
	// Update is called once per frame
	void Update () {
		if (initialPosition != transform.position || initialRotation != transform.rotation) {
			agent.SetDestination (initialPosition);
			transform.rotation = initialRotation;
		}
	}

	void Follow() {
		agent.SetDestination (target.position);
	}

	void OnDestroy() {
		Alarm.OnAlarmHasSpottedPlayer -= Follow;
	}
		
	void OnTriggerEnter(Collider collision)
	{
		if (collision.tag == "Player") {
			if (OnSleepyHasCollidedPlayer != null) {
				OnSleepyHasCollidedPlayer ();
			}
		}
	}

}
