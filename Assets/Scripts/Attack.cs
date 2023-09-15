using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public Transform target;
	public float h = 1;
	public float gravity = -3;
	public float attack;
	public GameManager gameManager;

	void Start()
	{
		gameObject.GetComponent<Rigidbody>().useGravity = false;
		target = gameManager.enemies[0].transform;
		Launch();
	}


	void Update()
	{
		if (target == null) { target = gameManager.character.transform; }
	}

	private void OnCollisionEnter(Collision collision)
	{
		print("attack");
		if (collision.gameObject.tag == "Enemy")
		{
			collision.gameObject.GetComponent<Enemy>().Damage(attack);
			Destroy(gameObject);
		}
		else { Destroy(gameObject); }
	}

	

	public void Launch()
	{
		Physics.gravity = Vector3.up * gravity;
		gameObject.GetComponent<Rigidbody>().useGravity = true;
		gameObject.GetComponent<Rigidbody>().velocity = CalculateLaunchData();
	}

	Vector3 CalculateLaunchData()
	{
		float displacementY = target.position.y - gameObject.GetComponent<Rigidbody>().position.y;
		Vector3 displacementXZ = new Vector3(target.position.x - gameObject.GetComponent<Rigidbody>().position.x, 0, target.position.z - gameObject.GetComponent<Rigidbody>().position.z);
		float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
		Vector3 velocityXZ = displacementXZ / time;
		return velocityXZ + velocityY;
	}
}