using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoise
{
    //octaves：倍频    persistence：持续度    lacunarity：隙度
    public static float[,] GeneratePerlinNoiseMap(int mapWidth, int mapHeight,int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        if (scale <= 0) scale = 0.0001f;


        Vector2[] octavesOffsets = GetOctavesOffsets(octaves, seed);


        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float[,] map = new float[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;        //振幅
                float frequency = 1;        //频率
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - mapWidth / 2f) / scale * frequency + octavesOffsets[i].x + offset.x;
                    float sampleY = (y - mapHeight / 2f) / scale * frequency + octavesOffsets[i].y + offset.y;
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                map[x, y] = noiseHeight;
            }
        }
        
        for (int y = 0; y < mapHeight; y++)
            for (int x = 0; x < mapWidth; x++)
                map[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, map[x, y]);   //Mathf.InverseLerp：反差值，计算第三个参数在前两个参数之间的比例值

        return map;
    }

    static Vector2[] GetOctavesOffsets(int octaves, int seed)
    {
        Vector2[] octavesOffsets = new Vector2[octaves];

        System.Random prng = new System.Random(seed);
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000);
            float offsetY = prng.Next(-100000, 100000);

            octavesOffsets[i] = new Vector2(offsetX, offsetY);
        }

        return octavesOffsets;
    }
}
