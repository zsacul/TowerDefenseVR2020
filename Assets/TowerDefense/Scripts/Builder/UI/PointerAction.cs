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
            value = false;
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