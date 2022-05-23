using System;
using TMPro;
using UnityEngine;

namespace GAME_MAIN.Scripts.UI
{
    public class LevelCounterDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private void Start()
        {
            _text.text = "LEVEL " + SaveManager.instance.LevelCounter.ToString();
        }
    }
}
