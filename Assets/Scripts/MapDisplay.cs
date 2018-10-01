using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    [SerializeField]
    Renderer _textureRenderer;
    [SerializeField]
    MeshFilter _meshFilter;
    [SerializeField]
    MeshRenderer _meshRenderer;


    public void DrawTexture2D(Texture2D texture)
    {
        _textureRenderer.sharedMaterial.mainTexture = texture;
        _textureRenderer.transform.localScale = new Vector3(texture.width / 10, 1, texture.height / 10);
    }

    public void DrawMesh(MeshData meshDatea, Texture2D texture)
    {
        _meshFilter.sharedMesh = meshDatea.ToMesh();
        _meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
