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

        wizard1.OnWizardBossDefaet += OnWizard1Defeat;

        undeadExecutioner1.OnUndeadBossDefeat += OnUndead1Defeat;

        wizard2.OnWizardBossDefaet += OnLastAreaBossDefeat;
        undeadExecutioner2.OnUndeadBossDefeat += OnLastAreaBossDefeat;

    }

    private void OnWizard1Defeat()
    {
        prompt.PromptMessage("Good Job!");
        StartCoroutine(MoveToNextArea(new Vector2(-80, 13)));
        StartCoroutine(UnlockDoubleJump());
    }

    IEnumerator  UnlockDoubleJump()
    {
        yield return new WaitForSeconds(6);
        player.UnlockDoubleJump(true);
        prompt.PromptMessage("Ability: Double Jump is unlocked");
    }

    private void OnUndead1Defeat() 
    {
        prompt.PromptMessage("Keep Fighting!");
        StartCoroutine(MoveToNextArea(new Vector2(-106, 13)));
    }

    IEnumerator MoveToNextArea(Vector2 position)
    {
        yield return new WaitForSeconds(1);
        prompt.PromptMessage("Moving to next area in 5 seconds...");
        yield return new WaitForSeconds(5);
        player.MoveToNewPosition(position);
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
