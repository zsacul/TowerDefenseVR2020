namespace Zinnia.Action
{
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Emits a <see cref="bool"/> value.
    /// </summary>
    public class BooleanAction : Action<BooleanAction, bool, BooleanAction.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="bool"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<bool>
        {
        }

        //Serves only as a way to turn off currently activated teleport pointer because every other way failed
        public void TurnOff()
        {
            ProcessValue(false);
        }
    }
}