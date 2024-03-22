using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScreenButton : PlayerScreenButton
{
    [SerializeField] AttackChoices attackChoice;

    //needs to be logic as to whether an attack should be shown based on progression

    public override void ButtonClicked()
    {
        //if (!CheckAttackAvailable()) return;

        if (attackChoice == AttackChoices.JUMP)
        {
            FindObjectOfType<ChooseEnemyHandler>().EnableEnemyHandler(attackChoice);
        }
        else if (attackChoice == AttackChoices.SLASH)
        {
            FindObjectOfType<ChooseEnemyHandler>().EnableEnemyHandler(attackChoice);
        }
    }

    public override bool CheckValidButton()
    {
        //for example: not enough energy?
        return true;
    }
}
