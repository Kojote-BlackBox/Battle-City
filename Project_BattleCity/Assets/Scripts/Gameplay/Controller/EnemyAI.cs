using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    /* A* Algo
     * 
     * G cost = distance from starting node
     * H cost (heuristic) = distance from end node
     * F cost = G cost + F cost
     * 
     * Schaue alle betrettbaren Nachbarn an.
     * Betrachte den mit den günstigsten F cost.
     * Schließe diesen Knoten (Liste)
     * Wiederhole
     * 
     * Bei Knoten mit gleiche F cost nimmt man den mit geringste H cost.
     * 
     */
}
