using UnityEngine;
using FireExtinguisher.Attributes;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;

namespace FireExtinguisher.Utilities
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "ScriptableObjects/ProjectSettings", order = 1)]
    public class ProjectSettings : ScriptableObject
    {
        public float version;
        [EnumFlagsAttribute]
        public SceneObjects flamableObjects;

        public IEnumerable<string>[] GetEnumerableFlamableObjects()
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


            IEnumerable<string>[] flamableObjectsArr = new IEnumerable<string>[selectedElements.Count];

            flamableObjectsArr = new IEnumerable<string>[selectedElements.Count];

            for (int i = 0; i < selectedElements.Count; i++)
            {
                flamableObjectsArr[i] = new[] { ((SceneObjects)selectedElements[i]).ToString() };
            }

            return flamableObjectsArr;
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


            string[] flamableObjectsArr = new string[selectedElements.Count];

            flamableObjectsArr = new string[selectedElements.Count];

            for (int i = 0; i < selectedElements.Count; i++)
            {
                flamableObjectsArr[i] = ((SceneObjects)selectedElements[i]).ToString();
            }

            return flamableObjectsArr;
        }
    }
}