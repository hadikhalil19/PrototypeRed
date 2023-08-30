using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.SceneManagement {
public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName {get; private set;}

    public void SetTransitionName(string sceneTransitionName) {
        this.SceneTransitionName = sceneTransitionName;
    }
}
}
