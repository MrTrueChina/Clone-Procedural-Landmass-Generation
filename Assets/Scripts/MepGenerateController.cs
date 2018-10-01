using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MepGenerateController : MonoBehaviour
{
    [SerializeField]
    MeshRenderer _displayRenderer;
    [SerializeField]
    MeshFilter _displayFilter;

    [System.Serializable]
    enum GenerateMode
    {
        HeightMap,
        ColorsMap,
        MeshMap,
    }
    [SerializeField]
    GenerateMode _generateMode;

    [SerializeField]
    int _width;
    [SerializeField]
    int _height;
    [SerializeField]
    float _maxMeshHeight;
    [SerializeField]
    AnimationCurve _heightCurve;
    [SerializeField]
    [Range(-3, 3)]
    int _detailLevel = 0;
    [SerializeField]
    float _scale;
    [SerializeField]
    int _octaves;
    [SerializeField]
    [Range(0, 1)]
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
        switch (_generateMode)
        {
            case GenerateMode.HeightMap:
                GenerateHeightMap();
                break;

            case GenerateMode.ColorsMap:
                GenerateColorsMap();
                break;

            case GenerateMode.MeshMap:
                GenerateMeshMap();
                break;
        }
    }

    void GenerateHeightMap()
    {
        _displayFilter.sharedMesh = MyMapGenerator.GenerateQuad(_width, _height);
        _displayRenderer.sharedMaterial.mainTexture = MyMapGenerator.GenerateHeightMap(GetHeightMap());
    }

    void GenerateColorsMap()
    {
        _displayFilter.sharedMesh = MyMapGenerator.GenerateQuad(_width, _height);

        float[,] heightMap = GetHeightMap();
        Color[] colorsMap = GetColorsMap(heightMap);
        _displayRenderer.sharedMaterial.mainTexture = MyMapGenerator.GenerateColorsMap(colorsMap, heightMap.GetLength(0), heightMap.GetLength(1));
    }

    void GenerateMeshMap()
    {
        float[,] heightMap = GetHeightMap();
        Color[] colorsMap = GetColorsMap(heightMap);
        float meshScale = 1 / Mathf.Pow(2, _detailLevel);
        _displayFilter.sharedMesh = MyMapGenerator.GenerateHeightMesh(heightMap, _heightCurve, _maxMeshHeight, meshScale);
        _displayRenderer.sharedMaterial.mainTexture = MyMapGenerator.GenerateColorsMap(colorsMap, heightMap.GetLength(0), heightMap.GetLength(1));
    }


    float[,] GetHeightMap()
    {
        int width = (int)(_width * Mathf.Pow(2, _detailLevel));
        int height = (int)(_height * Mathf.Pow(2, _detailLevel));
        float scale = _scale * Mathf.Pow(2, _detailLevel);
        return PerlinNoise.GeneratePerlinNoiseMap(width, height, _seed, scale, _octaves, _persistence, _lacunarity, _offset);
    }

    Color[] GetColorsMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float currentHeight = heightMap[x, y];
                for (int i = 0; i < _regions.Length; i++)
                {
                    if (currentHeight > _regions[i].height)
                    {
                        colorMap[y * width + x] = _regions[i].color;
                        break;
                    }
                }
            }
        }
        return colorMap;
    }



    private void OnValidate()
    {
        if (_width < 1) _width = 1;
        if (_height < 1) _height = 1;
        if (_scale < 0) _scale = 0;
        if (_octaves < 1) _octaves = 1;
        if (_lacunarity < 1) _lacunarity = 1;

        GenerateMap();
    }
}
