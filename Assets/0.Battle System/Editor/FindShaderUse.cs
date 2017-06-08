using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FindShaderUse : EditorWindow {
	string st = "";
	string stArea = "Empty List";

	[MenuItem("Window/Find Shader Use")]
	public static void ShowWindow() {
		EditorWindow.GetWindow(typeof(FindShaderUse));
	}

	public void OnGUI() {
		GUILayout.Label("Enter shader to find:");
		st = GUILayout.TextField (st);
		if (GUILayout.Button("Find Materials")) {
			FindShader(st);
		}
		GUILayout.Label(stArea);
	}

	private void FindShader(string shaderName) {
		int count = 0;
		stArea = "Materials using shader " + shaderName+":\n\n";

		List<Material> armat = new List<Material>();
		List<Renderer> rend = new List<Renderer>();

		Renderer[] arrend = (Renderer[])Resources.FindObjectsOfTypeAll(typeof(Renderer));
		foreach (Renderer r in arrend) {
			foreach (Material mat in r.sharedMaterials) {
				if (!armat.Contains (mat)) {
					armat.Add (mat);
//					stArea += ">" + mat.name + "\n";
//					stArea += ">" + r.name + "\n"; 
				}
			}
		}

		foreach (Material mat in armat) {
			if (mat != null && mat.shader != null && mat.shader.name != null && mat.shader.name == shaderName) {
				stArea += ">"+mat.name + "\n";
				count++;
			}
		}

		stArea += "\n"+count + " materials using shader " + shaderName + " found.";
	}
}