using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyeongJin
{
	public class CEndingCharacter : MonoBehaviour
	{
		Vector3 originPosition;
		[SerializeField] private CDinosaurChase cDinosaurChase;

		private void Start()
		{
			originPosition = this.transform.position;
			StartCoroutine(UpdateCharacter());
		}
		private IEnumerator UpdateCharacter()
		{
			Vector3 updatePosition = this.transform.position;

			while((originPosition.z - updatePosition.z) < 1.5f)
			{
				updatePosition.x += this.transform.forward.x * Time.deltaTime;
				updatePosition.z += this.transform.forward.z * Time.deltaTime;

				this.transform.position = updatePosition;
				yield return null;
			}

			this.GetComponent<Animator>().SetBool("Change", true);

			StartCoroutine(SetSpeed(2f));
		}
		private IEnumerator SetSpeed(float setTime)
		{
			while(setTime > 0)
			{
				setTime -= Time.deltaTime;
				cDinosaurChase.playeroffsetZ += Time.deltaTime / 5;
				cDinosaurChase.playeroffsetY -= Time.deltaTime / 5;
				yield return null;
			}
			this.GetComponent<Animator>().speed = 0;
		}
	}
}