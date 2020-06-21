using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
        [SerializeField]
        LightningCycle lightningCycle;
        

        BreakButtonHandler BreakButton;
        public Transform[] spawnPoints;
        [SerializeField]
        Wave[] wave;
        [SerializeField]
        float breakDuration;
        bool paused;
        int waveIndex;
        int spawnerIndex;
        int spawnedInGroup;
        int spawnMagicNumber;
        public bool breakOn;

        public Canvas waveInfoLabelPrefab;
        private Canvas waveInfoLabel;

        float breakTime;
        // Update is called once per frame
        void Start()
        {
            waveInfoLabel = Instantiate(waveInfoLabelPrefab);
            spawnMagicNumber = 2;
            breakOn = true;
            wave[waveIndex].waveData = ScriptableObject.Instantiate(wave[waveIndex].waveData);
            breakTime = 0;
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
            UpdateUI();
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
            UpdateUI();
            breakTime = 0;
            StartCoroutine(lightningCycle.ChangeToDay());
        }

        public void EndBreak()
        {
            if(breakOn)
            {
                //if (BreakButton == null)
                //{
                //    BreakButton = FindObjectOfType<BreakButtonHandler>();
                //    if (BreakButton == null)
                //        Debug.LogError("Nie ma na planszy breakbuttona");
                //}
                breakEnd.Raise();
                InvokeRepeating("SpawnLoop", 3, 1);
                breakOn = false;
                StartCoroutine(lightningCycle.ChangeToNight());
            }
            UpdateUI();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton0) || breakTime > breakDuration)
            {
                EndBreak();
            }

            if (breakOn)
                breakTime += Time.deltaTime;
        }

        private void UpdateUI()
        {
            if (!breakOn)
            {
                Transform spawnPoint = spawnPoints[0].transform;
                waveInfoLabel.enabled = true;
                waveInfoLabel.transform.Rotate(new Vector3(0f, 0f, 0f));
                waveInfoLabel.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y + 9f, spawnPoint.position.z);
                TextMeshProUGUI text = waveInfoLabel.GetComponentInChildren<TextMeshProUGUI>();
                //GameObject enemyPreview = waveInfoLabel.GetComponentsInChildren<GameObject>()[1];
                //enemyPreview = Instantiate(wave[waveIndex].waveData.GetEnemy());
                int enemiesLeft = wave[waveIndex].waveData.EnemiesLeft();
                if (enemiesLeft == 0)
                {
                    text.SetText("No enemies left");
                } else if (enemiesLeft == 1)
                {
                    text.SetText("1 enemy left");
                } else
                {
                    text.SetText(enemiesLeft + " enemies left");
                }
                
            } else 
            {
                waveInfoLabel.enabled = false;
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

