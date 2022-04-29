using B3.Actions;
using B3.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace B3.Utility
{
    public static class Logging
    {
        /// <summary>
        /// Method to streamline logging formatting in general.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="caller"></param>
        /// <param name="message"></param>
        /// <param name="callerName"></param>
        /// <param name="getHighestClass"></param>
        public static void LogGeneral<T>(T caller, object message, string callerName = "", bool getHighestClass = true)
        {
            LogHidden(caller, message, LoggingEnum.Log, callerName, getHighestClass);
        }

        /// <summary>
        /// Method to streamline warning logging formatting in general.
        /// </summary>
        /// <typeparam name="T">The class type</typeparam>
        /// <param name="caller">The caller of the method.</param>
        /// <param name="message">The message object to pass to Unity's Log method.</param>
        /// <param name="callerName">An optional parameter if the debug call should list a name for the caller.</param>
        /// <param name="getHighestClass">Whether to only print the name of the highest class in the hiearchy or the whole class hierarchy.</param>
        public static void LogWarningGeneral<T>(T caller, object message, string callerName = "", bool getHighestClass = true)
        {
            LogHidden(caller, message, LoggingEnum.Warning, callerName, getHighestClass);
        }

        /// <summary>
        /// Method to streamline error logging formatting in general.
        /// </summary>
        /// <typeparam name="T">The class type</typeparam>
        /// <param name="caller">The caller of the method.</param>
        /// <param name="message">The message object to pass to Unity's Log method.</param>
        /// <param name="callerName">An optional parameter if the debug call should list a name for the caller.</param>
        /// <param name="getHighestClass">Whether to only print the name of the highest class in the hiearchy or the whole class hierarchy.</param>
        public static void LogErrorGeneral<T>(T caller, object message, string callerName = "", bool getHighestClass = true)
        {
            LogHidden(caller, message, LoggingEnum.Error, callerName, getHighestClass);
        }

        /// <summary>
        /// The logging method that does the actual logging call, given an enum value.
        /// </summary>
        /// <typeparam name="T">The class type</typeparam>
        /// <param name="caller">The caller of the method.</param>
        /// <param name="message">The message object to pass to Unity's Log method.</param>
        /// <param name="logType">What type of logging it is.</param>
        /// <param name="callerName">An optional parameter if the debug call should list a name for the caller.</param>
        /// <param name="getHighestClass">Whether to only print the name of the highest class in the hiearchy or the whole class hierarchy.</param>
        private static void LogHidden<T>(T caller, object message, LoggingEnum logType, string callerName = "", bool getHighestClass = true)
        {
            string className = $"{typeof(T)}";
            string callerString = "";
            if (getHighestClass == true)
            {
                className = caller.GetHighestClassName();
            }
            callerString = $"[{className}]";

            if (!string.IsNullOrEmpty(callerName))
            {
                callerString += $"[{callerName}]";
            }

            switch (logType)
            {
                case LoggingEnum.Log:
                    Debug.Log($"{callerString}:" + message);
                    break;
                case LoggingEnum.Warning:
                    Debug.LogWarning($"{callerString}:" + message);
                    break;
                case LoggingEnum.Error:
                    Debug.LogError($"{callerString}:" + message);
                    break;
            }
        }

        /// <summary>
        /// Extension method used to return the highest class name in the class hierarchy.
        /// </summary>
        /// <typeparam name="T">The class type.</typeparam>
        /// <param name="obj">The object to call the method on.</param>
        /// <returns>A string containing the name of the highest class in the hierarchy.</returns>
        public static string GetHighestClassName<T>(this T obj)
        {
            string classString = $"{typeof(T)}";
            if (classString.Contains("."))
            {
                string[] splitString = classString.Split('.');
                return splitString[splitString.Length - 1];
            }
            else
            {
                return classString;
            }
        }
    }

    public static class LoggingExtensionMonoBehaviour
    {
        /// <summary>
        /// Extension method to streamline logging formatting for MonoBehaviours.
        /// </summary>
        /// <typeparam name="T">The class type</typeparam>
        /// <param name="caller">The caller of the method.</param>
        /// <param name="message">The message object to pass to Unity's Log method.</param>
        /// <param name="getHighestClass">Whether to only print the name of the highest class in the hiearchy or the whole class hierarchy.</param>
        public static void Log<T>(this T caller, object message, bool getHighestClass = true) where T : MonoBehaviour
        {
            string className = $"{typeof(T)}";
            if (getHighestClass == true)
            {
                className = caller.GetHighestClassName();
            }
            Debug.Log($"[{className}][{caller.name}]:" + message);
        }

        /// <summary>
        /// Extension method to streamline warning logging formatting for MonoBehaviours.
        /// </summary>
        /// <typeparam name="T">The class type</typeparam>
        /// <param name="caller">The caller of the method.</param>
        /// <param name="message">The message object to pass to Unity's Log method.</param>
        /// <param name="getHighestClass">Whether to only print the name of the highest class in the hiearchy or the whole class hierarchy.</param>
        public static void LogWarning<T>(this T caller, object message, bool getHighestClass = true) where T : MonoBehaviour
        {
            string className = $"{typeof(T)}";
            if (getHighestClass == true)
            {
                className = caller.GetHighestClassName();
            }
            Debug.LogWarning($"[{className}][{caller.name}]:" + message);
        }

        /// <summary>
        /// Extension method to streamline error logging formatting for MonoBehaviours.
        /// </summary>
        /// <typeparam name="T">The class type</typeparam>
        /// <param name="caller">The caller of the method.</param>
        /// <param name="message">The message object to pass to Unity's Log method.</param>
        /// <param name="getHighestClass">Whether to only print the name of the highest class in the hiearchy or the whole class hierarchy.</param>
        public static void LogError<T>(this T caller, object message, bool getHighestClass = true) where T : MonoBehaviour
        {
            string className = $"{typeof(T)}";
            if (getHighestClass == true)
            {
                className = caller.GetHighestClassName();
            }
            Debug.LogError($"[{className}][{caller.name}]:" + message);
        }
    }
}
