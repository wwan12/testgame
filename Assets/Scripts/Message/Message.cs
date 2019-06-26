/*
*Ilya Suzdalnitski的高级C#信使。V1.0
*
*基于罗德·海德的《CSharpmessenger》和马格纳斯·沃尔菲特的《CSharpmessenger extended》。
*
*特点：
*阻止MissingReferenceException，因为引用了已销毁的消息处理程序。
*记录所有消息的选项
*广泛的错误检测，防止沉默的错误
*
*用法示例：
1。messenger.addlistener<gameobject>（prop collected，propcollected）；
messenger.broadcast<gameobject>（prop collected，prop）；
2。messenger.addlistener<float>（"speed changed"，SpeedChanged）；
messenger.broadcast<float>（"speed changed"，0.5f）；
*
*在加载新级别时，messenger会自动清除其可能发生的事件。
*
*不要忘记，应该在清理之后保留的消息应该用messenger.MarkAsPermanent（string）标记。
 * 
 */

//#define LOG_ALL_MESSAGES
//#define LOG_ADD_LISTENER
//#define LOG_BROADCAST_MESSAGE
//#define REQUIRE_LISTENER 是否必须添加接收监听

using System;
using System.Collections.Generic;
using UnityEngine;
 
static internal class Messenger {
    #region Internal variables

    //禁用未使用的变量警告
#pragma warning disable 0414
    //确保messengerhelper将在游戏开始时自动创建。
    private static  MessengerHelper messengerHelper = ( new GameObject("MessengerHelper") ).AddComponent< MessengerHelper >();
#pragma warning restore 0414

    public static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

    //不应删除的消息处理程序，无论调用清理
    public static  List<string> permanentMessages = new List< string > ();
    #endregion
    #region Helper methods
    //将某条消息标记为永久消息。
    static public void MarkAsPermanent(string eventType) {
#if LOG_ALL_MESSAGES
        Debug.Log("Messenger MarkAsPermanent \t\"" + eventType + "\"");
#endif
        if (!permanentMessages.Contains(eventType))
        {
            permanentMessages.Add(eventType);
        }
    }
 
    static public void Cleanup()
    {
#if LOG_ALL_MESSAGES
        Debug.Log("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
#endif
 
        List< string > messagesToRemove = new List<string>();
 
        foreach (KeyValuePair<string, Delegate> pair in eventTable) {
            bool wasFound = false;
 
            foreach (string message in permanentMessages) {
                if (pair.Key == message) {
                    wasFound = true;
                    break;
                }
            }
 
            if (!wasFound)
                messagesToRemove.Add( pair.Key );
        }
 
        foreach (string message in messagesToRemove) {
            eventTable.Remove( message );
        }
    }
 
    static public void PrintEventTable()
    {
        Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");
 
        foreach (KeyValuePair<string, Delegate> pair in eventTable) {
            Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
        }
 
        Debug.Log("\n");
    }
    #endregion
 
    #region Message logging and exception throwing
    static public void OnListenerAdding(string eventType, Delegate listenerBeingAdded) {
#if LOG_ALL_MESSAGES || LOG_ADD_LISTENER
        Debug.Log("MESSENGER OnListenerAdding \t\"" + eventType + "\"\t{" + listenerBeingAdded.Target + " -> " + listenerBeingAdded.Method + "}");
#endif
 
        if (!eventTable.ContainsKey(eventType)) {
            eventTable.Add(eventType, null );
        }
 
        Delegate d = eventTable[eventType];
        if (d != null && d.GetType() != listenerBeingAdded.GetType()) {
            throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
        }
    }
 
    static public void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved) {
#if LOG_ALL_MESSAGES
        Debug.Log("MESSENGER OnListenerRemoving \t\"" + eventType + "\"\t{" + listenerBeingRemoved.Target + " -> " + listenerBeingRemoved.Method + "}");
#endif
        //if (eventTable.ContainsKey(eventType)) {
        //    Delegate d = eventTable[eventType];
 
        //    if (d == null) {
        //        throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
        //    } else if (d.GetType() != listenerBeingRemoved.GetType()) {
        //        throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
        //    }
        //} else {
        //    throw new ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
        //}
    }
 
    static public void OnListenerRemoved(string eventType) {
        if (eventTable.ContainsKey(eventType) && eventTable[eventType] == null)
        {
            eventTable.Remove(eventType);
        }
    }

    static public void RemovedUIMgrOnBroad(string eventType)
    {
        if (eventTable.ContainsKey(eventType))
        {
            if (eventTable[eventType] != null)
            {
                eventTable[eventType] = null;
            }
            eventTable.Remove(eventType);
        }
    }
 
    static public void OnBroadcasting(string eventType) {
#if REQUIRE_LISTENER
        if (!eventTable.ContainsKey(eventType)) {
            throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
        }
#endif
    }
 
