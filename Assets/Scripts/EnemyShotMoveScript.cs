using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotMoveScript : MonoBehaviour
{
	public float shotSpeed=0.05f;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		this.transform.position += new Vector3(0, -1, 0) * shotSpeed;
        if (transform.position.y <= -5) Destroy(gameObject);
	}
}
