using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

	Queue<int> switches = new Queue<int>();

	public GameObject Door1, Door2, Door3, Door4;
	public Material switchON;
	public AudioSource Open;

	private string SwitchName;

	// Use this for initialization
	void Start () {

		SwitchName = this.name;

	}
	
	// Update is called once per frame
	void Update () {
		if (Door1.activeSelf == false && Door2.activeSelf == false && Door3.activeSelf == false){
					Door4.SetActive (false);
				}
			}

	void OnTriggerEnter(Collider collision)
	{
		if (collision.tag == "Player") {
			if (SwitchName == "Switch1") {
				Door1.SetActive (false);
				GetComponent<Renderer> ().material = switchON;
			    switches.Enqueue (1);
				Open.Play ();
			}
			if (SwitchName == "Switch2") {
				Door2.SetActive (false);
				GetComponent<Renderer> ().material = switchON;
				switches.Enqueue (2);
				Open.Play ();
			}
			if (SwitchName == "Switch3") {
				Door3.SetActive (false);
				GetComponent<Renderer> ().material = switchON;
				switches.Enqueue (3);
				Open.Play ();
			}
			}
		}
	}
		
