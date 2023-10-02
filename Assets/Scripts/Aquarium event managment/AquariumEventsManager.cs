using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;

public class AquariumEventsManager : MonoBehaviour
{
    public AquariumEvent[] poolOfEvents;
    public struct ListedEvent
    {
        public string Name;
        public AquariumEvent AE;
        public EventOnUI EUI;
        public int Timer;

        public ListedEvent(string _Name, AquariumEvent _AE, EventOnUI _EUI, int _Timer)
        {
            Name = _Name;
            AE = _AE;
            EUI = _EUI;
            Timer = _Timer;
        }
    }

    [Space(16)]
    public int EventCount = 6;
    public int StartWait = 15;
    public List<ListedEvent> SeriesOfEvents = new List<ListedEvent>();

    [Header("Roll")]
    public Transform rollParent;
    public EventOnUI EUI_Agent;


    //*************************************************************************


    private void Start()
    {
        FillTheList();
        StartCoroutine(CountBackTheEvents());
    }


    IAquariumEvent eventSpawn;
    EventOnUI EUI_Spawn;
    AquariumEvent choosenToFill;
    int filloop, waitingT;
    void FillTheList()
    {
        waitingT = StartWait;
        for (filloop = 0; filloop < EventCount; filloop++)
        {
            choosenToFill = poolOfEvents[Random.Range(0, poolOfEvents.Length)];

            eventSpawn = LeanPool.Spawn<MonoBehaviour>(choosenToFill.theEvent as MonoBehaviour) as IAquariumEvent;
            eventSpawn.DependentItemPool(choosenToFill.theEvent);
            LeanPool.Despawn(eventSpawn as MonoBehaviour);

            EUI_Spawn = LeanPool.Spawn<EventOnUI>(EUI_Agent);
            EUI_Spawn.CountDown.text = waitingT.ToString();
            EUI_Spawn.Pic.sprite = choosenToFill.Pic;

            EUI_Spawn._transform.SetParent(rollParent);

            SeriesOfEvents.Add(new ListedEvent(choosenToFill.Name, choosenToFill, EUI_Spawn, waitingT));

            EUI_Spawn.indexed = SeriesOfEvents[filloop];

            waitingT += SeriesOfEvents[filloop].AE.afterTime;
        }

        StopCoroutine(CountBackTheEvents());
    }


    int pastSeconds;
    IEnumerator CountBackTheEvents()
    {
        while(true)
        {
            pastSeconds++;

            //******************

            //�erit g�r�n�m�n� yenile
            RollTimers();

            //******************

            yield return new WaitForSeconds(1);
        }
    }


    int secondsLeft;
    void RollTimers()
    {
        foreach (ListedEvent le in SeriesOfEvents)
        {
            secondsLeft = le.Timer - pastSeconds;

            le.EUI.CountDown.text = secondsLeft.ToString();

            if (secondsLeft <= 0)
            {
                //*************************

                UseOfAnEvent(le);

                break;

                //*************************
            }
        }
    }


    public void UseOfAnEvent(ListedEvent indexed)
    {
        indexed.AE.TheHappening();

        LeanPool.Despawn(indexed.EUI);

        SeriesOfEvents.Remove(indexed);
        
        if (SeriesOfEvents.Count == 0)
        {
            EventsCleared();
            return;
        }
        
        RollTimers();

        for (int earlyloop = 0; earlyloop < SeriesOfEvents.Count; earlyloop++)
        {
            SeriesOfEvents[earlyloop].EUI.indexed = SeriesOfEvents[earlyloop];
        }
    }
    
    public static event UnityAction AllEventsCompleted;

    void EventsCleared()
    {
        AllEventsCompleted?.Invoke();
        // end of level
    }
}
