using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    public event System.Action RoundChange;

    // enemy increase between rounds and waves
    const float roundMulitplier = 0.3f;
    const float waveMulitplier = 0.25f;

    int startingRound = 0;
    int round;
    int totalRounds = 25;
    int walkEnemies = 3;
    int sprintEnemies = 2;

    Round[] walk;
    Round[] sprint;

    bool roundComplete;
    bool walkComplete;
    bool sprintComplete;
    bool waveSetWalk;
    bool waveSetSprint;

    public void Start() {
        walk = InitializeArray<Round>(totalRounds);
        sprint = InitializeArray<Round>(totalRounds);
        SetRounds(walk);
        SetRounds(sprint);
        round = startingRound;
    }

    void Update() {
        if (waveSetWalk && waveSetSprint) {
            roundComplete = false;
            waveSetWalk = false;
            waveSetSprint = false;
        }
    }

    // sets enemies for each wave and waves for each round
    void SetRounds(Round[] round) {
        for (int i = 0; i < totalRounds; i++) {

            float modifier = (i+1) * roundMulitplier;

            // sets number of waves per round
            // waves increase throughout game
            //
            // round        waves
            // 1            1
            // 2-3          2
            // 4-7          3
            // 8-12         4
            // 12+          5
            // POSSIBLY ADD MORE WAVES FOR HIGHER ROUNDS
            // WILL REVISIT DURING BALANCING

            if (i == 0) {
                round[i].numWaves = 1;
            }
            else if (i >= 1 && i <= 2) {
                round[i].numWaves = 2;
            }
            else if (i >= 3 && i <= 6) {
                round[i].numWaves = 3;
            }
            else if (i >= 7 && i <= 11) {
                round[i].numWaves = 4;
            }
            else {
                round[i].numWaves = 5;
            }

            round[i]._waves = InitializeArray<Spawner.Wave>(round[i].numWaves);

            // sets the number of enemies and time between spawns for each wave
            for (int j = 0; j < round[i].numWaves; j++) {

                modifier += (j+1) * waveMulitplier;
                switch (j) {

                    case 0:
                        if (round.Equals(walk)) round[i]._waves[j].enemyCount = (int)Mathf.Ceil(walkEnemies * modifier);
                        if (round.Equals(sprint)) round[i]._waves[j].enemyCount = (int)Mathf.Ceil(sprintEnemies * modifier);
                        round[i]._waves[j].timeBetweenSpawns = 4;
                        break;

                    case 1:
                        if (round.Equals(walk)) round[i]._waves[j].enemyCount = (int)Mathf.Ceil(walkEnemies * modifier);
                        if (round.Equals(sprint)) round[i]._waves[j].enemyCount = (int)Mathf.Ceil(sprintEnemies * modifier);
                        round[i]._waves[j].timeBetweenSpawns = 2;
                        break;

                    case 2:
                        if (round.Equals(walk)) round[i]._waves[j].enemyCount = (int)Mathf.Ceil(walkEnemies * modifier);
                        if (round.Equals(sprint)) round[i]._waves[j].enemyCount = (int)Mathf.Ceil(sprintEnemies * modifier);
                        round[i]._waves[j].timeBetweenSpawns = 2;
                        break;

                    case 3:
                        if (round.Equals(walk)) round[i]._waves[j].enemyCount = (int)Mathf.Ceil(walkEnemies * modifier);
                        if (round.Equals(sprint)) round[i]._waves[j].enemyCount = (int)Mathf.Ceil(sprintEnemies * modifier);
                        round[i]._waves[j].timeBetweenSpawns = 1;
                        break;

                    default:
                        if (round.Equals(walk)) round[i]._waves[j].enemyCount = (int)Mathf.Ceil(walkEnemies * modifier);
                        if (round.Equals(sprint)) round[i]._waves[j].enemyCount = (int)Mathf.Ceil(sprintEnemies * modifier);
                        round[i]._waves[j].timeBetweenSpawns = .5f;
                        break;
                }
            }
        }
    }

    public void RoundEnd(int type) {
        if (type == 1) walkComplete = true;
        else if (type == 2) sprintComplete = true;

        Debug.Log("RoundEnd function " + type);
        Debug.Log(walkComplete + " " + type);
        Debug.Log(sprintComplete + " " + type);

        if (walkComplete && sprintComplete) {
            Debug.Log("RoundComplete function " + type);
            round++;
            roundComplete = true;
            walkComplete = false;
            sprintComplete = false;
            RoundChange();
        }
    }

    public int GetRoundNum() {
        return round + 1;
    }

    public bool GetRoundComplete() {
        return roundComplete;
    }

    public Spawner.Wave[] GetWaves(int type) {
        if (type == 1) {
            waveSetWalk = true;
            return walk[round]._waves;
        }
        else if (type == 2) {
            waveSetSprint = true;
            return sprint[round]._waves;
        }
        else return null;
    }

    // initializes objects within an array of objects
    T[] InitializeArray<T>(int length) where T : new() {
        T[] array = new T[length];
        for (int i = 0; i < length; ++i) {
            array[i] = new T();
        }

        return array;
    }

    public class Round
    {
        public int numWaves;
        public Spawner.Wave[] _waves;
    }
}
