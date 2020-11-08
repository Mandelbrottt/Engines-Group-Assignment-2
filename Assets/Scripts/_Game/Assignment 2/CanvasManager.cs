using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CanvasManager : MonoBehaviour
{
    public TMP_Text Objective1;
    public Sprite JumpIcon;
    public Sprite WallrunIcon;
    public Sprite SwapGunIcon;
    public Sprite ShootEnemyIcon;
    public Sprite ShootWeakspotIcon;
    public Sprite IncompleteCheckbox;
    public Sprite CompleteCheckbox;
    public Sprite NextRoomIcon;
    public Image currentIcon;
    public Image currentCheckbox;

    bool isCoroutineRunning = false;
     
    int objectiveIndex = 0;

    string[] objectiveStrings = {"Press [Space] to Jump", "Continue to next room", "Hold [W] and Jump next to a wall to Wallrun",
                                "Continue to next room", "Wallrun the curved wall to reach the exit", "Press 1, 2, 3 or 4 to swap weapons",
                                "Shoot the frog", "Shoot the frog in the head", "Continue to next room",
                                "Play around and escape when satisfied"};

    Sprite[] objectiveImages;

    private void Awake()
    {
        objectiveImages = new Sprite[] {JumpIcon, NextRoomIcon, WallrunIcon, NextRoomIcon, NextRoomIcon, SwapGunIcon, ShootEnemyIcon,
                                       ShootWeakspotIcon, NextRoomIcon, NextRoomIcon};
    }

    // Start is called before the first frame update
    private void Start()
    {
        Objective1.text = objectiveStrings[objectiveIndex];
    }

    

    // Update is called once per frame
    private void Update()
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
            currentIcon.color = alpha;
            yield return new WaitForSeconds(0.1f);
        }

        objectiveIndex++;
        Objective1.text = objectiveStrings[objectiveIndex];
        currentIcon.sprite = objectiveImages[objectiveIndex];

        for (int i = 1; i <= 10; i++)
        {
            alpha.a = i / 10f;
            Objective1.color = alpha;
            currentIcon.color = alpha;
            yield return new WaitForSeconds(0.1f);
        }

        isCoroutineRunning = false;
    }

    //Cycle which icons are being displayed using room trigger volumes
    //and update the objective text with the current objective
}
