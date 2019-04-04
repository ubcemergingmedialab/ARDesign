using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARDesign.Widgets {
    public class DataWidgetHandler : WidgetType<DataWidget>
    {

        [SerializeField]
        private TextMesh label;
        [SerializeField]
        private Text labelText;
        [SerializeField]
        private Text curval;
        [SerializeField]
        private Text curtime;

        //TODO: get rid of this kinda asap
        public SpriteRenderer graph;


        private void Start()
        {

        }

        private void Update()
        {
            curval.text = reader.GetCurrentValue().y.ToString();
            DateTime time = new DateTime((long)reader.GetCurrentValue().x);
            curtime.text = time.ToString();
        }

        public void UpdateLabel()
        {
            label.text = reader.GetLabel();
            labelText.text = reader.GetLabel();
        }

    }
}
