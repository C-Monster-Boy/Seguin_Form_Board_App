using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MedicalApp.Core
{
    public class DataIdentifyingObject : MonoBehaviour
    {
        public string daoIndifierKey;
        public TMP_InputField inputField;

        private DataEntryHandler dataEntryHandler;

        private void Start()
        {
            dataEntryHandler = FindObjectOfType<DataEntryHandler>();
        }

        public void AddData(string value)
        {
            bool isDataValid = dataEntryHandler.AddValueToDao(daoIndifierKey, value);
            if(!isDataValid)
            {
                GetComponent<Image>().color = dataEntryHandler.invalidInput;
            }
            else
            {
                GetComponent<Image>().color = dataEntryHandler.validInput;
            }

        }
    }
}

