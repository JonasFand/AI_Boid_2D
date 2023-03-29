using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class SwarmManager : MonoBehaviour
{
    public static SwarmManager Instance { get; private set; }
   
    public Vector2 Amount = new Vector2(2,5);
    public GameObject PrefabToSpawn;
    public float DistanceBetweenEntitys = 0.2f;
    public List<ForceMovement> EntityList;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    
    void Start()
    {
        EntityList = new List<ForceMovement>();
        Spawn();
    }
    
    void Update()
    {
        
    }

    void Spawn()
    {
        Vector3 pos = transform.position;
        for (int i = 0; i < Amount.y; i++)
        {
            for (int j = 0; j < Amount.x; j++)
            {
                pos.y = j + DistanceBetweenEntitys;
                EntityList.Add(Instantiate(PrefabToSpawn, pos, Quaternion.Euler(0, 0, Random.Range(0, 360))).GetComponent<ForceMovement>());
            }
            pos.x = i + DistanceBetweenEntitys;
            pos.y = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        for (int i = 0; i < Amount.y; i++)
        {
            for (int j = 0; j < Amount.x; j++)
            {
                pos.y = j + DistanceBetweenEntitys;
                Gizmos.DrawCube(pos,new Vector3(0.5f,0.5f,0.5f));
            }
            pos.x = i + DistanceBetweenEntitys;
            pos.y = 0;
        }
    }
}
