using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fertools.Pooling
{

    public class PooledMonobehaviour : MonoBehaviour
    {
        [SerializeField] private int initialPoolSize = 100;
        
        

        public int InitialPoolSize => initialPoolSize;

        public event Action OnDestroyEvent;

        protected void OnDisable()
        {
            if (OnDestroyEvent != null)
                OnDestroyEvent();
            
            
        }

        public T Get<T>(bool enable = true) where T : PooledMonobehaviour
        {
            var pool = Pool.GetPool(this);
            var pooledObject = pool.Get<T>();

            if (enable)
            {
                pooledObject.gameObject.SetActive(true);
            }

            return pooledObject;
        }

        public T Get<T>(Transform parent, bool resetTransform = false) where T : PooledMonobehaviour
        {
            var pooledObject = Get<T>(true);
            pooledObject.transform.SetParent(parent);

            if (resetTransform)
            {
                pooledObject.transform.localPosition = Vector3.zero;
                pooledObject.transform.localRotation = Quaternion.identity;
                
            }

            return pooledObject;
        }

        public T Get<T>(Transform parent, Vector3 relativePosition, Quaternion relativeRotation)
            where T : PooledMonobehaviour
        {
            var pooledObject = Get<T>(true);
            pooledObject.transform.SetParent(parent);
            pooledObject.transform.localPosition = relativePosition;
            pooledObject.transform.localRotation = relativeRotation;
            return pooledObject;
        }
        
        
    }
}
