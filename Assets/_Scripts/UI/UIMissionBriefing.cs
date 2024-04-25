using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIMissionBriefing : MonoBehaviour
{
    public static event Action<bool> OnBriefingSet;
    [SerializeField] GameObject screen;
    [SerializeField] TextMeshProUGUI missionText;
    [SerializeField] Image missionImage;

    private void OnEnable()
    {
        LevelSetup.StartMissionBriefing += LevelSetup_StartMissionBriefing;
    }

    private void OnDisable()
    {
        LevelSetup.StartMissionBriefing -= LevelSetup_StartMissionBriefing;
    }

    public void LetsGo()
    {
        screen.SetActive(false);
        OnBriefingSet.Invoke(false);
        //Time.timeScale = 1;// Maybe instead halt the ActionSequencer
    }

    private void LevelSetup_StartMissionBriefing(LevelSO level)
    {
        OnBriefingSet.Invoke(true);
        //Time.timeScale = 0;
        screen.SetActive(true);
        missionText.text = level.missionBriefing;
        if (level.missionVisual)
            missionImage.sprite = level.missionVisual;

    }
}
