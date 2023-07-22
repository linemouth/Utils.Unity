using UnityEngine;

namespace Utils.Unity
{
    public class Lifetime : MonoBehaviour
    {
        [Range(0, float.MaxValue)]
        public float duration = 1;

        private void Start()
        {
            Destroy(gameObject, duration);
        }
    }
}