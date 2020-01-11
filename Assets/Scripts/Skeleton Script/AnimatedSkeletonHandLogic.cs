using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSkeletonHandLogic: MonoBehaviour
{
	public int snatchQuantity;


	private void OnCollisionEnter(Collision collision)
	{
		//Debug.Log("Collision Tag: " + other.transform.tag);

		if (collision.transform.tag == "Player1" || collision.transform.tag == "Player2")
		{

			collision.gameObject.GetComponent<PlayerLogic>().loseFruits(snatchQuantity, true);


		}
	}
}
