using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator
{
    public static MeshData GeneratorTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float leftX = -(width - 1) / 2f;
        float topZ = -(height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);

        int vertexIndex = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                meshData.Vertices[vertexIndex] = new Vector3(x + leftX, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, y + topZ);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTrangle(vertexIndex, vertexIndex + width, vertexIndex + width + 1);     //顺时针存入，注意看好顶点的实际坐标
                    meshData.AddTrangle(vertexIndex, vertexIndex + width + 1, vertexIndex + 1);
                } 

                vertexIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData
{
    public Vector3[] Vertices
    {
        get { return _vertices; }
    }
    Vector3[] _vertices;
    public Vector2[] uvs
    {
        get { return _uvs; }
    }
    Vector2[] _uvs;
    int[] _triangles;

    int _triangleIndex = 0;


    public MeshData(int width, int height)
    {
        _uvs = new Vector2[width * height];
        _vertices = new Vector3[width * height];                //顶点数组，顶点数量对应高度图的每个点
        _triangles = new int[(width - 1) * (height - 1) * 6];
        /*
         *  三角形数组，或者是三角面数组，实际存储的是每个三角形的三个顶点在顶点数组里的索引值
         *  
         *  一般来说模型是由三角面构成的，每个角是一个顶点，那么每一个三角形需要三个顶点
         *  地形是顶点连接起来的网格，每四个顶点连接成一个方格，每个方格由两个三角面拼接而来
         *      种树问题，方格数量是(顶点数组长 - 1) * (顶点数组宽 - 1)
         *      每个方格由两个三角面拼接，三角面数量 = 方格数量 * 2
         *      每个三角面需要三个顶点，订单数量 = 三角面数量 * 3
         *      最后的数量就是上面的 (width - 1) * (height - 1) * 6
         */
    }


    public void AddTrangle(int a, int b, int c)
    {
        _triangles[_triangleIndex] = a;
        _triangles[_triangleIndex + 1] = b;
        _triangles[_triangleIndex + 2] = c;
        _triangleIndex += 3;
    }


    public Mesh ToMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.uv = _uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
