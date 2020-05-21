using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpawnManaging
{
    public class SpawnManager : GameEventListener
    {
        [SerializeField]
        public Transform target;
        [SerializeField]
        GameEvent breakEnd;
        [SerializeField]
        GameEvent breakStart;
        
        public Transform[] spawnPoints;
        [SerializeField]
        Wave[] wave;
        bool paused;
        int waveIndex;
        int spawnerIndex;
        [SerializeField]
        float breakLength;
        int spawnedInGroup;
        int spawnMagicNumber;
        public bool breakOn;
        // Update is called once per frame
        void Start()
        {
            spawnMagicNumber = 2;
            breakOn = true;
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
                    InitBreak();
                }
                else
                {
                    InitBreak();
                    Debug.Log("All waves cleared");//TODO
                }
                return;
            }
            spawnedInGroup++;
            Instantiate(obj, spawnPoints[spawnerIndex]).GetComponent<EnemyAgentMotivator>().target1 = target;
        }
        private void SpawnLoop()
        {
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
        private void InitBreak()
        {
            InvokeRepeating("BreakCheck", 0, 1);
        }
        private void BreakCheck()
        {
            Debug.Log(spawnMagicNumber);
            Debug.Log(spawnPoints[0].childCount);
            if (spawnPoints[0].childCount == spawnMagicNumber)
            {
                CancelInvoke("BreakCheck");
                StartBreak();
            }
        }
        private void StartBreak()
        {
            breakStart.Raise();
            breakOn = true;
        }
        public void EndBreak()
        {
            if(breakOn)
            {
                breakEnd.Raise();
                InvokeRepeating("SpawnLoop", 0, 1);
                breakOn = false;
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                EndBreak();
            }
        }
    }
    [Serializable]
    public class Wave
    {
        public WaveObject waveData;
        [Tooltip("interval between spawns")]
        public float interval;
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

