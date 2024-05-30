using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionSelectButton : MonoBehaviour
{
    [SerializeField] TMP_Text missionName;
    [SerializeField] Image icon;
    [SerializeField] Image frame;

    private bool isSelected = false;

    private LevelSO mission;
    private StartScreenManager startScreen;

    private void OnDisable()
    {
        if(startScreen)
            startScreen.OnMissionSelected -= StartScreen_OnMissionSelected;
    }

    public void Setup(LevelSO mission, StartScreenManager startScreen)
    {
        this.mission = mission;
        this.startScreen = startScreen;

        this.startScreen.OnMissionSelected += StartScreen_OnMissionSelected;

        missionName.text = mission.LevelName;
        icon.sprite = mission.missionIcon;
    }

    public void MissionButtonPressed()
    {
        startScreen.PressMissionSelectButton(mission);
    }

    private void StartScreen_OnMissionSelected(LevelSO selectedLevel)
    {
        if(selectedLevel!=mission)
        {

            frame.color = Color.white;
            isSelected = false;
        }
        else
        {
            frame.color = Color.red;
            isSelected = true;
        }
    }
}
