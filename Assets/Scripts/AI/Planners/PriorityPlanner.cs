using B3.Interfaces;
using B3.Priorities;
using B3.Utility;
using B3.Utility.PropertyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

using Random = UnityEngine.Random;

namespace B3.Planners
{
    public class PriorityPlanner : MonoBehaviour
    {
        private AbstractPriority[] priorities;
        private int oldPriorityCount = 0;
        private IRandom rng;

        public void Start()
        {
            priorities = GetComponents<AbstractPriority>();
            if (priorities == null || priorities.Length == 0)
            {
                this.LogError("No Priority components found. Disabling Planner.");
                enabled = false;
            }
            else if (priorities.Length == 1)
            {
                this.LogWarning("Only 1 priority found. Weight set to 1.");
                priorities[0].Weight = 1f;
            }
            foreach (AbstractPriority priority in priorities)
            {
                priority.OnDone += Priority_OnDone;
            }
            InvokePriority();
        }

        private void Priority_OnDone(object sender, EventArgs e)
        {
            InvokePriority();
        }

        private void InvokePriority()
        {
            List<FloatRange> ranges = new List<FloatRange>();
            int count = 0;
            foreach (AbstractPriority priority in priorities)
            {
                FloatRange range = new FloatRange();
                range.Weight = priority.Weight;
                range.Min = count;
                range.Max = count;
                ranges.Add(range);
                count += 1;
            }
            int rndIndex = (int)RandomRange.Range(ranges.ToArray());
            priorities[rndIndex].InvokePriority();
        }

        public void OnValidate()
        {
            //if (priorities == null || priorities.Length == 0)
            //{
            //    if (EditorApplication.isPlaying)
            //    {
            //        this.LogError("No Priority components found. Disabling Planner.");
            //        enabled = false;
            //    }
            //}
            //else
            //{
                if (priorities != null && priorities.Length > oldPriorityCount)
                {
                    oldPriorityCount = priorities.Length;
                    AbstractPriority[] newArr = new AbstractPriority[priorities.Length];
                    Array.Copy(priorities, newArr, priorities.Length);
                    priorities = newArr;
                }
                else if (priorities != null && priorities.Length == oldPriorityCount)
                {
                    AbstractPriority[] newArr = new AbstractPriority[priorities.Length];
                    Array.Copy(priorities, newArr, priorities.Length);
                    priorities = newArr;
                }
            //}
        }
    }
}
