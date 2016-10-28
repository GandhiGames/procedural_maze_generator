using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HorrorHouse
{
	[RequireComponent (typeof(ObjectPool))]
	public class ObjectManager : MonoBehaviour
	{ 
	 
		protected List<GameObject> objects = new List<GameObject> ();

		private static ObjectManager _instance;
		
		public static ObjectManager instance { get { return _instance; } }

		private ObjectPool pool;

		void Awake ()
		{
			_instance = this;
			pool = GetComponent<ObjectPool> ();

		}

		public GameObject AddObject (string prefabName)
		{
			var obj = pool.GetObjectForType (prefabName, false);

			if (obj) {
				objects.Add (obj);
			}
			
			return obj;
		}


		public GameObject AddObject (string prefabName, Vector3 position, Quaternion rotation)
		{
			var obj = pool.GetObjectForType (prefabName, false);
			
			if (obj) {
				obj.transform.position = position;
				obj.transform.rotation = rotation;
				obj.SetActive (true);
				
				objects.Add (obj);
			} 
			
			return obj;
		}

		public GameObject AddObject (string prefabName, Vector3 position)
		{
			var obj = pool.GetObjectForType (prefabName, false);

			if (obj) {
				obj.transform.position = position;

				obj.SetActive (true);

				objects.Add (obj);
			} 

			return obj;
		}

		public GameObject AddObject (string prefabName, Vector3 position, bool onlyPooledObjects)
		{

			var obj = pool.GetObjectForType (prefabName, onlyPooledObjects);
			
			if (obj) {
				obj.transform.position = position;
				
				obj.SetActive (true);
				
				objects.Add (obj);
			} 
			
			return obj;
		}

		public void RemoveObject (GameObject obj)
		{
			pool.PoolObject (obj);
					
			objects.Remove (obj);

		}


		public void RemoveObjects ()
		{
			for (int i = 0; i < objects.Count; i++) {
				pool.PoolObject (objects [i]);
			}
			
			objects.Clear ();
		}

	}
}

