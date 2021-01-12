using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsList 
{
    private List<EventTemplate> eventList = new List<EventTemplate>();
    
    public void Add(EventTemplate e) {
        eventList.Add(e);
    }
    public void Remove(EventTemplate e) {
        eventList.Remove(e);
    }

    public void RemoveFirst() { eventList.RemoveAt(0); }
    public void Insert(EventTemplate e) {
        eventList.Insert(0, e);
    }
    public void LoadDefault() {
        eventList.Add(new DayEvent());
        eventList.Add(new FameEvent());
        eventList.Add(new ThreatEvent());
    }
    public List<EventTemplate> GetEventList()
    {
        return eventList;
    }
    public EventTemplate GetEvent(int i) {
        return eventList[i];
    }
    
}
    
