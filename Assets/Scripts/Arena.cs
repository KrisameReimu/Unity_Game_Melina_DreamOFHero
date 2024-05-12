using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private WizardBoss wizard1;
    
    [SerializeField]
    private UndeadExecutionerBoss undeadExecutioner1;

    [SerializeField]
    private WizardBoss wizard2;
    [SerializeField]
    private UndeadExecutionerBoss undeadExecutioner2;

    private int lastBossCounter = 2;

    [SerializeField]
    private MessagePrompt prompt;

    private void Start()
    {
        player.UnlockDoubleJump(true);

        wizard1.OnWizardBossDefaet += OnWizard1Defeat;

        undeadExecutioner1.OnUndeadBossDefeat += OnUndead1Defeat;

        wizard2.OnWizardBossDefaet += OnLastAreaBossDefeat;
        undeadExecutioner2.OnUndeadBossDefeat += OnLastAreaBossDefeat;


        prompt.PromptMessage("Ability: Double Jump is unlocked");
    }

    private void OnWizard1Defeat() 
    {
        player.MoveToNewPosition(new Vector2(-80, 13));
        prompt.PromptMessage("Good Job!");
    }
    private void OnUndead1Defeat() 
    {
        player.MoveToNewPosition(new Vector2(-107, 13));
        prompt.PromptMessage("Keep Fighting!");
    }

    

    private void OnLastAreaBossDefeat()
    {
        lastBossCounter--;
        if (lastBossCounter == 1)
        {
            prompt.PromptMessage("Almost there!");
        }
        else if(lastBossCounter == 0)
        {
            prompt.PromptMessage("Congratulation!");
        }
        else
        {
            Debug.Log("Arena: Unexpected Error");
        }
    }
}
