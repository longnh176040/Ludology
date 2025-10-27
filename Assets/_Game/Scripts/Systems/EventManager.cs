using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    private static class Holder<T> where T : class // Exclude structs because of iOS JIT issues
    {
        public static Dictionary<Type, Action<T>> s_eventMap = new Dictionary<Type, Action<T>>();

        public static Dictionary<int, Action<T>> p_eventMap = new Dictionary<int, Action<T>>();
    }

    private static class HolderSimple
    {
        public static Dictionary<int, Action> s_eventMap = new Dictionary<int, Action>();
    }

    public static void Connect<T>(Action<T> action) where T : class
    {
        if (Holder<T>.s_eventMap.ContainsKey(typeof(T)))
        {
            Holder<T>.s_eventMap[typeof(T)] += action;
        }
        else
        {
            Holder<T>.s_eventMap.Add(typeof(T), action);
        }
    }

    public static void Disconnect<T>(Action<T> action) where T : class
    {
        if (Holder<T>.s_eventMap.ContainsKey(typeof(T)))
        {
            Holder<T>.s_eventMap[typeof(T)] -= action;
            if (Holder<T>.s_eventMap[typeof(T)] == null)
                Holder<T>.s_eventMap.Remove(typeof(T));
        }
        else
        {
            Debug.LogWarning("Fail to Disconnect handler! Can't find event handler for type " + typeof(T).Name);
        }
    }


    public static void SendEvent<T>(T eventData) where T : class
    {
        if (Holder<T>.s_eventMap.ContainsKey(typeof(T)))
        {
            var handler = Holder<T>.s_eventMap[typeof(T)];
            if (handler != null)
            {
                handler(eventData);
            }
            else
            {
                Debug.LogWarning("Event handler of " + typeof(T).Name + " was destroyed without removing from EventManager!");
            }
        }
    }

    public static void ConnectPair<T>(MonoBehaviour receiver, Action<T> action) where T : class
    {
        int receiverID = receiver.GetHashCode();
        if (Holder<T>.p_eventMap.ContainsKey(receiverID))
        {
            Holder<T>.p_eventMap[receiverID] += action;
        }
        else
        {
            Holder<T>.p_eventMap.Add(receiverID, action);
        }
    }

    public static void DisonnectPair<T>(MonoBehaviour receiver, Action<T> action) where T : class
    {
        int receiverID = receiver.GetHashCode();
        if (Holder<T>.p_eventMap.ContainsKey(receiverID))
        {
            Holder<T>.p_eventMap[receiverID] += action;
        }
        else
        {
            Holder<T>.p_eventMap.Add(receiverID, action);
        }

        if (Holder<T>.p_eventMap.ContainsKey(receiverID))
        {
            Holder<T>.p_eventMap[receiverID] -= action;
            if (Holder<T>.p_eventMap[receiverID] == null)
                Holder<T>.p_eventMap.Remove(receiverID);
        }
        else
        {
            Debug.LogWarning("Fail to Disconnect handler! Can't find event handler for " + receiver.name);
        }
    }

    public static void SendPairEvent<T>(MonoBehaviour receiver, T eventData) where T : class
    {
        int receiverID = receiver.GetHashCode();

        if (Holder<T>.p_eventMap.ContainsKey(receiverID))
        {
            var handler = Holder<T>.p_eventMap[receiverID];
            if (handler != null)
            {
                handler(eventData);
            }
            else
            {
                Debug.LogWarning("Event handler of " + receiver.name + " was destroyed without removing from EventManager!");
            }
        }
    }

    #region Simple event handling

    public static void Connect(Events simpleEvent, Action action)
    {
        if (HolderSimple.s_eventMap.ContainsKey((int)simpleEvent))
        {
            HolderSimple.s_eventMap[(int)simpleEvent] += action;
        }
        else
        {
            HolderSimple.s_eventMap.Add((int)simpleEvent, action);
        }
    }

    public static void Disconnect(Events simpleEvent, Action action)
    {
        if (HolderSimple.s_eventMap.ContainsKey((int)simpleEvent))
        {
            HolderSimple.s_eventMap[(int)simpleEvent] -= action;
            if (HolderSimple.s_eventMap[(int)simpleEvent] == null)
                HolderSimple.s_eventMap.Remove((int)simpleEvent);
        }
        else
        {
            Debug.LogWarning("Fail to Disconnect handler for event: " + simpleEvent.ToString());
        }
    }

    public static void SendSimpleEvent(Events simpleEvent)
    {
        if (HolderSimple.s_eventMap.ContainsKey((int)simpleEvent))
        {
            var handler = HolderSimple.s_eventMap[(int)simpleEvent];
            if (handler != null)
                handler();
        }
    }

    #endregion

    public static void SendEvent<T>(MonoBehaviour receiver, T eventData) where T : class
    {
        if (Holder<T>.s_eventMap.ContainsKey(typeof(T)))
        {
            var handler = Holder<T>.s_eventMap[typeof(T)];
            if (handler != null)
            {
                handler(eventData);
            }
            else
            {
                Debug.LogWarning("Event handler of " + typeof(T).Name + " was destroyed without removing from EventManager!");
            }
        }
    }
}
public enum Events
{
    //Event for game stage
    START_GAME,

    FINISH_DICE,
    END_TURN,

    WIN_GAME,


}
