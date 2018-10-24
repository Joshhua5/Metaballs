using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MetaBallsImageEffect : MonoBehaviour {

    int width, height;

    Texture2D _texture;
    Color[] _textureData;
    Transform _transform;
    Camera _camera;

    CircleCollider2D[] _colliders;
	// Use this for initialization
	void Start () {
        width = Display.main.renderingWidth;
        height = Display.main.renderingHeight;

        _texture = new Texture2D(width, height, TextureFormat.RGBA32, false, true);
        _transform = transform;
        _camera = GetComponent<Camera>(); 
	}
     
    public float intensity(int x, int y)
    {
        // (0, 0) should be (-width, -5)
        // (512, 512) should be (width, 5)

        var w2 = _camera.aspect * _camera.orthographicSize;
        var h2 = _camera.orthographicSize;

         
        float intensity = 0;
        for (var i = 0; i < _colliders.Length; ++i)
        {
            var circle = _colliders[i];

            // Pixel Position
            // From the bottom left corner, take the unit of the pixel position and scale it to the cameras visible width
            var pX = ((x / (float)width) * w2 * 2) + (_transform.position.x - w2);
            var pY = ((y / (float)height) * h2 * 2) + (_transform.position.y - h2);
             
            // Circle Position
            var cX = circle.transform.position.x;
            var cY = circle.transform.position.y; 

            // Calculate the distance squared
            var xComp = (pX - cX) * (pX - cX);  
            var yComp = (pY - cY) * (pY - cY);

            var radius = circle.radius;

            // Intensity
            intensity += (radius * radius) / (xComp + yComp);
        }
        return intensity;
    }

    public void processTexture()
    { 
        _colliders = FindObjectsOfType<CircleCollider2D>();
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                var result = intensity(x, y); 
                if(result > 1) {
                    int index = x + (y * width);
                    _textureData[index].r = 
                    _textureData[index].g = 
                    _textureData[index].b = result; 
                }
            }
        }
        _texture.SetPixels(_textureData);
        _texture.Apply(); 
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture.active = source; 

        // Reads the pixels from the source into the _texture
        _texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        _textureData = _texture.GetPixels(); 

        processTexture();

        Graphics.Blit(_texture, destination, Vector2.one, Vector2.zero);
        RenderTexture.active = null;
    }
}
