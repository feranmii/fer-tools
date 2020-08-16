using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionUIExample : MonoBehaviour
{
    public RectTransform missionsParent;

    public MissionUIInfoExample missionPrefab;


    public GameObject missionCompleteLabel;

    private MissionController _missionController;

    private void Start()
    {
        missionCompleteLabel.SetActive(false);
        _missionController = MissionController.Instance;
        InitMissions();
        _missionController.onMissionComplete += ProcessMissionComplete;
        _missionController.onMissionFailed += ProcessMissionFailed;
    }
    

    private void ProcessMissionComplete()
    {
        missionsParent.gameObject.SetActive(false);
        missionCompleteLabel.GetComponent<TextMeshProUGUI>().text = "MISSION COMPLETE";
        missionCompleteLabel.GetComponent<TextMeshProUGUI>().color = Colors.ForestGreen;
        missionCompleteLabel.SetActive(true);
    }

    private void ProcessMissionFailed()
    {
        missionsParent.gameObject.SetActive(false);
        missionCompleteLabel.GetComponent<TextMeshProUGUI>().text = "MISSION FAILED";
        missionCompleteLabel.GetComponent<TextMeshProUGUI>().color = Colors.IndianRed;
        missionCompleteLabel.SetActive(true);  
    }

    private void InitMissions()
    {
        _missionController.missionObjectivesList.ForEach(objective =>
        {
            var mp = Instantiate(missionPrefab, missionsParent);
            mp.Init(objective);
        });
    }
}