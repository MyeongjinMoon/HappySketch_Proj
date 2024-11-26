using UnityEngine;
using System.Collections;

public class CrocodileUserController : MonoBehaviour {
	CrocodileCharacter crocodileCharacter;
	
	void Start () {
		crocodileCharacter = GetComponent < CrocodileCharacter> ();
	}
	
	void Update () {	
		if (Input.GetButtonDown ("Fire1")) {
			crocodileCharacter.Attack();
		}

		if (Input.GetKeyDown (KeyCode.H)) {
			crocodileCharacter.Hit();
		}		
		
		if (Input.GetKeyDown (KeyCode.K)) {
			crocodileCharacter.Death();
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			crocodileCharacter.Rebirth();
		}			

		if (Input.GetKeyDown (KeyCode.I)) {
			crocodileCharacter.SwimStart();
		}	
		if (Input.GetKeyDown (KeyCode.M)) {
			crocodileCharacter.SwimEnd();
		}			

		if (crocodileCharacter.isSwimming) {
			if (Input.GetKey (KeyCode.U)) {
				crocodileCharacter.forwardAccerelation = Mathf.Lerp (crocodileCharacter.forwardAccerelation, 2f, Time.deltaTime);
			} else if (Input.GetKey (KeyCode.N)) {
				crocodileCharacter.forwardAccerelation = Mathf.Lerp (crocodileCharacter.forwardAccerelation, -1f, Time.deltaTime);
			} else {
				crocodileCharacter.forwardAccerelation = Mathf.Lerp (crocodileCharacter.forwardAccerelation, 1f, Time.deltaTime);
			}
		
			if (Input.GetKey (KeyCode.Z)) {
				crocodileCharacter.rollAccerelation = Mathf.Lerp (crocodileCharacter.rollAccerelation, 1f, Time.deltaTime);
			} else if (Input.GetKey (KeyCode.X)) {
				crocodileCharacter.rollAccerelation = Mathf.Lerp (crocodileCharacter.rollAccerelation, -1f, Time.deltaTime);
			} else {
				crocodileCharacter.rollAccerelation = Mathf.Lerp (crocodileCharacter.rollAccerelation, 0f, Time.deltaTime);
			}
		
			crocodileCharacter.upDownAccerelation = Input.GetAxis ("Vertical") ;
			crocodileCharacter.turnAccerelation = Input.GetAxis ("Horizontal");
		} else {
			if (Input.GetKeyDown (KeyCode.U)) {
				crocodileCharacter.WakeUp();
			}	
			if (Input.GetKeyDown (KeyCode.N)) {
				crocodileCharacter.SitDown();
			}	
			crocodileCharacter.forwardSpeed = Input.GetAxis ("Vertical");
			crocodileCharacter.turnSpeed = Input.GetAxis ("Horizontal");
		}
	}
}
