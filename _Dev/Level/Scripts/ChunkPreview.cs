using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class ChunkPreview : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private Chunk chunkPrefab;
    [SerializeField] private Chunk[] firstPrefab;
    [SerializeField] private Chunk finalPrefab;
    [SerializeField] private Transform chunkParent;
    [SerializeField] private LevelTemplate template;
    [Header("Build")] [SerializeField] private bool build;
    [SerializeField] private bool rebuild;

    private void Update()
    {
        if (build)
        {
            build = false;
            BuildLevel();
        }

        if (rebuild)
        {
            rebuild = false;
            if (chunkParent)
                DestroyImmediate(chunkParent.gameObject);
            BuildLevel();
        }
    }

    private void BuildLevel()
    {
        List<Chunk> spawnedChunks = firstPrefab.ToList();
        int currentLength = spawnedChunks.Count;
        if (!chunkParent)
        {
            chunkParent = new GameObject().transform;
        }

        Chunk newChunk;
        foreach (var chunkTemplate in template.chunks)
        {
            newChunk = Instantiate(chunkPrefab, chunkParent);
            newChunk.transform.position =
                spawnedChunks[spawnedChunks.Count - 1].end.position - newChunk.begin.localPosition;
            spawnedChunks.Add(newChunk);
            newChunk.InitializeChunk(chunkTemplate);
        }

        newChunk = Instantiate(finalPrefab, chunkParent);
        newChunk.transform.position =
            spawnedChunks[spawnedChunks.Count - 1].end.position - newChunk.begin.localPosition;
        spawnedChunks.Add(newChunk);
    }
}