using HordeEngine;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class PerfTestManager : MonoBehaviour
{
    public Material Mat;
    float dt_;
    public Text TextCount;
    public Text TextFps;
    Rect MovementBounds = new Rect(-5, -5, 10, 10);
    DynamicQuadMesh mesh;
    Projectile[] sprites_ = new Projectile[0];

    struct test
    {
        public Projectile[] Sprites;
    }

    struct Job : IJobParallelFor
    {
        public test Sprites;

        public void Execute(int i)
        {
            Sprites.Sprites[i].TickCallback(ref Sprites.Sprites[i], i);
        }
    }

    private void Awake()
    {
        mesh = new DynamicQuadMesh(1000);
    }

    bool TickProjectile(ref Projectile projectile, int idx)
    {
        projectile.Origin += projectile.Velocity * dt_;
        projectile.ActualPos.x = projectile.Origin.x + Mathf.Sin(Time.time + idx);
        projectile.ActualPos.y = projectile.Origin.y + Mathf.Cos(Time.time + idx);
        if (projectile.ActualPos.x < MovementBounds.xMin || projectile.ActualPos.x > MovementBounds.xMax)
        {
            projectile.Velocity.x *= -1;
        }

        if (projectile.ActualPos.y < MovementBounds.yMin || projectile.ActualPos.y > MovementBounds.yMax)
        {
            projectile.Velocity.y *= -1;
        }

        RenderProjectile(ref projectile);
        return true;
    }

    void RenderProjectile(ref Projectile projectile)
    {
        mesh.AddQuad(projectile.ActualPos, projectile.Size, 0.0f, projectile.Size.y, projectile.Color);
    }

    void Update()
    {
        dt_ = Time.deltaTime;

        mesh.Clear();
        UpdateSprites();
        mesh.ApplyChanges();

        Matrix4x4 matrix = Matrix4x4.identity;
        matrix.SetTRS(Vector3.zero, Quaternion.identity, Vector3.one);
        Graphics.DrawMesh(mesh.GetMesh(), matrix, Mat, 0);
    }

    void UpdateSprites()
    {
        //test t = new test();
        //t.Sprites = sprites_;

        //Job job = new Job();
        //job.Sprites = t;
        //job.Run(sprites_.Length);

        for (int i = 0; i < sprites_.Length; ++i)
        {
            sprites_[i].TickCallback(ref sprites_[i], i);
        }

        if (Time.frameCount % 20 == 0)
            TextFps.text = string.Format("{0:0.0} ms", Time.smoothDeltaTime * 1000);
    }

    public void OnButton()
    {
        int count = int.Parse(TextCount.text);
        CreateSprites(count);
    }

    void CreateSprites(int count)
    {
        sprites_ = new Projectile[count];
        for (int i = 0; i < count; ++i)
        {
            var spr = new Projectile()
            {
                Size = Vector2.one * 0.5f,
                StartPos = Random.insideUnitCircle * 2,
                Velocity = Random.insideUnitCircle * 0.5f,
                Color = new Color(Random.value, Random.value, Random.value),
                TickCallback = TickProjectile
            };
            spr.ActualPos = spr.StartPos;
            sprites_[i] = spr;
        }
    }
}
