using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Types = Model.Types;

namespace UI.Game
{
    public class Item : MonoBehaviour
    {
        public event Action<int, Types> ClickEvent;

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button button;

        public int CaseID { get; private set; }
        public Types CaseTypes { get; private set; }


        public void SetID(int value, Types types)
        {
            CaseID = value;
            CaseTypes = types;
        }

        public void SetImage(Sprite sprite) => 
            image.sprite = sprite;

        public void SetText(string value) => 
            text.text = value;

        public void SetActiveText(bool value) => 
            text.color = value 
            ? Color.white 
            : Color.clear;

        public void RemoveID()
        {
            CaseID = -1;
            CaseTypes = Types.Null;

            SetImage(null);
            SetText(string.Empty);
            SetActiveText(false);
        }

        private void Awake()
        {
            CaseID = -1;
            CaseTypes = Types.Null;

            if (button != null) 
                button.onClick.AddListener(OnClick);
        }

        private void OnClick() => 
            ClickEvent?.Invoke(CaseID, CaseTypes);
    }
}