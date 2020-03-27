using System.Collections;
using System.Collections.Generic;
using Fertools.Utils;
using NaughtyAttributes;
using UnityEngine;

namespace Fertools.UI.Fade
{
    public class AdvancedFade : MonoBehaviour
    {
        //Public Fields
        [BoxGroup("Fading Properties")]
        public float fadedAlpha;
        [BoxGroup("Fading Properties")]
        public float fullAlpha;

        [BoxGroup("Time Properties")] public float defaultDuration = .2f;

        [BoxGroup("Interaction")] public bool shouldBlockInteraction = false;

    }
}