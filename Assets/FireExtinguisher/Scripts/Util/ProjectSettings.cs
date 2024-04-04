using UnityEngine;
using System.Collections.Generic;

namespace FireExtinguisher.Utilities
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "ScriptableObjects/ProjectSettings", order = 1)]
    public class ProjectSettings : ScriptableObject
    {
        public float version;

        [EnumFlagsAttribute]
        public SceneObjects flamableObjects;
        [EnumFlagsAttribute]
        public SceneObjects fireExtinguisherPlacement;

        #region PropertiesGetter
        public IEnumerable<string>[] GetFlamableObjectsEnumerable()
        {
            List<int> selectedElements = new List<int>();
            for (int i = 0; i < System.Enum.GetValues(typeof(SceneObjects)).Length; i++)
            {
                int layer = 1 << i;
                if (((int)flamableObjects & layer) != 0)
                {
                    selectedElements.Add(i);
                }
            }


            IEnumerable<string>[] flamableObjectList = new IEnumerable<string>[selectedElements.Count];

            for (int i = 0; i < selectedElements.Count; i++)
            {
                flamableObjectList[i] = new[] { ((SceneObjects)selectedElements[i]).ToString() };
            }

            return flamableObjectList;
        }

        public string[] GetFlamableObjects()
        {
            List<int> selectedElements = new List<int>();
            for (int i = 0; i < System.Enum.GetValues(typeof(SceneObjects)).Length; i++)
            {
                int layer = 1 << i;
                if (((int)flamableObjects & layer) != 0)
                {
                    selectedElements.Add(i);
                }
            }


            string[] flamableObjectList = new string[selectedElements.Count];

            for (int i = 0; i < selectedElements.Count; i++)
            {
                flamableObjectList[i] = ((SceneObjects)selectedElements[i]).ToString();
            }

            return flamableObjectList;
        }       
        
        public IEnumerable<string>[] GetPlaceableObjectsEnumerable()
        {
            List<int> selectedElements = new List<int>();
            for (int i = 0; i < System.Enum.GetValues(typeof(SceneObjects)).Length; i++)
            {
                int layer = 1 << i;
                if (((int)fireExtinguisherPlacement & layer) != 0)
                {
                    selectedElements.Add(i);
                }
            }


            IEnumerable<string>[] placeableObjects = new IEnumerable<string>[selectedElements.Count];

            for (int i = 0; i < selectedElements.Count; i++)
            {
                placeableObjects[i] = new[] { ((SceneObjects)selectedElements[i]).ToString() };
            }

            return placeableObjects;
        }

        public string[] GetPlaceableObjectsObjects()
        {
            List<int> selectedElements = new List<int>();
            for (int i = 0; i < System.Enum.GetValues(typeof(SceneObjects)).Length; i++)
            {
                int layer = 1 << i;
                if (((int)fireExtinguisherPlacement & layer) != 0)
                {
                    selectedElements.Add(i);
                }
            }


            string[] placeableObjects = new string[selectedElements.Count];

            for (int i = 0; i < selectedElements.Count; i++)
            {
                placeableObjects[i] = ((SceneObjects)selectedElements[i]).ToString();
            }

            return placeableObjects;
        }
        #endregion

        public void UpdateVersion()
        {
            version = Mathf.Round((version + 0.01f) * 100) / 100f;
        }
    }
}