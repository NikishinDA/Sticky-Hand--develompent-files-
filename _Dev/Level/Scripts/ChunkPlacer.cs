using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChunkPlacer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Chunk chunkPrefab;
    [SerializeField] private Chunk[] firstPrefab;
    [SerializeField] private Chunk finalPrefab;
    [SerializeField] private float spawnDistance;
    [SerializeField] private int concurrentChunkNumber;
    [SerializeField] private Transform chunkParent;
    [SerializeField] private LevelTemplate[] templates;
    private LevelTemplate _template;
    private List<Chunk> _spawnedChunks;
    private bool _finishSpawned;
    private int _currentLength;
    private int _level;
    private int _secondLevel;
    private List<ChunkTemplate> _chunks;
    private Dictionary<int, int> _levelPairs;
    private void Awake()
    {
        _spawnedChunks = new List<Chunk>();
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<GameStartEvent>(OnGameStart);
        InitializeDictionary();
        _level = PlayerPrefs.GetInt(PlayerPrefsStrings.Level, 1) - 1;
        if (_level >= 10)
        {
            _level %= 10;
            _chunks = templates[_level].chunks.ToList();
            _chunks = _chunks.Concat(templates[_levelPairs[_level+1]-1].chunks).ToList();
        }
        else
        {
            _level %= 10;
            _chunks = templates[_level].chunks.ToList();
        }
        //_secondLevel = Random.Range(0, templates.Length);
        //_secondLevel = _level == _secondLevel ? (_secondLevel + 1) % 10 : _secondLevel;//.Concat(templates[_secondLevel].chunks).ToList();
        //_template = _level < 10 ? templates[_level] : templates[Random.Range(0, templates.Length)];
        VarSaver.LevelLength = _chunks.Count;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);
    }

    private void OnGameStart(GameStartEvent obj)
    {
    }

    private void Start()
    {
        _currentLength = 0;
        foreach (Chunk ch in firstPrefab)
        {
            //ch.SetEnv(_level);
            _spawnedChunks.Add(ch);
        }
    }

    private void Update()
    {
        if ((!_finishSpawned) &&
            (playerTransform.position.z >
             _spawnedChunks[_spawnedChunks.Count - 1].end.position.z - spawnDistance))
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        Chunk newChunk;
        if (_currentLength < _chunks.Count)
        {
            newChunk = Instantiate(chunkPrefab, chunkParent);
            newChunk.transform.position =
                _spawnedChunks[_spawnedChunks.Count - 1].end.position - newChunk.begin.localPosition;
            _spawnedChunks.Add(newChunk);
            newChunk.InitializeChunk(_chunks[_currentLength]);
            //newChunk.SetEnv(_level);
        }
        else
        {
            newChunk = Instantiate(finalPrefab, chunkParent);
            newChunk.transform.position =
                _spawnedChunks[_spawnedChunks.Count - 1].end.position - newChunk.begin.localPosition;
            _spawnedChunks.Add(newChunk);
            _finishSpawned = true;
        }

        

        _currentLength++;
        if (_spawnedChunks.Count > concurrentChunkNumber)
        {
            Destroy(_spawnedChunks[0].gameObject);
            _spawnedChunks.RemoveAt(0);
        }
    }

    private void OnGameOver(GameOverEvent obj)
    {
        _finishSpawned = true;
    }

    private void InitializeDictionary()
    {
        _levelPairs = new Dictionary<int, int>
        {
            {1, 3},
            {2, 4},
            {3, 6},
            {4, 7},
            {5, 9},
            {6, 3},
            {7, 4},
            {8, 5},
            {9, 2},
            {10, 1}
        };
    }
}