using UnityEngine;
using FireExtinguisher.Attributes;
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
        
        public IEnumerable<string>[] GetPlacableObjectsEnumerable()
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


            IEnumerable<string>[] placableObjects = new IEnumerable<string>[selectedElements.Count];

            for (int i = 0; i < selectedElements.Count; i++)
            {
                placableObjects[i] = new[] { ((SceneObjects)selectedElements[i]).ToString() };
            }

            return placableObjects;
        }

        public string[] GetPlacableObjectsObjects()
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


            string[] placableObjects = new string[selectedElements.Count];

            for (int i = 0; i < selectedElements.Count; i++)
            {
                placableObjects[i] = ((SceneObjects)selectedElements[i]).ToString();
            }

            return placableObjects;
        }
        #endregion

        public void UpdateVersion()
        {
            version = Mathf.Round((version + 0.01f) * 100) / 100f;
        }
    }
}