//#define TEST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AngleDetector : MonoBehaviour {

	public Material coneMaterial;
	public PlayerAvatar player;
	public bool farAngle;
	public int layer = 8;
	public bool hideColliders;
	public Projector vZone;

	private float radius;
	private float height = 2;
	private MeshRenderer meshRenderer;
	private int verticles = 6;
	private float angle;
	private MeshFilter filterArc;
	private Mesh colliderMesh;
//	private MeshCollider m_collider;
	private float realAngle;
	private IDetectObjects detector;

	void Start(){
		#if TEST
		radius = 6;
		realAngle = 30;
		#else
		if (player.battleNear == null) {
			this.enabled = false;
			return;
		}
		if (!farAngle)
			radius = player.battleNear.distanceNear;
		else
			radius = player.battleGun.distance;
//		print("RADIUS: " + radius);
		if(vZone) vZone.orthographicSize = radius;
		realAngle = player.battleNear.angle;
		#endif
//		print (radius + "   |  " + realAngle);
		DrawMesh ();
	}

	void DrawMesh(){
		angle = realAngle/verticles;

		//Рисуем 2Д проекцию угла
		Mesh arc = new Mesh ();
		List<Vector3> verts = new List<Vector3> ();
		List<int> tris = new List<int> ();

		arc.name = "Angle";
		float x = radius * Mathf.Cos (0);
		float y = radius * Mathf.Sin (0);
		verts.Add (Vector3.zero); // центр круга
		verts.Add (new Vector3 (y, 0, x));
		for (int i = 1; i < verticles + 1; i++) {
			float a = angle * i;
			x = radius * Mathf.Cos (a*Mathf.Deg2Rad);
			y = radius * Mathf.Sin (a*Mathf.Deg2Rad);
			verts.Add (new Vector3 (y, 0, x));
			tris.Add (0);
			tris.Add (i);
			tris.Add (i + 1);
		}

		arc.vertices = verts.ToArray ();	
		arc.triangles = tris.ToArray ();

		Vector2[] uvs = new Vector2[verts.Count];
		for (int i = 0; i < uvs.Length; i++)
			uvs[i] = new Vector2(verts[i].x, verts[i].z);
		arc.uv = uvs;

		arc.RecalculateNormals();
		GameObject arcObj = new GameObject(
			"Angle",
			typeof(MeshRenderer),
			typeof(MeshFilter)
//			typeof(MeshCollider)
//			typeof(Rigidbody)
//			typeof(DetectObjects)
		);

		filterArc = arcObj.GetComponent<MeshFilter> ();
		filterArc.mesh = arc;
		arcObj.transform.SetParent (this.transform);
		arcObj.transform.localPosition = Vector3.zero;
		arcObj.transform.localEulerAngles = new Vector3(-90, 0, 0);
		arcObj.layer = layer;

		meshRenderer = arcObj.GetComponent<MeshRenderer> ();
		meshRenderer.material = coneMaterial;
		meshRenderer.enabled = !hideColliders;

//		arcObj.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
//		arcObj.GetComponent<DetectObjects> ().Init (transform.parent.gameObject);

		if(farAngle)
			detector = arcObj.AddComponent<DetectObjectsFar>();
		else
			detector = arcObj.AddComponent<DetectObjects>();

		//Генерируем коллайдер
		colliderMesh = new Mesh ();
		verts = new List<Vector3> ();
		tris = new List<int> ();
		x = radius * Mathf.Cos (0);
		y = radius * Mathf.Sin (0);
		verts.Add (Vector3.zero); // центр круга низ
		verts.Add (new Vector3 (0,  height, 0)); // центр круга верх
		verts.Add (new Vector3 (y, 0, x)); //1я точка низ
		verts.Add (new Vector3 (y, height, x)); //1я точка верх

		// левая стенка
		tris.Add (3);
		tris.Add (1);
		tris.Add (0);
		tris.Add (2);
		tris.Add (3);
		tris.Add (0);

		for (int i = 1; i < verticles + 1; i++) {
			float a = angle * i * Mathf.Deg2Rad;
			x = radius * Mathf.Cos (a);
			y = radius * Mathf.Sin (a);
			verts.Add (new Vector3 (y, 0, x));
			verts.Add (new Vector3 (y, height, x));
			tris.Add (i * 2 + 2);
			tris.Add (i * 2);
			tris.Add (0);
			tris.Add (i * 2 + 1);
			tris.Add (i * 2 + 3);
			tris.Add (1);

			tris.Add (i*2 + 3);
			tris.Add (i*2 + 1);
			tris.Add (i*2);
			tris.Add (i*2 + 2);
			tris.Add (i*2 + 3);
			tris.Add (i*2);
		}

		// парвая стенка
		tris.Add (0);
		tris.Add (1);
		tris.Add (verticles*2 + 2);
		tris.Add (verticles*2 + 2);
		tris.Add (1);
		tris.Add (verticles*2 + 3);

		colliderMesh.vertices = verts.ToArray ();	
		colliderMesh.triangles = tris.ToArray ();

		uvs = new Vector2[verts.Count];
		for (int j = 0; j < uvs.Length; j++)
			uvs[j] = new Vector2(verts[j].x, verts[j].z);
		colliderMesh.uv = uvs;

		colliderMesh.RecalculateNormals();

//		m_collider = arcObj.GetComponent<MeshCollider> ();
//		m_collider.sharedMesh = colliderMesh;
//		m_collider.convex = true;
//		m_collider.sharedMesh.MarkDynamic ();
//		m_collider.isTrigger = true;
//		m_collider.inflateMesh = true;
		UpdateMesh ();
		RebuidCollider ();
	}

	void UpdateMesh() {
		//Арка
		Vector3[] vertices = filterArc.mesh.vertices;
		Quaternion rotation = Quaternion.Euler(new Vector3(0, -realAngle/2f, 0));
		int i = 0;
		while (i < vertices.Length - 1) {
			float a = angle * i * Mathf.Deg2Rad;
			float x = radius * Mathf.Cos (a);
			float y = radius * Mathf.Sin (a);
			vertices [i + 1] = rotation * (new Vector3 (y, 0, x));
			i++;
		}
		filterArc.mesh.vertices = vertices;
	}

	public void SetRadius(float r){
		radius = r;
		if (filterArc == null) return;
		UpdateMesh ();
		RebuidCollider ();
	}

	public void SetCameraHeight(float h){
//		Assets.Scripts.Terrains.TPCamera.I.SetZoom (h*1.4f - 5);
//		Assets.Scripts.Terrains.TPCamera.I.SetZoom (h);
//		if(GameLogic.instance) GameLogic.instance.playerDOF = h;
//		vZone.orthographicSize = h;
	}

	public void SetAngle(float a){
		realAngle = a;
		angle = realAngle/verticles;
		if (filterArc == null) return;
		UpdateMesh ();
		RebuidCollider ();
	}

	public void MouseUp(){
		if (filterArc == null) return;
		detector.Reset ();
		RebuidCollider ();
	}

	void RebuidCollider(){
		Vector3[] vertices = colliderMesh.vertices;
		Quaternion rotation = Quaternion.Euler(new Vector3(0, - realAngle/2f, 0));
		int i = 0;
		while (i < vertices.Length/2 - 1) {
			float a = angle * (i) * Mathf.Deg2Rad;
			float x = radius * Mathf.Cos (a);
			float y = radius * Mathf.Sin (a);
			vertices [(i + 1)*2] = rotation*(new Vector3 (y, 0, x));
			vertices [(i + 1)*2 + 1] = rotation*(new Vector3 (y, height, x));
			i++;
		}
		colliderMesh.vertices = vertices;
//		m_collider.sharedMesh = colliderMesh;
	}

	public void Rotate(float angle){
		transform.localEulerAngles = new Vector3 (0, angle, 0);
	}
}
