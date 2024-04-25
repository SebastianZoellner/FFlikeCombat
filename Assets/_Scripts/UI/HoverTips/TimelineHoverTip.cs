using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineHoverTip : HoverTip
{
    private CharacterInitiative character;


    public void SetCharacter(CharacterInitiative character) => this.character = character;
    
    protected override string GetTip()
    {    
        return character.GetTimelineTip();     
    }
}
