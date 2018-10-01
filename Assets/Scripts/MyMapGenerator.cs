using UnityEngine;

public static class MyMapGenerator
{
    //生成高度图
    public static Texture2D GenerateHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] colorsMap = new Color[width * height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                colorsMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);

        Texture2D texture = new Texture2D(width, height);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorsMap);
        texture.Apply();
        return texture;
    }


    //生成彩色地图
    public static Texture2D GenerateColorsMap(Color[] colorsMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorsMap);
        texture.Apply();
        return texture;
    }


    //生成地图网格
    public static Mesh GenerateHeightMesh(float[,] heightMap, AnimationCurve heightCurve, float maxHeight = 16, float scale = 1)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = GetVertices(heightMap,heightCurve, maxHeight, scale);
        mesh.triangles = GetTriangles(heightMap);
        mesh.uv = GetUvs(heightMap);
        mesh.RecalculateNormals();

        return mesh;
    }
    static Vector3[] GetVertices(float[,] heightMap, AnimationCurve heightCurve, float maxHeight, float scale)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        float xOffset = -width / 2f;
        float yOffset = -height / 2f;

        Vector3[] vertices = new Vector3[width * height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                vertices[y * width + x] = new Vector3((x + xOffset) * scale, heightCurve.Evaluate(heightMap[x, y]) * maxHeight, (y + yOffset) * scale);

        return vertices;
    }
    private static int[] GetTriangles(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        int[] triangles = new int[(width - 1) * (height - 1) * 6];

        int triangleWidth = width - 1;
        int triangleHeight = height - 1;
        for (int y = 0; y < triangleHeight; y++)
        {
            for (int x = 0; x < triangleWidth; x++)
            {
                int currentIndex = y * width + x;
                int currentTrtangleIndex = (y * triangleWidth + x) * 6;

                triangles[currentTrtangleIndex] = currentIndex;
                triangles[currentTrtangleIndex + 1] = currentIndex + width;
                triangles[currentTrtangleIndex + 2] = currentIndex + width + 1;

                triangles[currentTrtangleIndex + 3] = currentIndex;
                triangles[currentTrtangleIndex + 4] = currentIndex + width + 1;
                triangles[currentTrtangleIndex + 5] = currentIndex + 1;
            }
        }

        return triangles;
    }
    static Vector2[] GetUvs(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Vector2[] uvs = new Vector2[width * height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                uvs[y * width + x] = new Vector2(x / (float)width, y / (float)height);

        return uvs;
    }


    //生成平面
    public static Mesh GenerateQuad(float width, float height, float scale = 1)
    {
        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[]
            {
                new Vector3(-width/2f, 0, -height/2f) * scale,
                new Vector3(width/2f , 0, -height/2f) * scale,
                new Vector3(-width/2f, 0, height/2f) * scale,
                new Vector3(width/2f, 0, height/2f) * scale
            };

        mesh.triangles = new int[] { 0, 2, 3, 0, 3, 1 };

        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };

        mesh.RecalculateNormals();

        return mesh;
    }
}