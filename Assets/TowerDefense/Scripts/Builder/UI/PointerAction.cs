namespace VRTK.Prefabs.CameraRig.UnityXRCameraRig.Input
{
    using UnityEngine;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Action;

    /// <summary>
    /// Emits action given by user
    /// </summary>
    public class PointerAction : BooleanAction
    {
        private bool value;

        void Start()
        {
            Receive(false);
            value = false;
        }

        public void StartAction()
        {
            Receive(true);
        }

        public void StopAction()
        {
            Receive(false);
        }

        public void ChangeValue()
        {
            value = !value;
            Receive(value);
        }

        public void Activate()
        {
            Receive(true);
            value = true;
        }

        public void Deactivate()
        {
            Receive(false);
            value = false;
        }
    }
}