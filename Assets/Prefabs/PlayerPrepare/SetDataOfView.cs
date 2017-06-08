using UnityEngine;
using System.Collections;

public class SetDataOfView : MonoBehaviour {

    public FourmulEndSystem _FourmulEndSystem;
    public FOV2DEyes _FOV2DEyes;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

        _FOV2DEyes.fovMaxDistance = _FourmulEndSystem.DistanceAttack;
        _FOV2DEyes.fovAngle = (int)_FourmulEndSystem.AngelAttack;

    }
}
