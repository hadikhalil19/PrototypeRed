using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.Dialogue {

    [System.Serializable]
    public class DailogueNode 
    {
        public string uniqueID;
        public string text;
        public string[] children;
    }
}