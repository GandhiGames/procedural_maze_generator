using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour
{
	private Transform player;
	private bool initialised = false;
	
	public void Initialise ()
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		Camera.main.rect = new Rect (0.7f, 0.7f, 0.29f, 0.29f);

		Camera.main.clearFlags = CameraClearFlags.Depth;
		initialised = true;
	}
	
	void Update ()
	{
		if (!initialised)
			return;
		
		var pos = player.position;
		transform.position = new Vector3 (pos.x, transform.position.y, pos.z);
	}
}
