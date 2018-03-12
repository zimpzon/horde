using UnityEngine;

namespace HordeEngine
{
    public class ProjectileRenderer
    {
        public Material ProjectileMaterial;
        public int SpritesPerRow;
        public int SpritesPerCol;

        Vector3[] vertices_;
        Vector2[] UV_;
        int[] indices_;
        Mesh mesh_ = new Mesh();
    }
}
