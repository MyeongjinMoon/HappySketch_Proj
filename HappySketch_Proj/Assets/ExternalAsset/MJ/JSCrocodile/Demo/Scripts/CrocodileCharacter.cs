using UnityEngine;
using System.Collections;

public class CrocodileCharacter : MonoBehaviour {
	Animator crocodileAnimator;
	Rigidbody crocodileRigid;
	public float forwardSpeed;
	public float turnSpeed;
	public bool isSwimming;
	public bool isLived=true;
	public float forwardAccerelation=0f;
	public float turnAccerelation=0f;
	public float upDownAccerelation=0f;
	public float rollAccerelation = 0f;


	void Start () {
		crocodileAnimator = GetComponent<Animator> ();
		crocodileRigid=GetComponent<Rigidbody>();
	}
	
	void FixedUpdate(){
		Move ();
	}
	
	public void Attack(){
		crocodileAnimator.SetTrigger("Attack");
	}
	
	public void Hit(){
		crocodileAnimator.SetTrigger("Hit");
	}
	
	public void Death(){
		crocodileAnimator.SetBool("IsLived",false);
		isLived = false;
	}
	
	public void Rebirth(){
		crocodileAnimator.SetBool("IsLived",true);
		isLived = true;
	}
	
	public void SitDown(){
		crocodileAnimator.SetBool("SitDown",true);
	}
	
	public void WakeUp(){
		crocodileAnimator.SetBool("SitDown",false);
	}

	public void SwimStart(){
		crocodileAnimator.SetBool("IsSwimming",true);
		isSwimming = true;
		crocodileRigid.useGravity = false;
		crocodileAnimator.applyRootMotion = false;
	 	crocodileRigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		crocodileRigid.constraints = RigidbodyConstraints.None;
		crocodileRigid.angularDrag = 10f;
	}	
	
	public void SwimEnd(){
		crocodileAnimator.SetBool("IsSwimming",false);
		isSwimming = false;
		crocodileRigid.useGravity = true;
		crocodileRigid.angularDrag = 0.1f;
		transform.rotation = Quaternion.identity;
		crocodileAnimator.applyRootMotion = true;
		crocodileRigid.constraints = RigidbodyConstraints.FreezeRotation;
	}	
	
	public void Move(){
		if (isLived && !isSwimming) {
			crocodileAnimator.SetFloat ("Forward", forwardSpeed);
			crocodileAnimator.SetFloat ("Turn", turnSpeed);
		}
	
		if (isLived && isSwimming) {
			crocodileRigid.AddForce(transform.forward*forwardAccerelation*20000f);
			crocodileRigid.AddTorque(-transform.right*upDownAccerelation *80000f);
			crocodileRigid.AddTorque(transform.up*turnAccerelation*80000f);
			crocodileRigid.AddTorque(transform.forward*rollAccerelation*3000f);
			crocodileAnimator.SetFloat ("Forward", forwardAccerelation);
			crocodileAnimator.SetFloat ("Turn", turnAccerelation);
			crocodileAnimator.SetFloat ("UpDown", upDownAccerelation);
			crocodileAnimator.SetFloat ("Roll", rollAccerelation);
			crocodileAnimator.speed=Mathf.Abs(forwardAccerelation);
		}
	}
}
