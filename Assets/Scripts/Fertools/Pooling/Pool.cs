using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fertools.Pooling
{

    public class Pool : MonoBehaviour
    {
        private static Dictionary<PooledMonobehaviour, Pool> pools_dict = new Dictionary<PooledMonobehaviour, Pool>();
        private Queue<PooledMonobehaviour> objects = new Queue<PooledMonobehaviour>();
        private List<PooledMonobehaviour> disabledObjects = new List<PooledMonobehaviour>();

        private PooledMonobehaviour prefab;


        public static Pool GetPool(PooledMonobehaviour prefab)
        {
            if (pools_dict.ContainsKey(prefab))
                return pools_dict[prefab];
            
            var pool = new GameObject("Pool-" + (prefab).name).AddComponent<Pool>();
            pool.prefab = prefab;
            pool.GrowPool();
            pools_dict.Add(prefab, pool);
            return pool;
        }
        
        
        public T Get<T>() where T :  PooledMonobehaviour
        {
            if (objects.Count == 0)
                GrowPool();

            var pooledObject = objects.Dequeue();
            return pooledObject as T;
        }

        public void GrowPool()
        {
            for (int i = 0; i < prefab.InitialPoolSize; i++)
            {
                var pooledObject = Instantiate(this.prefab);
                pooledObject.gameObject.name += " " + i;

                pooledObject.OnDestroyEvent += () => AddObjectToAvailable(pooledObject);
                pooledObject.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            MakeDisabledObjectsChildren();
        }

        private void MakeDisabledObjectsChildren()
        {
            if (disabledObjects.Count > 0)
            {
                foreach (var pooledObject in disabledObjects)
                {
                    if (pooledObject.gameObject.activeInHierarchy == false)
                    {
                        pooledObject.transform.SetParent(transform);
                        
                    }
                }
                
                disabledObjects.Clear();
            }
        }
        private void AddObjectToAvailable(PooledMonobehaviour pooledObject)
        {
            disabledObjects.Add(pooledObject);
            objects.Enqueue(pooledObject);
        }
    }
    

}