    static public BroadcastException CreateBroadcastSignatureException(string eventType) {
        return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
    }
 
    public class BroadcastException : Exception {
        public BroadcastException(string msg)
            : base(msg) {
        }
    }
 
    public class ListenerException : Exception {
        public ListenerException(string msg)
            : base(msg) {
        }
    }
    #endregion
 
    #region AddListener
    //No parameters
    static public void AddListener(string eventType, Callback handler) {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback)eventTable[eventType] + handler;
    }
 
    //Single parameter
    static public void AddListener<T>(string eventType, Callback<T> handler) {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
    }
 
    //Two parameters
    static public void AddListener<T, U>(string eventType, Callback<T, U> handler) {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback<T, U>)eventTable[eventType] + handler;
    }
 
    //Three parameters
    static public void AddListener<T, U, V>(string eventType, Callback<T, U, V> handler) {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] + handler;
    }
    //四个参数
    static public void AddListener<T, U, V,E>(string eventType, Callback<T, U, V,E> handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback<T, U, V,E>)eventTable[eventType] + handler;
    }

    //Single parameter
    static public void AddReturnListener<T,R>(string eventType, CallbackReturn<T,R> handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (CallbackReturn<T,R>)eventTable[eventType] + handler;
    }


    //四个参数+返回值
    static public void AddReturnListener<T, U, V, E,R>(string eventType, CallbackReturn<T, U, V, E,R> handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (CallbackReturn<T, U, V, E,R>)eventTable[eventType] + handler;
    }

    #endregion

    #region RemoveListener
    //No parameters
    static public void RemoveListener(string eventType, Callback handler) {
        //OnListenerRemoving(eventType, handler);
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = (Callback)eventTable[eventType] - handler;
        }
        OnListenerRemoved(eventType);
    }
 
    //Single parameter
    static public void RemoveListener<T>(string eventType, Callback<T> handler) {
        //OnListenerRemoving(eventType, handler);
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;
        }
        OnListenerRemoved(eventType);
    }
 
    //Two parameters
    static public void RemoveListener<T, U>(string eventType, Callback<T, U> handler) {
        //OnListenerRemoving(eventType, handler);
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = (Callback<T, U>)eventTable[eventType] - handler;
        }
        OnListenerRemoved(eventType);
    }
 
    //Three parameters
    static public void RemoveListener<T, U, V>(string eventType, Callback<T, U, V> handler) {
        //OnListenerRemoving(eventType, handler);
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] - handler;
        }
        OnListenerRemoved(eventType);
    }
    #endregion
 
    #region Broadcast
    //No parameters
    static public void Broadcast(string eventType) {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
        Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);
 
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d)) {
            Callback callback = d as Callback;
 
            if (callback != null) {
                callback();
            } else {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
 
    //Single parameter
    static public void Broadcast<T>(string eventType, T arg1) {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
        Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);
 
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d)) {
            Callback<T> callback = d as Callback<T>;
 
            if (callback != null) {
                callback(arg1);
            } else {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
 
    //Two parameters
    static public void Broadcast<T, U>(string eventType, T arg1, U arg2) {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
        Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);
 
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d)) {

            if (d is Callback<T, U> callback)
            {
                callback(arg1, arg2);
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
 
    //Three parameters
    static public void Broadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3) {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
        Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);
 
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d)) {
            Callback<T, U, V> callback = d as Callback<T, U, V>;
 
            if (callback != null) {
                callback(arg1, arg2, arg3);
            } else {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
    //Three parameters
    static public void Broadcast<T, U, V,E>(string eventType, T arg1, U arg2, V arg3,E arg4)
    {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
        Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);

        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            Callback<T, U, V,E> callback = d as Callback<T, U, V,E>;

            if (callback != null)
            {
                callback(arg1, arg2, arg3,arg4);
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }

    //Three parameters
    static public R BroadcastReturn<T, R>(string eventType, T arg1)
    {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
        Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);

        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            R r;
            if (d is CallbackReturn<T, R> callback)
            {
                r = callback(arg1);
                return r;
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
        return default;
    }

 

    //Three parameters
    static public R BroadcastReturn<T, U, V, E,R>(string eventType, T arg1, U arg2, V arg3, E arg4)
    {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
        Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);

        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            CallbackReturn<T, U, V, E,R> callback = d as CallbackReturn<T, U, V, E,R>;
            R r;
            if (callback != null)
            {
               r= callback(arg1, arg2, arg3, arg4);
               return r;
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }           
        }
        return default(R);
    }

    #endregion
}

//此管理器将确保在加载新级别时清理信使的事件表。
public sealed class MessengerHelper : MonoBehaviour {
    void Awake ()
    {
        DontDestroyOnLoad(gameObject);    
    }

    //每次加载新级别时清理事件表。
    public void OnDisable() {
        Messenger.Cleanup();
    }
}