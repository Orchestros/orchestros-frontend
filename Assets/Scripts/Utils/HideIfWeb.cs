using System;
using UnityEngine;

namespace Utils
{
    public class HideIfWeb : MonoBehaviour
    {
        private void Start()
        {
#if UNITY_WEBGL
            gameObject.SetActive(false);
#endif
        }
    }
}