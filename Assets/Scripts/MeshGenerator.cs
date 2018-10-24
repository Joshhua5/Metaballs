using UnityEngine;
using System.Collections;

public class MeshGenerator : MonoBehaviour
{
    const int w = 512, h = 512;

    float[,] _scalarField= new float[w, h];
    float _threshold = 0.5f;

    public void Start()
    {
        // Initialize the scalar field
        for (int x = 0; x < w; ++x)
            for (int y = 0; y < h; ++y)
                _scalarField[x, y] = (float)Perlin.perlin((float)x, (float)y, 0f);


        for (int x = 0; x < w - 1; ++x)
        {
            for (int y = 0; y < h - 1; ++y)
            {
                int index =
                    ((_scalarField[x + 0, y + 0] > _threshold) ? 1 : 0) << 0 |
                    ((_scalarField[x + 1, y + 0] > _threshold) ? 1 : 0) << 1 |
                    ((_scalarField[x + 1, y + 1] > _threshold) ? 1 : 0) << 2 |
                    ((_scalarField[x + 0, y + 1] > _threshold) ? 1 : 0) << 3;
            }
        }
    }

    // Here we need to turn the lookup into 
    float[,] Lookup =
    {
        { 0 }
    };
} 