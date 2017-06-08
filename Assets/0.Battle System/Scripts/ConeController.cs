using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ConeController : MonoBehaviour {

	public Material coneMaterial;
	public PlayerAvatar player;
	public bool stop;
	[HideInInspector]
	public float radius;
	[HideInInspector]
	public float distance;
	[HideInInspector]
	private int verticles = 30;

	private float angle;
	private MeshFilter filter;

	void OnEnable() {
		if (player.profile == null) {
			this.enabled = false;
			return;
		}
		if (stop)
			return;
		Mesh m = new Mesh();
		List<Vector3> verts = new List<Vector3> ();
		List<int> tris = new List<int> ();
		float r = player.profile.radius == 0 ? 1 : player.profile.radius;
		radius = (1 - r)*2 + 0.1f;
		distance = player.profile.distance == 0 ? 30 : player.profile.distance;
		m.name = "Cone";
		angle = 360f / verticles;
		float x = radius * Mathf.Cos (0);
		float y = radius * Mathf.Sin (0);
		verts.Add (transform.position);
		verts.Add (new Vector3 (y, x, -distance));
		for (int i = 1; i < verticles + 1; i++) {
			float a = angle * i;
			x = radius * Mathf.Cos (a*Mathf.Deg2Rad);
			y = radius * Mathf.Sin (a*Mathf.Deg2Rad);
			verts.Add (new Vector3 (y, x, -distance));	
			tris.Add (0);
			tris.Add (i);
			tris.Add (i + 1);
		}

		m.vertices = verts.ToArray ();	
		m.triangles = tris.ToArray ();

		Vector2[] uvs = new Vector2[verts.Count];
		for (int i = 0; i < uvs.Length; i++)
			uvs[i] = new Vector2(verts[i].x, verts[i].z);
		m.uv = uvs;

		m.RecalculateNormals();
		GameObject obj = new GameObject("Cone", typeof(MeshRenderer), typeof(MeshFilter));
		filter = obj.GetComponent<MeshFilter> ();
		filter.mesh = m;
		obj.GetComponent<MeshRenderer>().material = coneMaterial;
	}

	void Update() {
		if (stop)
			return;
		Vector3[] vertices = filter.mesh.vertices;
		vertices [0] = transform.position;
		int i = 0;
		while (i < vertices.Length - 1) {
			float a = angle * i * Mathf.Deg2Rad;
			float x = radius * Mathf.Cos (a);
			float y = radius * Mathf.Sin (a);
			vertices [i + 1] = transform.rotation*(new Vector3 (y, x, distance)) + transform.position;
			i++;
		}
		filter.mesh.vertices = vertices;
	}

	void OnDisable(){
		if(filter) Destroy (filter.gameObject);
	}
}
