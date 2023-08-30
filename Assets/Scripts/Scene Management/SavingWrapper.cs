using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto.Saving;

namespace Proto.SceneManagement {
public class SavingWrapper : MonoBehaviour
{

    const string defaultSaveFile = "save";
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                Save();
            } 
    }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
}


}
