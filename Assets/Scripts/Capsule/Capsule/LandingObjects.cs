using System.Collections;
using UnityEngine;
using Gradient = UnityEngine.Gradient;

namespace Capsule
{
    public class LandingObjects : MonoBehaviour
    {
        public Transform dummyObject;
        public int sizeOfGrid;

        protected Animator animator;

        private void Awake()
        {
            animator = transform.GetComponent<Animator>();
        }

        public virtual void Land()
        {
            StartCoroutine(LandUpdate(4));
        }

        IEnumerator LandUpdate(float sec)
        {
            animator.SetTrigger("Land");

            yield return new WaitForSeconds(sec);
            //обновление сетки
            Collider[] colliders = GetComponentsInChildren<Collider>();

            Bounds bounds = new Bounds(transform.position, Vector3.zero);
            foreach (var c in colliders)
            {
                bounds.Encapsulate(c.bounds);
            }

            Debug.Log("UpdateGriph");

            AstarPath.active.UpdateGraphs(bounds);
        }

        public void AnimFinished()
        {
            
        }
    }
}

