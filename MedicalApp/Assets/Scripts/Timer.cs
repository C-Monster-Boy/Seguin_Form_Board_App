using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MedicalApp.Core
{
    public class Timer : MonoBehaviour
    {
        public delegate void TimerEnd(TimerDetails timerDetails);
        public static event TimerEnd OnTimerEnd;

        [SerializeField]
        TimerDetails timerDetails;
        bool startTimer = false;

        public void StartTimer(string timerName, float duration)
        {
            timerDetails = new TimerDetails()
            {
                name = timerName,
                totalTime = duration,
                timeRemaining = duration
            };

            startTimer = true;
        }

        public void StopTimer()
        {
            startTimer = false;
        }

        public float GetPassedTime()
        {         
            return timerDetails.totalTime - timerDetails.timeRemaining;
        }

        // Start is called before the first frame update
        void Start()
        {
            startTimer = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(startTimer && timerDetails.timeRemaining > 0)
            {
                timerDetails.timeRemaining -= Time.deltaTime;
            }
            else if (startTimer && timerDetails.timeRemaining <= 0)
            {
                startTimer = false;
                OnTimerEnd?.Invoke(timerDetails);
            }
        }
    }

    [System.Serializable]
    public struct TimerDetails
    {
        public string name;
        public float totalTime;
        public float timeRemaining;
    }
}

