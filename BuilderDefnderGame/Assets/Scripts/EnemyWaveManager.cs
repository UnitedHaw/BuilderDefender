using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance { get; private set; }

    public EventHandler OnWaveNumberChanged;
    private enum State
    {
        WaitingTOSpawnNextWave,
        SpawingWave,
    }
    [SerializeField] private List<Transform> spawnPositionTransform;
    [SerializeField] private Transform nextWaveSpawnPositionTransform;
    [SerializeField] private int firstWaveEnemyAmount;
    private int waveNumber;
    private State state;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int remaningEnemySpawnAmount;
    private Vector3 spawnPosition;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        state = State.WaitingTOSpawnNextWave;
        spawnPosition = spawnPositionTransform[UnityEngine.Random.Range(0, spawnPositionTransform.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
        nextWaveSpawnTimer = 3f;
    }
    private void Update()
    {
        switch (state)
        {
            case State.WaitingTOSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer < 0)
                {
                    SpawnWave();
                }
                break;
            case State.SpawingWave:
                if (remaningEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0, .2f);
                        Enemy.Create(spawnPosition + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
                        remaningEnemySpawnAmount--;

                        if(remaningEnemySpawnAmount <= 0)
                        {
                            state = State.WaitingTOSpawnNextWave;
                            spawnPosition = spawnPositionTransform[UnityEngine.Random.Range(0, spawnPositionTransform.Count)].position;
                            nextWaveSpawnPositionTransform.position = spawnPosition;
                            nextWaveSpawnTimer = 10f;
                        }
                    }
                }
                break;
        }       
    }
    private void SpawnWave()
    {
        remaningEnemySpawnAmount = firstWaveEnemyAmount + waveNumber * 3;
        state = State.SpawingWave;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public float GetNextWaveSpawnTimer()
    {
        return nextWaveSpawnTimer;
    }
    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}
