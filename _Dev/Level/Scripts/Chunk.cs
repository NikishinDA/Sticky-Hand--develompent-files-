using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class Chunk : MonoBehaviour
{
    public Transform begin;
    public Transform end;
    [SerializeField] private float width;
    [SerializeField] private float length;

    [SerializeField] private GameObject[] chunkObjects;
    [SerializeField] private GameObject plainObject;
    [SerializeField] private GameObject[] plains;
    [SerializeField] private Material plainMaterial;
    [SerializeField] private Transform plainSpawn;
    private Renderer _plainRenderer;
    [SerializeField] private LayerMask layerMask;

    private void Awake()
    {
        
    }

    public void InitializeChunk(ChunkTemplate template)
    {
        Vector3 objectPos = new Vector3();
            plainObject = Instantiate(plains[(int) template.chunkType], plainSpawn);
            _plainRenderer = plainObject.GetComponent<Renderer>();
        

        foreach (var levelObject in template.objects)
        {
            objectPos.x = levelObject.position.x * width;
            objectPos.z = levelObject.position.y * length;
            objectPos.y = 10f;
            Transform go = Instantiate(chunkObjects[(int) levelObject.type], transform).transform;
            float originY = go.position.y;
            go.localPosition = objectPos;
            RaycastHit hit;
           if( Physics.Raycast(
                go.position,
                Vector3.down,
                out hit,
                20f,
                layerMask))
           {
               objectPos.y = originY + hit.point.y;

               go.localPosition = objectPos;
               
           }
           Debug.DrawRay(go.position, Vector3.down * 15f, Color.green, 60f);
        }
    }

    public void SetEnv(int level)
    {
        if (!_plainRenderer)
        {
            _plainRenderer = plainObject.GetComponent<Renderer>();
        }
        if (level / 5 % 2 == 1)
        {
            _plainRenderer.materials = new[] {plainMaterial, plainMaterial};
        }
    }
}