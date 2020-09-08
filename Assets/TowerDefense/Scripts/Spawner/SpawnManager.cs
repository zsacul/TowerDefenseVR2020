﻿using System;
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
        GameEvent GameWon;
        [SerializeField]
        LightningCycle lightningCycle;
        private static SpawnManager instance;

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
        [NonSerialized]
        public bool breakOn;

        public Canvas waveInfoLabelPrefab;
        private Canvas waveInfoLabel;
        public Canvas incomingWaveInfoLabelPrefab;
        private Canvas incomingWaveInfoLabel;
        private bool firstUIUpdate;
        private static bool gameOver;

        float breakTime;
        private void Awake()
        {
            instance = this;
        }
        // Update is called once per frame
        void Start()
        {
            gameOver = false;
            waveInfoLabel = Instantiate(waveInfoLabelPrefab);
            incomingWaveInfoLabel = Instantiate(incomingWaveInfoLabelPrefab);
            incomingWaveInfoLabel.gameObject.SetActive(false);
            spawnMagicNumber = 2;
            breakOn = true;
            wave[waveIndex].waveData = ScriptableObject.Instantiate(wave[waveIndex].waveData);
            breakTime = 0;
            firstUIUpdate = true;
            UpdateUI();
        }

        private void Spawn()
        {
            if (spawnedInGroup >= wave[waveIndex].groupSize)
            {
                CancelInvoke("Spawn");
                return;
            }
            GameObject obj = wave[waveIndex].waveData.GetEnemy();
            if (obj == null)//wave end
            {
                CancelInvoke("Spawn");
                CancelInvoke("SpawnLoop");
                waveIndex++;
                if (waveIndex < wave.Length)
                {
                    wave[waveIndex].waveData = ScriptableObject.Instantiate(wave[waveIndex].waveData);
                    InitBreak();
                }
                else
                {
                    InitBreak();
                    Debug.Log("All waves cleared");//TODO
                    gameOver = true;
                    GameWon.Raise();
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
            GameOver();
        }
        private void InitBreak()
        {
            InvokeRepeating("BreakCheck", 0, 1);
        }
        private void BreakCheck()
        {
            //Debug.Log(spawnMagicNumber);
            //Debug.Log(spawnPoints[0].childCount);
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
            breakTime = 0;
            UpdateUI();
            StartCoroutine(lightningCycle.ChangeToDay());
        }

        public void EndBreak()
        {
            if (breakOn && !gameOver)
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
            else if (!gameOver)
            {
                UpdateUI();
            }
        }
        public static void StartWave()
        {
            if (!gameOver)
            {
                instance.EndBreak();
            }
        }

        IEnumerator LateStartUpdateUI(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            Transform spawnPoint = spawnPoints[0].transform;
            waveInfoLabel.enabled = false;
            incomingWaveInfoLabel.enabled = true;
            incomingWaveInfoLabel.transform.Rotate(new Vector3(0f, 0f, 0f));
            incomingWaveInfoLabel.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y + 25f, spawnPoint.position.z);
        }

        private void Update()
        {
            if ((Input.GetKeyDown(KeyCode.P) /*|| Input.GetKeyDown(KeyCode.JoystickButton0)*/ || breakTime > breakDuration)
                && !gameOver)
            {
                EndBreak();
            }

            if (breakOn && !gameOver)
            {
                breakTime += Time.deltaTime;
            }
        }

        public void ChangeWaveInfoLabelStatusTo(bool status)
        {
            waveInfoLabel.gameObject.SetActive(status);
        }

        public void ChangeIncomingWaveInfoLabelStatusTo(bool status)
        {
            incomingWaveInfoLabel.gameObject.SetActive(status);
        }

        private void UpdateUI()
        {
            Transform spawnPoint = spawnPoints[0].transform;
            if (!breakOn)
            {
                incomingWaveInfoLabel.enabled = false;
                waveInfoLabel.enabled = true;
                waveInfoLabel.transform.Rotate(new Vector3(0f, 0f, 0f));
                waveInfoLabel.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y + 9f, spawnPoint.position.z);
                incomingWaveInfoLabel.GetComponentInChildren<TextMeshProUGUI>().SetText("Next wave:");
                TextMeshProUGUI text = waveInfoLabel.GetComponentInChildren<TextMeshProUGUI>();
                int enemiesLeft = wave[waveIndex].waveData.EnemiesLeft();
                if (enemiesLeft == 0)
                {
                    text.SetText("No enemies left");
                }
                else if (enemiesLeft == 1)
                {
                    text.SetText("1 enemy left");
                }
                else
                {
                    text.SetText(enemiesLeft + " enemies left");
                }

            }
            else if(!gameOver)
            {
                waveInfoLabel.enabled = false;
                incomingWaveInfoLabel.enabled = true;
                incomingWaveInfoLabel.transform.Rotate(new Vector3(0f, 0f, 0f));
                incomingWaveInfoLabel.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y + 25f, spawnPoint.position.z);
                EnemyCount[] incomingEnemies = wave[waveIndex].waveData.GetWaveInfo();

                int fireEnemies = 0;
                int windEnemies = 0;
                int iceEnemies = 0;
                int lightningEnemies = 0;
                int stoneEnemies = 0;
                int poisonEnemies = 0;

                foreach (EnemyCount enemy in incomingEnemies)
                {
                    ElementType enemyType = enemy.enemyPrefab.GetComponent<EnemyHPManager>().GetElementType();
                    switch (enemyType)
                    {
                        case ElementType.ice:
                            iceEnemies += enemy.count;
                            break;
                        case ElementType.fire:
                            fireEnemies += enemy.count;
                            break;
                        case ElementType.electricity:
                            lightningEnemies += enemy.count;
                            break;
                        case ElementType.wind:
                            windEnemies += enemy.count;
                            break;
                        case ElementType.stone:
                            stoneEnemies += enemy.count;
                            break;
                        case ElementType.poison:
                            poisonEnemies += enemy.count;
                            break;
                    }
                }

                incomingWaveInfoLabel.GetComponentInChildren<TextMeshProUGUI>().text ="Next wave:";
                if (lightningEnemies != 0)
                    SetIncomingWaveText(0, lightningEnemies);
                if (iceEnemies != 0)
                    SetIncomingWaveText(1, iceEnemies);
                if (fireEnemies != 0)
                    SetIncomingWaveText(2, fireEnemies);
                if (windEnemies != 0)
                    SetIncomingWaveText(3, windEnemies);
                if (stoneEnemies != 0)
                    SetIncomingWaveText(4, stoneEnemies);
                if (poisonEnemies != 0)
                    SetIncomingWaveText(5, poisonEnemies);
            }

            if (firstUIUpdate)
            {
                StartCoroutine(LateStartUpdateUI(0.1f));
                firstUIUpdate = false;
            }
        }

        private void SetIncomingWaveText(int indexOfSpriteAsset, int numberOfEnemies)
        {
            String enemiesText = (numberOfEnemies > 1) ? "enemies" : "enemy";
            incomingWaveInfoLabel.GetComponentInChildren<TextMeshProUGUI>().text += $"\n{numberOfEnemies}   <sprite={indexOfSpriteAsset}><size=70%>{enemiesText}<size=100%>";
        }

        public void GameOver()
        {
            gameOver = true;
            Debug.Log("GameOver()");
            // TODO: display GameOver info
        }

        public void TemporaryFixForIncomingWave()
        {
            ChangeIncomingWaveInfoLabelStatusTo(true);
            incomingWaveInfoLabel.enabled = false;
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

