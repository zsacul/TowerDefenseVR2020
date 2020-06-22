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
        private BuildManager buildManager;

        void Start()
        {
            buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
        }

        void Update()
        {
            Receive(buildManager.currentlyBuilding);
        }

    }
}