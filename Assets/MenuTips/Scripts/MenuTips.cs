using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace IgnitionImmersive
{
    public class MenuTips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Public
        //public members go here
        public Vector2 dialougeSize = new Vector2(100, 50);
        // Enum to show the hover position when the mouse moves over it. 
        // Add further presets here and add option to the switch below.
        public enum TIPPOSITION
        {
            Upper_Left,
            Upper_Right,
            Lower_Right,
            Lower_Left,
            Centre_Top,
            Centre_Bottom
        };
        // select TIPPOSITION to place on the outer coner indicated
        public TIPPOSITION menuTipPosition = TIPPOSITION.Lower_Right;
        // offset the tips position
        public Vector2 offset = Vector2.zero;
        // prefab of the menu tip 
        [Tooltip("Select the prefab that has a TMP text or text element")]
        public GameObject menuTip;
        // the Text for the tooltip
        public string menuTipText = "Lorum ipsom";
        // check to stay active while in the editor
        public bool isEditing;

        #endregion

        #region Private
        //private members go here
        RectTransform thisTransform;

        Text uiText;
        TextMeshProUGUI tmpText;

        GameObject _menuTip;
        enum TextType
        {
            tmp,
            ui
        };

        TextType textType;
        #endregion
        // Place all unity Message Methods here like OnCollision, Update, Start ect. 
        #region Unity Messages 
        void Start()
        {
            // set the transform
            thisTransform = (RectTransform)gameObject.transform;
            // look for either a TMP pro text or a UI text element.
            _menuTip = Instantiate(menuTip, thisTransform);
            _menuTip.transform.SetParent(thisTransform);
            // get the conners of the menu tips based of the width and height of both the parent element and the MenuTip
            Vector2 pos = new Vector2();
            RectTransform tipRectTransform = (RectTransform)_menuTip.transform;
            pos = new Vector2(thisTransform.rect.width / 2 + tipRectTransform.rect.width / 2, thisTransform.rect.height / 2 + tipRectTransform.rect.height / 2);
            // Set the pos
            SetSizeAndPosition(pos);

            // hide unitl the pointer hovers over the Element
            _menuTip.SetActive(false);

#if UNITY_EDITOR
            if (isEditing)
                _menuTip.SetActive(true);
#endif

        }
        void Update()
        {
            // for in editor editing
#if UNITY_EDITOR
            Vector2 pos = new Vector2();
            RectTransform tipRectTransform = (RectTransform)_menuTip.transform;
            pos = new Vector2(thisTransform.rect.width / 2 + tipRectTransform.rect.width / 2, thisTransform.rect.height / 2 + tipRectTransform.rect.height / 2);
            // update the position
            SetSizeAndPosition(pos);

            // show hide the menu tip for editing.
            if (isEditing && _menuTip.activeInHierarchy == false)
                _menuTip.SetActive(true);
            else if (!isEditing && _menuTip.activeInHierarchy == true)
                _menuTip.SetActive(false);

#endif
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            // disable if editing in the unity editor
#if UNITY_EDITOR
            if (isEditing)
               return;
#endif
            _menuTip.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // disable if editing in the unity editor
#if UNITY_EDITOR
            if (isEditing)
                return;
#endif
            _menuTip.SetActive(true);
        }
        #endregion
        #region Public Methods
        //Place your public methods here

        #endregion
        #region Private Methods
        //Place your public methods here
        private void SetSizeAndPosition(Vector2 pos)
        {
            // set the postion of the predefined enum above. add new predefines here 
            switch (menuTipPosition)
            {
                case TIPPOSITION.Upper_Left:
                    _menuTip.GetComponent<RectTransform>().localPosition = (new Vector2(pos.x * -1 + offset.x, pos.y + offset.y));
                    break;
                case TIPPOSITION.Upper_Right:
                    _menuTip.GetComponent<RectTransform>().localPosition = (new Vector2(pos.x + offset.x, pos.y + offset.y));
                    break;
                case TIPPOSITION.Lower_Right:
                    _menuTip.GetComponent<RectTransform>().localPosition = (new Vector2(pos.x + offset.x, pos.y * -1 + offset.y));
                    break;
                case TIPPOSITION.Lower_Left:
                    _menuTip.GetComponent<RectTransform>().localPosition = (new Vector2(pos.x * -1 + offset.x, pos.y * -1 + offset.y));
                    break;
                case TIPPOSITION.Centre_Top:
                    _menuTip.GetComponent<RectTransform>().localPosition = (new Vector2(0 + offset.x, pos.y + offset.y));
                    break;
                case TIPPOSITION.Centre_Bottom:
                    _menuTip.GetComponent<RectTransform>().localPosition = (new Vector2(0 + offset.x, pos.y * -1 + offset.y));
                    break;
                default: break;
            }
            // set the dialouge's size
            _menuTip.GetComponent<RectTransform>().sizeDelta = dialougeSize;

            // Get the Text Component and set the message type that is used in the prefab
            Text[] ui = _menuTip.GetComponentsInChildren<Text>();
            TextMeshProUGUI[] tmp = _menuTip.GetComponentsInChildren<TextMeshProUGUI>();
            // if a TextMeshProUGUI is found assign the menuTipText to the text field  
            if (tmp.Length > 0)
            {
                textType = TextType.tmp;
                tmpText = tmp[0];
                tmpText.text = menuTipText;
            }
            // if a UI Text is found assign the menuTipText to the text field  
            else if (ui.Length > 0)
            {
                textType = TextType.ui;
                uiText = ui[0];
                uiText.text = menuTipText;
            }
            // Log error is no text element is assigned.
            else
            {
                Debug.LogError("No Text Element found on Menu Text prefab");
            }
        }

        #endregion

    }
}
