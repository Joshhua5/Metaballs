using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MetaBallsImageEffectGPU : MonoBehaviour {

    public Material _metaballMaterial;
    public float Cutoff = 1;

    //Texture2D _texture;
    Color[] _textureData;
    Transform _transform;
    Camera _camera;

    CircleCollider2D[] _colliders;
    List<Vector4> _ballPositions = new List<Vector4>(64);
	// Use this for initialization
	void Start () {  
        _transform = transform;
        _camera = GetComponent<Camera>(); 

        _colliders = FindObjectsOfType<CircleCollider2D>();

        Time.timeScale = 0.5f;
	}  

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_colliders == null || _colliders.Length == 0)
            return;

        _metaballMaterial.SetFloat("_Aspect", _camera.aspect);
        _metaballMaterial.SetFloat("_Height", _camera.orthographicSize);
        _metaballMaterial.SetVector("_Position", _camera.transform.position);
        _metaballMaterial.SetFloat("_Cutoff", Cutoff);

        _metaballMaterial.SetInt("_BallCount", _colliders.Length);
        _ballPositions.Clear();
        for(int i = 0; i < _colliders.Length - 1; i++)
        {
            var transform = _colliders[i].transform;

            _ballPositions.Add(
                new Vector4(transform.position.x, transform.position.y,
                Mathf.Pow(_colliders[i].radius, 2f), 1)
            );
        } 

        _metaballMaterial.SetVectorArray("_BallPosition", _ballPositions);
         
        Graphics.Blit(source, destination, _metaballMaterial);   
        return;
    }
     
}
