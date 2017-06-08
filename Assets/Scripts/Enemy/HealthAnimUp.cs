using UnityEngine;
using System.Collections;

public class HealthAnimUp : MonoBehaviour {

    //float X_GO;
    float Y_GO;
    //float Z_GO;


    public Color Alpah;
    public TextMesh _Helth;
	// Use this for initialization
	void Start () {

        //X_GO = Random.Range(0.05f,0.3f);
        Y_GO = 0.2f;
        //Z_GO = Random.Range(0.05f, 0.3f);

        transform.LookAt(Camera.main.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        Alpah = _Helth.GetComponent<TextMesh>().color;
    }
	
	// Update is called once per frame
	void Update () {

        transform.Translate(0, Y_GO, 0);
        Alpah = new Color(Alpah.r, Alpah.g, Alpah.b, Alpah.a - 0.018f);
        _Helth.GetComponent<TextMesh>().color = Alpah;

    }
}
