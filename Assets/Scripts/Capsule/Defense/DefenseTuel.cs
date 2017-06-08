using UnityEngine;
using System.Collections;
using Capsule;

public class DefenseTuel : LandingObjects
{
    public Transform drone;

    private float heightDrone = 1; 
    private Transform instDrone;

    public override void Land()
    {
        instDrone = Instantiate(drone, transform.position + new Vector3(0, heightDrone), 
            Quaternion.identity) as Transform;

        StartCoroutine(LandUpdate(2));
    }

    IEnumerator LandUpdate(float sec)
    {
        

        yield return new WaitForSeconds(sec);
        instDrone.gameObject.SetActive(false);
        //обновление сетки
        animator.SetTrigger("Land");
        Collider[] colliders = GetComponentsInChildren<Collider>();

        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (var c in colliders)
        {
            bounds.Encapsulate(c.bounds);
        }

        AstarPath.active.UpdateGraphs(bounds);
        Debug.Log(bounds.size);
    }
}
