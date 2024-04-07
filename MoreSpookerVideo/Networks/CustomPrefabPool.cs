using Photon.Pun;
using UnityEngine;

namespace MoreSpookerVideo.Networks
{
    public class CustomPrefabPool : MonoBehaviour, IPunPrefabPool
    {
        public GameObject? prefabToPool;

        public GameObject? InstantiateGameObject(GameObject go, Vector3 position, Quaternion rotation)
        {
            prefabToPool = go;
            return Instantiate(go.name, position, rotation);
        }

        public GameObject? Instantiate(string prefabId, Vector3 position, Quaternion rotation)
        {
            if (prefabToPool)
            {
                return Instantiate(prefabToPool, position, rotation);
            }

            return null;
        }

        public void Destroy(GameObject gameObject)
        {
            Destroy(gameObject);
        }
    }
}
