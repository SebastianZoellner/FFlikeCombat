using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions : MonoBehaviour
{
    [SerializeField] private float gameSpeed=1;

    private void Update()
    {
        Time.timeScale = gameSpeed;
    }
}
