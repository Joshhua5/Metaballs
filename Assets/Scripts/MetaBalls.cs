using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaBalls : MonoBehaviour {

    const int width = 512, height = 512;

    Texture2D _texture;
    Color[] _textureData;
    Transform _transform;

    CircleCollider2D[] _colliders;
	// Use this for initialization
	void Start () {
        _texture = new Texture2D(width, height, TextureFormat.RGBA32, false, true);
        _textureData = _texture.GetPixels();
        _transform = transform;

        _colliders = FindObjectsOfType<CircleCollider2D>();

        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0, 0, width, height),  Vector2.zero, width);
	}
  
    public float intensity(int x, int y)
    {
        float intensity = 0;
        for (var i = 0; i < _colliders.Length; ++i)
        {
            var circle = _colliders[i];

            // Pixel Position
            var pX = (x / (float)width * _transform.localScale.x ) + _transform.position.x;
            var pY = (y / (float)height * _transform.localScale.y) + _transform.position.y;
             
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

	
	// Update is called once per frame
	void Update () {

        _colliders = FindObjectsOfType<CircleCollider2D>();
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                _textureData[x + (y * width)].r = intensity(x, y);
                //_textureData[x + (y * width)].r = intensity(x, y) > 1 ? 1 : 0;
            }
        }
        _texture.SetPixels(_textureData);
        _texture.Apply();
	}
}
