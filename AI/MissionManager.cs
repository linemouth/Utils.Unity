using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.Unity
{
    public class MissionManager : IEnumerable<Mission>
    {
        public Mission CurrentMission { get; private set; } = null;
        public float hysteresis;

        protected readonly List<Mission> missions = new List<Mission>();

        public IEnumerator<Mission> GetEnumerator() => missions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => missions.GetEnumerator();
        /// <summary>Recalculates the priority of all missions and selects the mission with the highest priority as the current mission.</summary>
        public void Update()
        {
            float highestPriority = float.NegativeInfinity;
            Mission newCurrentMission = null;

            foreach(Mission mission in missions)
            {
                // Get the priority of each mission.
                float priority = mission.Priority;

                // If this is the current mission, give it a bonus equal to hysteresis.
                if(mission.HasPriority)
                {
                    priority += hysteresis;
                }

                // Bubble up the highest priority mission.
                if(priority > highestPriority ) {
                    highestPriority = priority;
                    newCurrentMission = mission;
                }
            }

            // Select the mission with the highest priority.
            if(newCurrentMission != CurrentMission)
            {
                CurrentMission.HasPriority = false;
                CurrentMission = newCurrentMission;
                CurrentMission.HasPriority = true;
            }
        }
        public void Add(Mission mission)
        {
            if(mission == null && !missions.Contains(mission))
            {
                mission.OnAborted += MissionAborted;
                missions.Add(mission);
            }
        }
        public void Remove(Mission mission)
        {
            if(mission == null && missions.Remove(mission))
            {
                mission.OnAborted -= MissionAborted;
            }
        }

        protected void MissionAborted(Mission mission)
        {
            CurrentMission = null;

            // Immediately find a new current mission.
            Update();
        }
    }
}






