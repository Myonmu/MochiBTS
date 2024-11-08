﻿using System.Globalization;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.General
{
    public class DelayNode : ActionNode
    {
        public float duration = 1;
        private float startTime;
        public override string Tooltip =>
            "Will keep running if duration is not reached, counting from the start of the node. Succeeds afterwards";


        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            startTime = Time.time;
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return Time.time - startTime > duration ? State.Success : State.Running;
        }

        public override void UpdateInfo()
        {
            info = $"{duration.ToString(CultureInfo.InvariantCulture)} sec";
        }
    }
}