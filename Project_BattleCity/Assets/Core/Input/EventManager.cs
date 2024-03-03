using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public static EventManager gameEvents;

    private void Awake() => gameEvents = this;

    public static event Action OnGunFire;   

    public void Fire() {
        if(OnGunFire != null) {
            OnGunFire();
        }
    }
}
