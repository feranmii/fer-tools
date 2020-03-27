using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fertools.Health
{

    public class HealthManager : MonoBehaviour
    {
        public HealthVariable health;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                health.Reduce(10);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                health.Add(10);
            }
        }
    }
}