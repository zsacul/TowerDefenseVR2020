using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpawnManaging
{
    public class SpawnManager : GameEventListener
    {
        [SerializeField]
        GameEvent breakEnd;
        [SerializeField]
        GameEvent breakStart;
        [SerializeField]
        Transform[] spawnPoints;
        [SerializeField]
        Wave[] wave;
        bool paused;
        int waveIndex;
        int spawnerIndex;
        [SerializeField]
        float breakLength;
        int spawnedInGroup;
        // Update is called once per frame
        void Start()
        {
            InvokeRepeating("SpawnLoop", 0, 1);
            wave[waveIndex].waveData = ScriptableObject.Instantiate(wave[waveIndex].waveData);
        }
        private void Spawn()
        {
            if (spawnedInGroup >= wave[waveIndex].groupSize)
            {
                CancelInvoke("Spawn");
                return;
            }
            GameObject obj = wave[waveIndex].waveData.GetEnemy();
            if(obj == null)//wave end
            {
                CancelInvoke("Spawn");
                CancelInvoke("SpawnLoop");
                waveIndex++;
                if(waveIndex < wave.Length)
                {
                    wave[waveIndex].waveData = ScriptableObject.Instantiate(wave[waveIndex].waveData);
                    InvokeRepeating("SpawnLoop", breakLength, 1);
                    breakStart.Raise();
                }
                else
                {
                    Debug.Log("All waves cleared");//TODO
                }
                return;
            }
            spawnedInGroup++;
            Instantiate(obj, spawnPoints[spawnerIndex]);
        }
        private void SpawnLoop()
        {
            breakEnd.Raise();
            if (!IsInvoking("Spawn"))
            {
                spawnedInGroup = 0;
                spawnerIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
                InvokeRepeating("Spawn", wave[waveIndex].nextGroupTime, wave[waveIndex].interval);
            }
        }
        private void EndSpawn()
        {
            CancelInvoke("Spawn");
        }

        public override void OnEventRaised(UnityEngine.Object data)
        {
            CancelInvoke("SpawnLoop");
            CancelInvoke("Spawn");
        }
    }
    [Serializable]
    public class Wave
    {
        public WaveObject waveData;
        [Tooltip("interval between spawns")]
        public float interval;
        [Tooltip("maxLength = 0 is ignored")]
        public float maxLength;
        [Min(0.1f)]
        public float bonusHp = 1;
        [Min(0.1f)]
        public float bonusSpeed = 1;
        [Min(1)]
        public int groupSize = 1;
        [Tooltip("time from end of last group to begining of new")]
        public float nextGroupTime;
    }
}

