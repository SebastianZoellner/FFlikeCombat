using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Level", menuName = "Game Elements/Levels")]

public class LevelSO : ScriptableObject
{
    public string LevelName;
    [PreviewField(75)]
    public Sprite missionIcon;
    [HorizontalGroup("Display", 300)]
    [TextArea(5,15)]
    public string missionBriefing;
    [VerticalGroup("Display/Image")]
    [PreviewField(125)]
    public Sprite missionVisual;
    public int numberOfHeroes;

    public string sceneName;
    [SerializeField] private Stage[] stages;

    public List<GameObject> SpawnWave(int stage, int round, SpawnPointController spawnPointController) => stages[stage].waveArray[round].SpawnWave(spawnPointController);
    public int GetNumberOfWaves(int stage)=> stages[stage].waveArray.Length;
    public int GetNumberOfStages() => stages.Length;
    public AudioClip GetAmbience(int stage) => stages[stage].ambience;
    public WaveMusicSO GetMusic(int stage) => stages[stage].music;
    public Stage GetStage(int stageIndex) => stages[stageIndex];
    
}

[System.Serializable]
public struct Stage
{
    public EnemyWaveSO[] waveArray;
    public AudioClip ambience;
    public WaveMusicSO music;
    public int timeLimit;
    public float timeLimitPenalty;
}