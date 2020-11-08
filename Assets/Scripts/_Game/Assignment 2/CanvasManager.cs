using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CanvasManager : MonoBehaviour
{
    public TMP_Text Objective1;
    public Image JumpIcon;
    public Image WallrunIcon;
    public Image SwapGunIcon;
    public Image ShootEnemyIcon;
    public Image ShootWeakspotIcon;
    public Image IncompleteCheckbox;
    public Image CompleteCheckbox;

    bool isCoroutineRunning = false;
     
    int objectiveIndex = 0;

    string[] objectiveStrings = {"Press [Space] to Jump", "Continue to next room", "Hold [W] and Jump next to a wall to Wallrun",
                                "Continue to next room", "Wallrun the corner to reach the exit", "Press 1, 2, 3 or 4 to swap weapons",
                                "Shoot the frog", "Shoot the frog in the head", "Continue to next room",
                                "Play around and escape when satisfied"};


    // Start is called before the first frame update
    void Start()
    {
        Objective1.text = objectiveStrings[objectiveIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && !isCoroutineRunning)
        {
            StartCoroutine(FadeToNextText());
        }
    }

    private IEnumerator FadeToNextText()
    {
        isCoroutineRunning = true;
        Color alpha = Objective1.color;

        for (int i = 9; i >= 0; i--)
        {
            alpha.a = i / 10f;
            Objective1.color = alpha;
            yield return new WaitForSeconds(0.1f);
        }

        Objective1.text = objectiveStrings[++objectiveIndex];

        for (int i = 0; i <= 9; i++)
        {
            alpha.a = i / 10f;
            Objective1.color = alpha;
            yield return new WaitForSeconds(0.1f);
        }

        isCoroutineRunning = false;
    }

    //Cycle which icons are being displayed using room trigger volumes
    //and update the objective text with the current objective
}
