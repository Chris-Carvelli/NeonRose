using UnityEngine;

namespace Packs.MeshPainter
{
    public class TestBrush : MonoBehaviour
    {
        public float radius = 1;
        public float hardness = .5f;
        public float strength = .5f;
        public Color color = Color.red;
        private void OnCollisionStay(Collision collision)
        {
            Paintable p = collision.transform.GetComponent<Paintable>();
            if (p != null)
            {
                Vector3 pos = collision.contacts[0].point;
                MeshPainterManager.Instance.Paint(p, pos, radius, hardness, strength, color);
            }
        }
    }
}