using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}


public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    MapDisplay _displayer;

    [System.Serializable]
    enum DrawMode
    {
        NoiseMap,
        ColorsMap,
        Mesh,
    }
    [SerializeField]
    DrawMode _drawMode;

    [SerializeField]
    int _width;
    [SerializeField]
    int _height;
    [SerializeField]
    AnimationCurve _heightCurve;
    [SerializeField]
    float _maxMeshHeight;
    [SerializeField]
    float _scale;
    [SerializeField]
    int _octaves;
    [SerializeField]
    [Range(0,1)]
    float _persistence;
    [SerializeField]
    float _lacunarity;
    [SerializeField]
    int _seed;
    [SerializeField]
    Vector2 _offset;

    [SerializeField]
    TerrainType[] _regions;


    public void GenerateMap()
    {
        float[,] heightMap = PerlinNoise.GeneratePerlinNoiseMap(_width, _height, _seed, _scale, _octaves, _persistence, _lacunarity, _offset);
        Color[] colorMap = GetColorMap(heightMap);

        switch (_drawMode)
        {
            case DrawMode.NoiseMap:
                _displayer.DrawTexture2D(TextureGenerator.TextureFromHeightMap(heightMap));
                break;

            case DrawMode.ColorsMap:
                _displayer.DrawTexture2D(TextureGenerator.TextureFromColorMap(colorMap, _width, _height));
                break;

            case DrawMode.Mesh:
                _displayer.DrawMesh(MeshGenerator.GeneratorTerrainMesh(heightMap, _maxMeshHeight, _heightCurve), TextureGenerator.TextureFromColorMap(colorMap, _width, _height));
                break;
        }
    }

    Color[] GetColorMap(float[,] heightMap)
    {
        Color[] colorMap = new Color[_width * _height];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                float currentHeight = heightMap[x, y];
                for (int i = 0; i < _regions.Length; i++)
                {
                    if (currentHeight > _regions[i].height)
                    {
                        colorMap[y * _width + x] = _regions[i].color;
                        break;
                    }
                }
            }
        }
        return colorMap;
    }



    private void OnValidate()                   //OnValidate：当Inspector面板里的数值变化时自动调用
    {
        if (_width < 1) _width = 1;
        if (_height < 1) _height = 1;
        if (_scale < 0) _scale = 0;
        if (_octaves < 1) _octaves = 1;
        if (_lacunarity < 1) _lacunarity = 1;

        GenerateMap();                          //干脆把生成地图挪到这算了，每次修改数值就自动生成地图
    }
}
