using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TheMazeRunner
{
    public class AutoLinkComponentMonoTest : AutoLinkComponentMono
    {
        [SerializeField]
        private TextMeshProUGUI m_TextUserName;
        [SerializeField]
        private TextMeshProUGUI m_TextPassword;
        [SerializeField]
        private string m_DefaultStingValue;
        [SerializeField]
        private int m_DefaultIntValue;
        [SerializeField]
        private float m_DefaultFloatValue;
        [SerializeField]
        private List<int> m_DefaultIntListValue;
    }
}
