using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MedicalApp.Core {
    public class SceneTransitionHandler : MonoBehaviour
    {
        private const string EXIT_STRING = "Exit";
        public string previousSceneName;

        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !String.IsNullOrEmpty(previousSceneName))
            {
                if(previousSceneName == EXIT_STRING)
                {
                    Application.Quit();
                }
                else
                {
                    ChangeScene(previousSceneName);
                }
               
            }
        }
    }
}

