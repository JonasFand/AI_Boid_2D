using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Movement : MonoBehaviour
{
    [Range(0,5)]
    public float Speed = 30;
    public Vector2 Direction;

    private Vector2 newDirection;
    public Vector2 NewDirection
    {
        get => newDirection;
        set
        {
            newDirection = value;
            ApplyNewDirection();
        }
        
    }

    public Vector2 ForceVector = Vector2.zero;
    public Vector2 UnitEvasionVector = Vector2.zero;
    public Vector2 CheckPointVector = Vector2.zero;
    public Vector2 CentroidVector = Vector2.zero;
    public float EvasionRadius;
    private BoxCollider2D region;
    [SerializeField]private CircleCollider2D evasionCollider;
    [SerializeField]public PolygonCollider2D collider;
    [SerializeField]private float timer = 0;
    Quaternion startRotation;
    Quaternion newRotation;
    private bool colliderExit;

    private void Awake()
    {
        region = GameObject.Find("Region").GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        newDirection = RandomVector();
        ApplyNewDirection();
        float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
        newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    

    void Update()
    {
       //transform.position += transform.right * (Speed * Time.deltaTime);
       ManagerSteering();
       transform.position += (Vector3)newDirection.normalized * (Speed * Time.deltaTime);
       
    }

    void ManagerSteering()
    {
        if (timer < 1)
        {
            transform.rotation = Quaternion.Lerp(startRotation, newRotation, timer);
        }
        else
        {
            
        }

        timer += Time.deltaTime;
    }

    void ApplyNewDirection()
    {
        Direction = transform.right;
        newDirection = (ForceVector+newDirection);
        if (UnitEvasionVector.magnitude>0)
        {
            newDirection = (newDirection-UnitEvasionVector);
        }
        if (CheckPointVector.magnitude>0)
        {
            newDirection = (newDirection+CheckPointVector);
        }
        if (CheckPointVector.magnitude>0)
        {
            newDirection = (newDirection+CentroidVector);
        }
        
        
        newDirection.Normalize();
        startRotation = transform.rotation;
        float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
        newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        timer = 0;
    }

    Vector2 RandomVector()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    private void OnDrawGizmos()
    {
        
        //Gizmos.DrawWireCube(region.transform.position,region.size);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position+(Vector3)newDirection.normalized);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position+transform.right.normalized);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position+(Vector3)ForceVector);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position-(Vector3)UnitEvasionVector);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position+(Vector3)CentroidVector);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position+(Vector3)CheckPointVector);
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        Gizmos.DrawWireSphere(transform.position,EvasionRadius);
    }
}
