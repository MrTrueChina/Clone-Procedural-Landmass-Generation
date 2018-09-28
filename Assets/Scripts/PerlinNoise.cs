using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoise
{
    public static float[,] GeneratePerlinNoiseMap(int width, int height, float scale)
    {
        float[,] map = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sampleX = x * scale;
                float sampleY = y * scale;
                map[x, y] = Mathf.PerlinNoise(sampleX, sampleY);
            }
        }

        return map;
    }
}
