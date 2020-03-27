using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementNotificationItem : MonoBehaviour
{
    [BoxGroup("Image")]
    public Image backgroundImage;
    [BoxGroup("Image")]

    public Image backgroundLocked;
    [BoxGroup("Image")]
    public Image icon;
    
    
    [BoxGroup("Text")]
    
    public TextMeshProUGUI title;
    
    [BoxGroup("Text")]

    public TextMeshProUGUI description;
}
