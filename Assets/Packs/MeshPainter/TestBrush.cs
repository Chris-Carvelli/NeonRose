using UnityEngine;

namespace Packs.MeshPainter
{
    public class TestBrush : MonoBehaviour
    {
        public float radius = 1;
        public float hardness = .5f;
        public float strength = .5f;
        [ColorUsage(true, true)]
        public Color color = Color.red;

        private void OnTriggerStay(Collider other)
        {
            Paintable p = other.transform.GetComponent<Paintable>();
            if (p != null)
            {
                Vector3 pos = transform.position;
                MeshPainterManager.Instance.Paint(p, pos, radius, hardness, strength, color);
            }
        }
    }
}