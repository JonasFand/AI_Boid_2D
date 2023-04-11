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
        
        if (CheckPoint)
        {
            TowardsCheckPoint();
        }
        timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        foreach (var entity in SwarmManager.Instance.EntityList)
        {
            CheckRegion(entity.collider.Distance(Region).distance,entity);
        }

        foreach (var entity in SwarmManager.Instance.EntityList)
        {
            CheckEvasion(Physics2D.OverlapCircleAll(entity.transform.position, entity.EvasionRadius, LayerMask).ToList(), entity);
        }
    }

    private void TowardsCheckPoint()
    {
        foreach (var entity in SwarmManager.Instance.EntityList)
        {
            var tempVector = CheckPoint.position - entity.transform.position;
            entity.CheckPointVector = tempVector.normalized * CheckPointMultiplier;
        }
    }

    private void CheckEvasion(List<Collider2D> colliders, Movement entity)
    {
        Vector2 tempVector = Vector2.zero;
        int i = 0;
        if (colliders.Count == 0)
        {
            entity.UnitEvasionVector = Vector2.zero;
        }
        else
        {
            foreach (var col in colliders.Where(collider => collider != entity.collider))
            {
                i++;
                //tempVector += (Vector2)collider.transform.position - (Vector2)entity.transform.position;
                tempVector += ((Vector2)col.transform.position - (Vector2)entity.transform.position).normalized*(EvasionMultiplier-entity.collider.Distance(col).distance);
            }
            tempVector /= i;
            entity.UnitEvasionVector = tempVector;
        }
    }

    private void CheckRegion(float distance, Movement entity)
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

    static void ChangeDirection(Movement entity)
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
