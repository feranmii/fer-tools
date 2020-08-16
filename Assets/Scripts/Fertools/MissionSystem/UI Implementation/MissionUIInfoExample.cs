using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionUIInfoExample : MonoBehaviour
{
    public TextMeshProUGUI missionNameText;
    public TextMeshProUGUI missionDescText;
    public Image status;


    private MissionObjective _missionObjective;

    public void Init(MissionObjective objective)
    {
        objective.onObjectiveStatusChanged += ProcessObjectiveStatusChanged;

        _missionObjective = objective;

        Refresh();
    }

    private void ProcessObjectiveStatusChanged(MissionObjective objective)
    {
        print("Changes don happen");
        Refresh();
    }

    private void Refresh()
    {
        //Set Text and Image
        missionNameText.text = _missionObjective.objectiveName;
        missionDescText.text = _missionObjective.objectiveDescription;

        //Set Image Status color and Text Color
        switch (_missionObjective.objectiveStatus)
        {
            case MissionObjective.ObjectiveStatus.IDLE:
                missionNameText.DOColor(Colors.Gray, .5f);
                status.gameObject.SetActive(false);
                break;
            case MissionObjective.ObjectiveStatus.FAILED:
                missionNameText.DOColor(Colors.IndianRed, .5f);
                status.gameObject.SetActive(true);
                status.DOColor(Colors.IndianRed, .5f);

                break;
            case MissionObjective.ObjectiveStatus.COMPLETED:
                missionNameText.DOColor(Colors.Gray, .5f);
                status.gameObject.SetActive(true);
                status.DOColor(Colors.ForestGreen, .5f);

                break;
            case MissionObjective.ObjectiveStatus.INPROGRESS:
                missionNameText.DOColor(Colors.White, .5f);

                status.gameObject.SetActive(true);
                status.DOColor(Colors.LightYellow, .5f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}