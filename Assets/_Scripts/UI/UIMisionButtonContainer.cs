using UnityEngine;

public class UIMisionButtonContainer : MonoBehaviour
{
    [SerializeField] private StartScreenManager startScreen;
    [SerializeField] GameObject missionButtonContainer;
    [SerializeField] GameObject missionButtonObject;

    private void Start()
    {
        LevelSO[] availableMissions = startScreen.GetAvailableMissions();

        foreach (LevelSO cha in availableMissions)
        {
            GameObject newButton = Instantiate(missionButtonObject, missionButtonContainer.transform);

            newButton.GetComponent<UIMissionSelectButton>().Setup(cha, startScreen);
        }
    }
}
