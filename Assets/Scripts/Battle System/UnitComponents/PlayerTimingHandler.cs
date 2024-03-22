using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimingHandler : TimingHandler
{
    private void Update()
    {
        if (inputAllowed && !playerPressedButton)
        {
            if (GetKeyPressed())
            {
                CheckCorrectTiming();
            }
        }
    }

    public override bool GetKeyPressed()
    {
        return BattleInputManager.Instance.GetMainKeyPressed();
    }
}
