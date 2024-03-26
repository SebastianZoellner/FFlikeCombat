using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIMissionBriefing : MonoBehaviour
{
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
        Time.timeScale = 1;
    }

    private void LevelSetup_StartMissionBriefing(LevelSO level)
    {
        Time.timeScale = 0;
        screen.SetActive(true);
        missionText.text = level.missionBriefing;
        if (level.missionVisual)
            missionImage.sprite = level.missionVisual;

    }
}
