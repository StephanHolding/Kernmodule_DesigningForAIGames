using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{

	public class Boid
	{
		public Boid(GameObject objectReference)
		{
			this.ObjectReference = objectReference;
		}

		public GameObject ObjectReference { get; private set; }

		public void SimulateMovement(List<Boid> other, Vector3 steering, float speed, float steeringSpeed)
		{
			if (steering != Vector3.zero)
				ObjectReference.transform.rotation = Quaternion.RotateTowards(ObjectReference.transform.rotation, Quaternion.LookRotation(steering), steeringSpeed * Time.deltaTime);

			ObjectReference.transform.position += ObjectReference.transform.TransformDirection(new Vector3(0, 0, speed)) * Time.deltaTime;
		}
	}

	[Header("Settings")]
	public int boidAmount;
	public int spawnRange;
	public float seperationRange;
	public float localBoidRange;
	public float speed;
	public float steeringSpeed;

	[Header("Weights")]
	public int seperationWeight;
	public int allignmentWeight;
	public int cohesionWeight;

	[Header("Prefabs")]
	public GameObject prefab;

	private List<Boid> allBoids = new List<Boid>();

	private void Awake()
	{
		SpawnBoids();
	}

	private void SpawnBoids()
	{
		for (int i = 0; i < boidAmount; i++)
		{
			GameObject newObject = Instantiate(prefab, transform.position + Random.insideUnitSphere * spawnRange, Quaternion.identity);
			allBoids.Add(new Boid(newObject));
		}
	}

	private void Update()
	{
		foreach(Boid b in allBoids)
		{
			Vector3 steering = Vector3.zero;

			steering += Seperation(b).normalized * (seperationWeight / 100.0f);
			steering += Allignment(b).normalized * (allignmentWeight / 100.0f);
			steering += Cohesion(b).normalized * (cohesionWeight / 100.0f);


			b.SimulateMovement(allBoids, steering, speed, steeringSpeed);
		}
	}

	private Vector3 Seperation(Boid boid)
	{
		Vector3 seperationDirection = Vector3.zero;
		int count = 0;

		foreach (Boid b in allBoids)
		{
			if (b == boid) continue;

			float distance = Vector3.Distance(b.ObjectReference.transform.position, boid.ObjectReference.transform.position);

			if (distance < seperationRange)
			{
				seperationDirection += b.ObjectReference.transform.position - boid.ObjectReference.transform.position;
				count++;
			}
		}

		if (count > 0)
			seperationDirection /= count;

		seperationDirection = -seperationDirection.normalized;
		return seperationDirection;
	}

	private Vector3 Allignment(Boid boid)
	{
		Vector3 allignmentDirection = Vector3.zero;
		int count = 0;

		foreach (Boid b in allBoids)
		{
			if (b == boid) continue;

			float distance = Vector3.Distance(b.ObjectReference.transform.position, boid.ObjectReference.transform.position);

			if (distance < localBoidRange)
			{
				allignmentDirection += b.ObjectReference.transform.forward;
				count++;
			}
		}

		if (count > 0)
			allignmentDirection /= count;

		return allignmentDirection;	
	}

	private Vector3 Cohesion(Boid boid)
	{
		Vector3 cohesionDirection = Vector3.zero;
		int count = 0;

		foreach (Boid b in allBoids)
		{
			if (boid == b) continue;

			float distance = Vector3.Distance(b.ObjectReference.transform.position, boid.ObjectReference.transform.position);

			if (distance < localBoidRange)
			{
				cohesionDirection += b.ObjectReference.transform.position - boid.ObjectReference.transform.position;
				count++;
			}
		}

		cohesionDirection -= boid.ObjectReference.transform.position;
		return cohesionDirection;
	}
}
