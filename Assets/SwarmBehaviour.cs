using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SwarmBehaviour : MonoBehaviour
{
    public BoxCollider2D Region;
    public LayerMask LayerMask;
    public float ForceDivider = 10;
    private float timer = 0;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (timer>0.1f)
        {
            timer = 0;
             foreach (var entity in SwarmManager.Instance.EntityList)
             {
                 Vector2 tempVector2 = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized + entity.NewDirection;
                 entity.NewDirection = tempVector2.normalized;
                 //entity.NewDirection.Normalize();
             }
        }
        float temp;
        foreach (var entity in SwarmManager.Instance.EntityList)
        {
            temp = entity.collider.Distance(Region).distance;
            if (temp>0)
            {
                entity.ForceVector = (Region.transform.position - entity.transform.position.normalized);
                entity.ForceVector *= temp/ForceDivider;
            }
        }

        foreach (var entity in SwarmManager.Instance.EntityList)
        {
            List<Collider2D> a = Physics2D.OverlapCircleAll(entity.transform.position, entity.EvasionRadius, LayerMask).ToList();
            Vector2 tempvector = Vector2.zero;
            int i = 0;
            if (a.Count == 0)
            {
                entity.UnitEvasionVector = Vector2.zero;
            }
            else
            {
                foreach (var collider in a)
                {
                    i++;
                    tempvector += (Vector2)collider.transform.position - (Vector2)entity.transform.position;
                }
                tempvector /= i;
                entity.UnitEvasionVector = tempvector.normalized;
            }
            

            
        }

        timer += Time.deltaTime;

    }


}
