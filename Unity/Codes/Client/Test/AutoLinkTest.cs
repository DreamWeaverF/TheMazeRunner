using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public class AutoLinkTest : AutoLinkCompoent
    {
        //[AutoLinkField]
        //private TextMeshProUGUI m_TextUserName;
        //[AutoLinkField]
        //private TextMeshProUGUI m_TextPassword;
        [AutoLinkField]
        private string m_DefaultStingValue;
        [AutoLinkField]
        private int m_DefaultIntValue;
        [AutoLinkField]
        private float m_DefaultFloatValue;
        [AutoLinkField]
        private List<int> m_DefaultIntListValue;



        public override void Awake()
        {
            base.Awake();
            Debug.Log($"AutoLinkTest Awake");

            //m_TextUserName.text = m_DefaultStingValue;
            //m_TextPassword.text = $"{m_DefaultIntValue}";
        }
        public void Update()
        {

        }
    }
}
