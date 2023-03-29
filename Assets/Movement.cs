using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SteeringBehaviour
{
    RandomSteering,
    AttractorSteering,
}
public class Movement : MonoBehaviour
{
    [Range(0,5)]
    public float Speed = 30;
    public Vector2 Direction;
    public Vector2 NewDirection;
    private BoxCollider2D region;
    [SerializeField]private PolygonCollider2D collider;
    [SerializeField]private float timer = 0;
    Quaternion startRotation;
    Quaternion newRotation;
    public SteeringBehaviour Behaviour = SteeringBehaviour.RandomSteering;
    private bool colliderExit;

    private void Awake()
    {
        region = GameObject.Find("Region").GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        
        Direction = RandomVector();
        NewDirection = RandomVector();
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        startRotation = transform.rotation;
        angle = Mathf.Atan2(NewDirection.y, NewDirection.x) * Mathf.Rad2Deg;
        newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    void Update()
    {
        transform.position += transform.right * (Speed * Time.deltaTime);
        switch (Behaviour)
        {
            case SteeringBehaviour.RandomSteering:
                RandomSteering();
                break;
            case SteeringBehaviour.AttractorSteering:
                AttractorSteering();
                break;
        }
    }

    void RandomSteering()
    {
        
        
        if (timer<1)
        {
            //Direction = Vector2.Lerp(Direction, NewDirection,timer/3);
            transform.rotation = Quaternion.Lerp(startRotation, newRotation,timer);
        }
        else
        {
            startRotation = newRotation;
            transform.rotation = newRotation;
            NewDirection = RandomVector();
            float angle = Mathf.Atan2(NewDirection.y, NewDirection.x) * Mathf.Rad2Deg;
            newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            timer = 0;
        }

        timer += Time.deltaTime/5;
    }

    void AttractorSteering()
    {
        if (collider.Distance(region).distance>0)
        {
            if (timer<1)
            {
                //Direction = Vector2.Lerp(Direction, NewDirection,timer/3);
                transform.rotation = Quaternion.Lerp(startRotation, newRotation,timer);
            }
            else
            {
                NewDirection = region.transform.position - transform.position.normalized;
                startRotation = transform.rotation;
                float angle = Mathf.Atan2(NewDirection.y, NewDirection.x) * Mathf.Rad2Deg;
                newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                timer = 0;
            }

            timer += Time.deltaTime/2;
        }
        else
        {
            NewDirection = Vector2.zero;
            timer = 0;
        }
        
    }

    Vector2 RandomVector()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(region.transform.position,region.size);
        Gizmos.DrawLine(transform.position, transform.position+(Vector3)NewDirection*3);
    }
}
