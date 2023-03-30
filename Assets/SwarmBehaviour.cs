using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SwarmBehaviour : MonoBehaviour
{
    public BoxCollider2D Region;
    public Transform CheckPoint;
    public LayerMask LayerMask;
    public float EvasionMultiplier = 1;
    public float CheckPointMultiplier = 1;
    public float ForceDivider = 10;
    private float timer = 0;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (timer > 0.01f)
        {
            timer = 0;
            foreach (var entity in SwarmManager.Instance.EntityList)
            {
                ChangeDirection(entity);
            }
        }

        foreach (var entity in SwarmManager.Instance.EntityList)
        {
            CheckRegion(entity.collider.Distance(Region).distance,entity);
        }

        foreach (var entity in SwarmManager.Instance.EntityList)
        {
            CheckEvasion(Physics2D.OverlapCircleAll(entity.transform.position, entity.EvasionRadius, LayerMask).ToList(), entity);
        }

        if (CheckPoint)
        {
            TowardsCheckPoint();
        }
        timer += Time.deltaTime;
    }

    private void TowardsCheckPoint()
    {
        foreach (var entity in SwarmManager.Instance.EntityList)
        {
            var tempVector = CheckPoint.position - entity.transform.position;
            entity.CheckPointVector = tempVector.normalized * CheckPointMultiplier;
        }
    }
    
    void CheckEvasion(List<Collider2D> colliders, Movement entity)
    {
        Vector2 tempVector = Vector2.zero;
        int i = 0;
        if (colliders.Count == 0)
        {
            entity.UnitEvasionVector = Vector2.zero;
        }
        else
        {
            foreach (var collider in colliders)
            {
                i++;
                tempVector += (Vector2)collider.transform.position - (Vector2)entity.transform.position;
            }
            tempVector /= i;
            entity.UnitEvasionVector = tempVector.normalized*EvasionMultiplier;
        }
    }
    
    void CheckRegion(float distance, Movement entity)
    {
        if (distance > 0)
        {
            entity.ForceVector = (Region.transform.position - entity.transform.position.normalized);
            entity.ForceVector *= distance / ForceDivider;
        }
        else
        {
            entity.ForceVector = Vector2.zero;
        }
    }

    void ChangeDirection(Movement entity)
    {
        Vector2 tempVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized +
                             entity.NewDirection;
        entity.NewDirection = tempVector.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(CheckPoint.transform.position,2f);
    }
}
