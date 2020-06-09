using System;
using UnityEngine;

namespace SpawnManaging
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveObject", order = 1)]
    public class WaveObject : ScriptableObject
    {
        [SerializeField]
        private EnemyCount[] enemies;
        [SerializeField]
        [Tooltip("random - spawn random available enemy\n" +
            "top - spawn first available enemy from top to bottom" +
            "bottom - spawn first available enemy from bottom to top" +
            "cycleTop - spawn first available enemy from top to bottom, one from each group" +
            "cycleBottom - spawn first available enemy from bottom to top, one from each group")]
        private SpawnRule pattern;
        [SerializeField]
        [Tooltip("ignores enemies count field")]
        private bool ignoreLimits;
        [SerializeField]
        [Tooltip("-1 if unlimited")]
        private int maxSpawns;
        [SerializeField]
        [Tooltip("tries to avoid spawning same enemy")]
        private bool avoidIdent;

        private int index;
        private int lastIndex;
        private int indexStep;
        private bool initiated;
        private int timesSpawned;

        private  void Start()
        {
            index = pattern == SpawnRule.bottom ?
                    enemies.Length - 1 : 0;
            initiated = true;
        }
        /// <summary>
        /// Returns enemy object based on choosen spawn rules
        /// </summary>
        /// <returns></returns>
        public GameObject GetEnemy()
        {
            indexStep = 0;
            if(!initiated)
            {
                Start();
            }
            else
            {
                NextIndex();
            }
            if(!ignoreLimits && enemies[index].count == 0)
            {
                return null;
            }
            if(maxSpawns >= 0)
            {
                if(timesSpawned > maxSpawns)
                {
                    return null;
                }
                timesSpawned++;
            }
            enemies[index].count--;
            return enemies[index].enemyPrefab;
        }
        private void NextIndex()
        {
            lastIndex = index;
            switch(pattern)
            {
                case SpawnRule.top:
                    if(ignoreLimits)
                    {
                        index = index == enemies.Length - 1 ?
                            0 : index + 1;
                    }
                    else
                    {
                        if(avoidIdent)
                        {
                            index = index == enemies.Length - 1?
                            0 : index + 1;
                        }
                        int startt = index;
                        while (enemies[index].count == 0)
                        {
                            index = index == enemies.Length - 1 ?
                                0 : index + 1;
                            if (index == startt)
                            {
                                return;
                            }
                        }
                    }
                    break;
                case SpawnRule.bottom:
                    if(ignoreLimits)
                    {
                        index = index == 0 ?
                            enemies.Length - 1 : index - 1;
                    }
                    else
                    {
                        if (avoidIdent)
                        {
                            index = index == 0 ?
                            enemies.Length - 1 : index - 1;
                        }
                        int startt = index;
                        while (enemies[index].count == 0)
                        {
                            index = index == 0 ?
                                enemies.Length - 1: index - 1;
                            if (index == startt)
                            {
                                return;
                            }
                        }
                    }
                    break;
                case SpawnRule.random:
                    index = UnityEngine.Random.Range(0, enemies.Length);
                    int start = index;
                    while(enemies[index].count == 0)
                    {
                        index = index == enemies.Length - 1?
                            0 : index + 1;
                        if(index == start)
                        {
                            return;
                        }
                    }
                    break;
            }
        }

        public int EnemiesLeft()
        {
            int totalEnemies = 0;
            for(int i = 0; i < enemies.Length; i++)
            {
                totalEnemies += enemies[i].count;
            }
            return totalEnemies;
        }
    }

    [Serializable]
    public class EnemyCount
    {
        public GameObject enemyPrefab;
        public int count;
    }
    public enum SpawnRule
    {
        random,
        top,
        bottom,
    }
}

