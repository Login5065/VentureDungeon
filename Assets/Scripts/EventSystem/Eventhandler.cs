using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eventhandler 
{
    EventsList eventsList;
    EventsList daysList;


    public void Load()
    {
        eventsList = new EventsList();
        eventsList.LoadDefault();
        daysList = new EventsList();
        Debug.Log("Event List initialized");

    }
    public EventTemplate CheckIntegrity(EventTemplate a, EventTemplate b) {
        if (a.getImportance() >= b.getImportance())
        {
            return a;
        }
        return b;
    }
    public void PrepareEvent() {
        foreach (EventTemplate e in eventsList.GetEventList()) //the line the error is pointing to
        {
            if (e.Check() == true) {
                daysList.Add(e);
                if (e.getImportance() == 1)
                {
                    daysList.Insert(e);
                }
            }
            
        }
        Debug.Log("nr of events"+ daysList.GetEventList().Count);

    }
    public void StartEvent() {
        if (daysList.GetEventList().Count == 0) {
            Debug.Log("No events");
            return; }
        Debug.Log("events");
        daysList.GetEventList()[0].Enable();
        daysList.RemoveFirst();
    }


    public void Update()
    {
        
    }
}
