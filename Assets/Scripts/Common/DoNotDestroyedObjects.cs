using UnityEngine;

namespace Assets.Scripts.Common
{
    public class DoNotDestroyedObjects : MonoBehaviour
    {

        public GameObject[] AutoCreatedObjects; 
        public static DoNotDestroyedObjects I
        {
            get; private set;
        }
        public void Awake ()
        {
            if (I != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            I = this;
            DontDestroyOnLoad(gameObject);

            for (var i = 0; i < AutoCreatedObjects.Length; ++i)
            {
                AutoCreatedObjects[i].SetActive(true);
            }
        }

        public void Add(GameObject obj)
        {
            obj.transform.SetParent(transform);
        }
    }
}
