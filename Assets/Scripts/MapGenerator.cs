using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    MapDisplay _displayer;
    [SerializeField]
    int _width;
    [SerializeField]
    int _height;
    [SerializeField]
    float _scale;


    public void GenerateMap()
    {
        float[,] map = PerlinNoise.GeneratePerlinNoiseMap(_width, _height, _scale);

        _displayer.DrawNoiseMap(map);
    }
}
