using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MedicalApp.Core
{
    public class DataEntryHandler : MonoBehaviour
    {
        public SO_DAO dao;
        public Button proceedButton;
        public Color validInput;
        public Color invalidInput;

        private void Start()
        {
            dao.Initialize();
        }

        private void Update()
        {
            CheckIfUserCanProceedToTest();
        }

        public bool AddValueToDao(string key, string value)
        {
            print(key + "  " + value);
            return dao.AddItem(key, value);
        }
      
        public void CheckIfUserCanProceedToTest()
        {
            if (dao.ValidateUserInput())
            {
                proceedButton.interactable = true;
            }
            else
            {
                proceedButton.interactable = false;
            }
        }
    }
}