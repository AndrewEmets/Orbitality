using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    [SerializeField] private int resolution;
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private float topLevel;
    
    [SerializeField] private float maskFrequency;
    [SerializeField] private float maskLevel;
    [SerializeField] private float maskWidth;

    [ContextMenu("Test generate")]
    public void Test()
    {
        GenerateModel(150);
    }
    
    public void GenerateModel(float seed)
    {
        resolution = Mathf.Max(1, resolution);
        
        var mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        Generate(mesh, seed);   
    }

    private static readonly List<Vector3> vertices = new List<Vector3>();
    private static readonly List<int> triangles = new List<int>();
    private static readonly List<Color> colors = new List<Color>();
    private static readonly List<Vector4> uvs = new List<Vector4>();

    private void Generate(Mesh mesh, float seed)
    {
        vertices.Clear();
        triangles.Clear();
        colors.Clear();
        uvs.Clear();
        mesh.Clear();
        
        var rotation = Quaternion.identity;

        for (var r = 0; r < 6; r++)
        {
            var vc = vertices.Count;

            for (var i = 0; i <= resolution; i++)
            for (var j = 0; j <= resolution; j++)
            {
                var uv = new Vector2((float) j / resolution, 1 - (float) i / resolution) * 2 - Vector2.one;
                uv = new Vector2(Mathf.Tan(uv.x) / Mathf.Tan(1), Mathf.Tan(uv.y) / Mathf.Tan(1));
                uv *= 1f - 1f / (resolution + 1);

                var pos = rotation * new Vector3(uv.x, uv.y, -1).normalized;

                var modifier = Perlin.Noise(pos * frequency + Vector3.one * seed) +
                               Perlin.Noise(pos * (frequency * 2) + Vector3.one * seed) / 2f;
                modifier /= 1.5f;
                modifier *= amplitude;

                // mask
                modifier *= MathfExt.Smoothstep(maskLevel - maskWidth / 2f, maskLevel + maskWidth / 2, Perlin.Noise(pos * maskFrequency + Vector3.one * seed));

                // applying noise
                pos *= 1 + Mathf.Max(0, modifier);

                uvs.Add(new Vector4(modifier/(topLevel*amplitude), 0, 0, 0));

                vertices.Add(pos);
            }

            // making triangles
            for (var i = 0; i < resolution; i++)
            {
                for (var j = 0; j < resolution; j++)
                {
                    var isEven = (i + j) % 2 == 0;
                    var n = i + j * (resolution + 1) + vc;

                    triangles.Add(n);
                    triangles.Add(n + 1);
                    triangles.Add(n + resolution + 1 + (isEven ? 0 : 1));

                    triangles.Add(n + (isEven ? 1 : 0));
                    triangles.Add(n + resolution + 2);
                    triangles.Add(n + resolution + 1);
                }
            }

            rotation *= Quaternion.Euler(90, 0, 0);
            rotation *= Quaternion.Euler(0, r % 2 == 0 ? -90 : 90, 0);
        }

        // sewing faces

        for (var r = 0; r < 6; r++)
        for (var i = 0; i < resolution; i++)
        for (var j = 0; j < 2; j++)
        {
            var top = j == 0;
            var rIsEven = r % 2 == 0;

            var nextFaceIndex = top ? (r + 2) % 6 : (r + 5) % 6;

            var i0 = getTopDownVertexIndex(i, r, top, true);
            var i1 = getLeftRightVertexIndex(i, nextFaceIndex, rIsEven, rIsEven ? top : !top);

            var i2 = getTopDownVertexIndex(i + 1, r, top, true);
            var i3 = getLeftRightVertexIndex(i + 1, nextFaceIndex, rIsEven, rIsEven ? top : !top);

            triangles.Add(i0);
            triangles.Add(top ? i1 : i2);
            triangles.Add(top ? i2 : i1);

            triangles.Add(i3);
            triangles.Add(top ? i2 : i1);
            triangles.Add(top ? i1 : i2);
        }

        // corners
        {
            triangles.Add(getTopDownVertexIndex(0, 0, false, true));
            triangles.Add(getTopDownVertexIndex(0, 5, false, true));
            triangles.Add(getTopDownVertexIndex(0, 4, true, false));

            triangles.Add(getTopDownVertexIndex(0, 1, false, true));
            triangles.Add(getTopDownVertexIndex(0, 0, true, false));
            triangles.Add(getTopDownVertexIndex(0, 2, false, true));

            triangles.Add(getTopDownVertexIndex(0, 3, false, true));
            triangles.Add(getTopDownVertexIndex(0, 2, true, false));
            triangles.Add(getTopDownVertexIndex(0, 4, false, true));
            
            triangles.Add(getTopDownVertexIndex(0, 0, false, false));
            triangles.Add(getTopDownVertexIndex(0, 1, false, false));
            triangles.Add(getTopDownVertexIndex(0, 5, true, true));

            triangles.Add(getTopDownVertexIndex(0, 1, true, false));
            triangles.Add(getTopDownVertexIndex(0, 3, true, false));
            triangles.Add(getTopDownVertexIndex(0, 5, true, false));

            triangles.Add(getTopDownVertexIndex(0, 0, true, true));
            triangles.Add(getTopDownVertexIndex(0, 4, true, true));
            triangles.Add(getTopDownVertexIndex(0, 2, true, true));

            triangles.Add(getTopDownVertexIndex(0, 1, true, true));
            triangles.Add(getTopDownVertexIndex(0, 2, false, false));
            triangles.Add(getTopDownVertexIndex(0, 3, false, false));

            triangles.Add(getTopDownVertexIndex(0, 3, true, true));
            triangles.Add(getTopDownVertexIndex(0, 4, false, false));
            triangles.Add(getTopDownVertexIndex(0, 5, false, false));
        }

        mesh.SetVertices(vertices);
        mesh.SetUVs(0, uvs);
        mesh.SetColors(colors);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        
        

        int getTopDownVertexIndex(int i, int r, bool top, bool fromLeftToRight)
        {
            var offset = (resolution + 1) * (resolution + 1) * r;
            var res = top
                ? fromLeftToRight ? i : resolution - i
                : fromLeftToRight
                    ? (resolution + 1) * (resolution) + i
                    : (resolution + 1) * (resolution + 1) - 1 - i;
            return offset + res;
        }

        int getLeftRightVertexIndex(int i, int r, bool left, bool fromTopToBottom)
        {
            var offset = (resolution + 1) * (resolution + 1) * r;

            var res = left ? fromTopToBottom ? i * (resolution + 1) :
                (resolution - i) * (resolution + 1) :
                fromTopToBottom ? (resolution + 1) * (i + 1) - 1 : (resolution + 1) * (resolution + 1 - i) - 1;

            return offset + res;
        }
    }
}
