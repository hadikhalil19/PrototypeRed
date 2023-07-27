using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.Dialogue {

    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject {
        [SerializeField] DailogueNode[] node;
    }

}
