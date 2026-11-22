using UnityEngine;

public class StartBorder : MonoBehaviour
{
    public Package package;
    public float wallThickness = 1f;
    public float extraHeight = 20f;   // extra wall height so nobody can jump over

    GameObject[] walls = new GameObject[2];

    void Start()
    {
        Camera cam = Camera.main;
        float halfW = cam.orthographicSize * cam.aspect;
        float height = cam.orthographicSize * 2f + extraHeight;

        // No friction, so players don't stick to the walls when jumping against them
        PhysicsMaterial2D slick = new PhysicsMaterial2D("Slick");
        slick.friction = 0f;
        slick.bounciness = 0f;

        for (int i = 0; i < 2; i++)
        {
            int side = i == 0 ? -1 : 1;

            GameObject wall = new GameObject(side < 0 ? "StartWallLeft" : "StartWallRight");
            wall.transform.position = new Vector3(
                cam.transform.position.x + side * (halfW + wallThickness / 2f),
                cam.transform.position.y,
                0f);

            BoxCollider2D col = wall.AddComponent<BoxCollider2D>();
            col.size = new Vector2(wallThickness, height);
            col.sharedMaterial = slick;

            walls[i] = wall;
        }
    }

    void Update()
    {
        // The first time anyone grabs the package, remove the walls for good
        if (package != null && package.holder != null)
        {
            foreach (GameObject w in walls) Destroy(w);
            Destroy(this);
        }
    }
}