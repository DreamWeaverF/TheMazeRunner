using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TheMazeRunner
{
    public class UITest 
    {
        private TextMeshProUGUI mTextUserName;
        private TextMeshProUGUI mTextPassword;
        private string DefaultName;

        public UITest()
        {
            GameObject uiPrefab = Resources.Load<GameObject>(this.GetType().Name);
            if(uiPrefab == null)
            {
                return;
            }
            GameObject uiText = GameObject.Instantiate(uiPrefab);
            uiText.SetActive(true);
            AutoLinkComponent linkCom = uiText.GetComponent<AutoLinkComponent>();
            if(linkCom == null)
            {
                return;
            }
            //linkCom.SerializeDatas.TryGetValue("mTextUserName", out object outTextUserName);
            //mTextUserName = (TextMeshProUGUI)outTextUserName;
            //linkCom.SerializeDatas.TryGetValue("mTextPassword", out object outTextPassword);
            //mTextPassword = (TextMeshProUGUI)outTextPassword;
            //linkCom.SerializeDatas.TryGetValue("DefaultName", out object outDefaultName);
            //DefaultName = (string)outDefaultName;
        }
    }
}
