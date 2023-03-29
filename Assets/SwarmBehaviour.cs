using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SwarmBehaviour : MonoBehaviour
{
    private float timer = 0;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (timer>1)
        {
            timer = 0;
             foreach (var entity in SwarmManager.Instance.EntityList)
             {
                 entity.ForceVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                 entity.ForceVector.Normalize();
             }
        }

        timer += Time.deltaTime;

    }
}
