using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Winger.Input.Raw;
using Winger.SimpleMath;
using Winger.UserInterface.Utils;
using Winger.Utils;
using System;

namespace Winger.UserInterface
{
    public class Page : CMouseEventHandler, CGamePadEventHandler, CKeyboardEventHandler
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }
        public bool IsTransitioning { get; set; }
        public string OnTransitionOffStart { get; set; }
        public string OnTransitionOffEnd { get; set; }
        public string OnTransitionOnStart { get; set; }
        public string OnTransitionOnEnd { get; set; }
        public Transition Transition { get; set; }

        private CMouse mouse = new CMouse();
        private CKeyboard keyboard = new CKeyboard();
        private CGamePad gamePad = new CGamePad();

        private List<Element> elements = new List<Element>();
        private Element currentHoveredElement = null;
        private Element currentSelectedElement = null;
        private Element currentHasFocusElement = null;


        public Page(JSON data)
        {
            Initialize(data);
        }


        public Page(string pageFileLocation)
        {
            IsEnabled = false;
            IsVisible = false;
            IsTransitioning = false;

            string data = FileUtils.FileToString(pageFileLocation);
            JSON json = new JSON(data);
            Initialize(json);
        }


        private void Initialize(JSON json)
        {
            // parse transition
            if (json.Get("transition") != null)
            {
                Transition t = TransitionManager.Instance.GetTransition((string)json.Get("transition"));
                if (t != null)
                {
                    Transition = t.Clone(this);
                }
            }
            if (Transition == null)
            {
                Transition = TransitionManager.Instance.GetDefaultTransition().Clone(this);
            }

            // parse transition events
            OnTransitionOffStart = (string)json.Get("on-transition-off-start");
            OnTransitionOffEnd = (string)json.Get("on-transition-off-end");
            OnTransitionOnStart = (string)json.Get("on-transition-on-start");
            OnTransitionOnEnd = (string)json.Get("on-transition-on-end");

            // parse settings
            JSON settings = new JSON("{}");
            if (json.Get("settings") != null)
            {
                settings = (JSON) json.Get("settings");
            }
            // parse global
            JSON global = new JSON("{}");
            if (json.Get("global") != null)
            {
                global = (JSON)json.Get("global");
            }
            // parse elements
            List<object> elementsJson = (List<object>)json.Get("elements.#");
            foreach (object elemObj in elementsJson)
            {
                JSON elem = (JSON)elemObj;
                elements.Add(ElementFactory.CreateElement(elem, settings, global, null));
            }
            RegisterForMouseEvents();
            RegisterForKeyboardEvents();
            RegisterForGamePadEvents();
        }


        public void Update()
        {
            if (IsTransitioning)
            {
                if (Transition != null)
                {
                    Transition.Update();
                }
            }
            if (IsEnabled)
            {
                mouse.Update();
                keyboard.Update();
                if (gamePad.State.IsConnected)
                {
                    gamePad.Update();
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                foreach (Element rootElement in elements)
                {
                    foreach (Element nodeElement in rootElement)
                    {
                        nodeElement.Draw(spriteBatch);
                    }
                }
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ Page \n");
            foreach (Element element in elements)
            {
                foreach (Element child in element)
                {
                    sb.Append(child.ToString() + "\n");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }


        public List<Element> GetRootElements()
        {
            return elements;
        }


        public List<Element> GetAllElements()
        {
            List<Element> allElems = new List<Element>();
            foreach (Element rootElem in elements)
            {
                foreach (Element element in rootElem)
                {
                    allElems.Add(element);
                }
            }
            return allElems;
        }


        public Element GetElementById(string id)
        {
            foreach (Element elem in GetAllElements())
            {
                if (elem.Id == id)
                {
                    return elem;
                }
            }
            return null;
        }


        #region CMouseEventHandler Members
        private void RegisterForMouseEvents()
        {
            mouse.ClickNotifier.SubscribeToEvent(this, CMouseButton.LEFT, ButtonState.Pressed);
            mouse.ClickNotifier.SubscribeToEvent(this, CMouseButton.LEFT, ButtonState.Released);
            mouse.ClickNotifier.SubscribeToEvent(this, CMouseButton.RIGHT, ButtonState.Pressed);
            mouse.ClickNotifier.SubscribeToEvent(this, CMouseButton.RIGHT, ButtonState.Released);
            mouse.ClickNotifier.SubscribeToEvent(this, CMouseButton.MIDDLE, ButtonState.Pressed);
            mouse.ClickNotifier.SubscribeToEvent(this, CMouseButton.MIDDLE, ButtonState.Released);

            mouse.ScrollNotifier.SubscribeToEvent(this, CMouseScroll.CHANGED);

            mouse.MoveNotifier.SubscribeToEvent(this, CMousePosition.CHANGED);

            mouse.DragNotifier.SubscribeToEvent(this, CMouseButton.LEFT, CMouseDrag.BEGIN);
            mouse.DragNotifier.SubscribeToEvent(this, CMouseButton.LEFT, CMouseDrag.END);
            mouse.DragNotifier.SubscribeToEvent(this, CMouseButton.RIGHT, CMouseDrag.BEGIN);
            mouse.DragNotifier.SubscribeToEvent(this, CMouseButton.RIGHT, CMouseDrag.END);
            mouse.DragNotifier.SubscribeToEvent(this, CMouseButton.MIDDLE, CMouseDrag.BEGIN);
            mouse.DragNotifier.SubscribeToEvent(this, CMouseButton.MIDDLE, CMouseDrag.END);
        }


        void CMouseEventHandler.HandleClickEvent(CMouse mouse, CMouseButton button, ButtonState state)
        {
            if (button == CMouseButton.LEFT)
            {
                if (state == ButtonState.Pressed)
                {
                    SetElementsToNotHaveFocus();
                }
                foreach (Element element in GetAllElements())
                {
                    if (element.IsEnabled)
                    {
                        if (element.IsHover())
                        {
                            if (state == ButtonState.Pressed)
                            {
                                currentHasFocusElement = element;
                                currentHasFocusElement.HasFocus = true;
                                InitiateUIEvent(element, UIEventType.ON_SELECT_START);
                            }
                        }
                        else if (element.IsSelected())
                        {
                            if (state == ButtonState.Released)
                            {
                                InitiateUIEvent(element, UIEventType.ON_SELECT_END);
                                InitiateUIEvent(element, UIEventType.ON_HOVER_START);
                            }
                        }
                    }
                }
            }
        }


        void CMouseEventHandler.HandleScrollEvent(CMouse mouse, float difference)
        {
            
        }


        void CMouseEventHandler.HandleMoveEvent(CMouse mouse, Vector2 position)
        {
            foreach (Element element in GetAllElements())
            {
                if (element.IsEnabled)
                {
                    CRectangle rect = element.GetAbsoluteBoundingBox();
                    if (rect.Contains(position))
                    {
                        if (!element.IsHover() && !element.IsSelected())
                        {
                            InitiateUIEvent(element, UIEventType.ON_HOVER_START);
                        }
                    }
                    else
                    {
                        if (element.IsHover())
                        {
                            InitiateUIEvent(element, UIEventType.ON_HOVER_END);
                        }
                        if (element.IsSelected())
                        {
                            element.NeutralizeEvent();
                        }
                    }
                }
            }
        }


        void CMouseEventHandler.HandleDragEvent(CMouse mouse, CMouseButton button, CMouseDrag state)
        {
            
        }
        #endregion


        #region CKeyboardEventHandler Members
        private void RegisterForKeyboardEvents()
        {
            // register for keyboard events
            keyboard.SubscribeToAllKeyboardEvents(this, ButtonState.Pressed);
        }


        void CKeyboardEventHandler.HandleKeyEvent(CKeyboard keyboard, Keys key, ButtonState state)
        {
            if (currentHasFocusElement != null)
            {
                currentHasFocusElement.SendKeyboardInfoToThisElement(keyboard);
            }
            if (key == Keys.Tab && state == ButtonState.Pressed)
            {
                List<Element> elems = GetAllElements();
                if (currentHoveredElement != null)
                {
                    Element element = currentHoveredElement;
                    int i = elems.IndexOf(element);
                    InitiateUIEvent(element, UIEventType.ON_HOVER_END);
                    element = PickNextEnabledElement(i + 1, elems);
                    InitiateUIEvent(element, UIEventType.ON_HOVER_START);
                }
                else
                {
                    if (elems.Count > 0)
                    {
                        Element element = PickNextEnabledElement(0, elems);
                        InitiateUIEvent(element, UIEventType.ON_HOVER_START);
                    }
                }
            }
            else if ((key == Keys.Enter || key == Keys.Space) && state == ButtonState.Pressed)
            {
                SetElementsToNotHaveFocus();
                if (currentHoveredElement != null)
                {
                    Element element = currentHoveredElement;
                    currentHasFocusElement = element;
                    currentHasFocusElement.HasFocus = true;
                    InitiateUIEvent(element, UIEventType.ON_SELECT_END);
                    InitiateUIEvent(element, UIEventType.ON_HOVER_START);
                }
            }
            else if (key == Keys.Up || key == Keys.W || key == Keys.Down || key == Keys.S || 
                key == Keys.Left || key == Keys.A || key == Keys.Right || key == Keys.D)
            {
                if (currentHoveredElement != null)
                {
                    Vector2 direction = Vector2.Zero;
                    if (key == Keys.Up || key == Keys.W)
                        direction.Y = -1;
                    else if (key == Keys.Down || key == Keys.S)
                        direction.Y = 1;
                    else if (key == Keys.Left || key == Keys.A)
                        direction.X = -1;
                    else if (key == Keys.Right || key == Keys.D)
                        direction.X = 1;
                    Element nextElement = IntersectElementWithPointAndDirection(currentHoveredElement, direction);
                    if (nextElement != null)
                    {
                        InitiateUIEvent(currentHoveredElement, UIEventType.ON_HOVER_END);
                        InitiateUIEvent(nextElement, UIEventType.ON_HOVER_START);
                    }
                }
                else
                {
                    List<Element> elems = GetAllElements();
                    if (elems.Count > 0)
                    {
                        Element element = PickNextEnabledElement(0, elems);
                        InitiateUIEvent(element, UIEventType.ON_HOVER_START);
                    }
                }
            }
        }
        #endregion


        #region CGamePadEventHandler Members
        private void RegisterForGamePadEvents()
        {
            // register for gamepad events
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.A, ButtonState.Pressed);
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.B, ButtonState.Pressed);
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.LSTICKD, ButtonState.Pressed);
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.LSTICKU, ButtonState.Pressed);
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.LSTICKL, ButtonState.Pressed);
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.LSTICKR, ButtonState.Pressed);
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.RSTICKD, ButtonState.Pressed);
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.RSTICKU, ButtonState.Pressed);
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.RSTICKL, ButtonState.Pressed);
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.RSTICKR, ButtonState.Pressed);
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.DPADD, ButtonState.Pressed);
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.DPADU, ButtonState.Pressed);
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.DPADL, ButtonState.Pressed);
            gamePad.ButtonNotifier.SubscribeToEvent(this, CGamePadButton.DPADR, ButtonState.Pressed);
        }


        void CGamePadEventHandler.HandleButtonEvent(CGamePad gamePad, CGamePadButton button, ButtonState state)
        {
            if (button == CGamePadButton.A && state == ButtonState.Pressed)
            {
                SetElementsToNotHaveFocus();
                if (currentHoveredElement != null)
                {
                    Element element = currentHoveredElement;
                    currentHasFocusElement = element;
                    currentHasFocusElement.HasFocus = true;
                    InitiateUIEvent(element, UIEventType.ON_SELECT_END);
                    InitiateUIEvent(element, UIEventType.ON_HOVER_START);
                }
            }
            else if (button == CGamePadButton.LSTICKD || button == CGamePadButton.RSTICKD || button == CGamePadButton.DPADD || 
                button == CGamePadButton.LSTICKU || button == CGamePadButton.RSTICKU || button == CGamePadButton.DPADU || 
                button == CGamePadButton.LSTICKR || button == CGamePadButton.RSTICKR || button == CGamePadButton.DPADR || 
                button == CGamePadButton.LSTICKL || button == CGamePadButton.RSTICKL || button == CGamePadButton.DPADL)
            {
                if (currentHoveredElement != null)
                {
                    Vector2 direction = Vector2.Zero;
                    if (button == CGamePadButton.LSTICKD || button == CGamePadButton.RSTICKD || button == CGamePadButton.DPADD)
                        direction.Y = -1;
                    else if (button == CGamePadButton.LSTICKU || button == CGamePadButton.RSTICKU || button == CGamePadButton.DPADU)
                        direction.Y = 1;
                    else if (button == CGamePadButton.LSTICKR || button == CGamePadButton.RSTICKR || button == CGamePadButton.DPADR)
                        direction.X = 1;
                    else if (button == CGamePadButton.LSTICKL || button == CGamePadButton.RSTICKL || button == CGamePadButton.DPADL)
                        direction.X = -1;
                    Element nextElement = IntersectElementWithPointAndDirection(currentHoveredElement, direction);
                    if (nextElement != null)
                    {
                        InitiateUIEvent(currentHoveredElement, UIEventType.ON_HOVER_END);
                        InitiateUIEvent(nextElement, UIEventType.ON_HOVER_START);
                    }
                }
                else
                {
                    List<Element> elems = GetAllElements();
                    if (elems.Count > 0)
                    {
                        Element element = PickNextEnabledElement(0, elems);
                        InitiateUIEvent(element, UIEventType.ON_HOVER_START);
                    }
                }
            }
        }


        void CGamePadEventHandler.HandleTriggerEvent(CGamePad gamePad, CGamePadTrigger trigger, float state)
        {
            // probably don't need to worry about trigger events
        }


        void CGamePadEventHandler.HandleStickEvent(CGamePad gamePad, CGamePadStick stick, Vector2 state)
        {
            // will use the stick button events instead of the stick change events
        }
        #endregion


        #region Helpers
        private Element IntersectElementWithPointAndDirection(Element originElement, Vector2 direction)
        {
            CRectangle originRect = originElement.GetAbsoluteBoundingBox();
            Vector2 origin = new Vector2(originRect.X, originRect.Y);
            direction.Normalize();
            Vector2 dirMax = VectorMath.RotatePointAroundZero(direction, MathHelper.ToRadians(46));
            dirMax *= 10000;
            dirMax += origin;
            Vector2 dirMin = VectorMath.RotatePointAroundZero(direction, MathHelper.ToRadians(-46));
            dirMin *= 10000;
            dirMin += origin;

            float closestDistance = 10000;
            Element closestElement = null;
            foreach (Element element in GetAllElements())
            {
                if (!element.Equals(originElement) && element.IsEnabled)
                {
                    CRectangle rect = element.GetAbsoluteBoundingBox();
                    Vector2 pos = new Vector2(rect.X, rect.Y);
                    if (VectorMath.IsPointInTriangle(origin, dirMax, dirMin, pos))
                    {
                        float dist = Vector2.Distance(origin, pos);
                        if (dist < closestDistance && dist != 0)
                        {
                            closestDistance = dist;
                            closestElement = element;
                        }
                    }
                }
            }
            return closestElement;
        }


        private Element PickNextEnabledElement(int startIndex, List<Element> elems)
        {
            int index = startIndex;
            for (int i = 0; i < elems.Count; i++)
            {
                if (index >= elems.Count)
                {
                    index -= elems.Count;
                }
                else if (index < 0)
                {
                    index += elems.Count;
                }
                if (elems[index] != null && elems[index].IsEnabled)
                {
                    return elems[index];
                }
                else
                {
                    index++;
                }
            }
            return null;
        }


        private void SetElementsToNotHaveFocus()
        {
            currentHasFocusElement = null;
            foreach (Element elem in GetAllElements())
            {
                elem.HasFocus = false;
            }
        }


        private void InitiateUIEvent(Element element, UIEventType type)
        {
            if (element != null)
            {
                switch (type)
                {
                    case UIEventType.ON_HOVER_START:
                        currentHoveredElement = element;
                        element.OnHoverStartEvent();
                        if (element.OnHoverStartEventName != null)
                        {
                            // notify subscribers that OnHoverStart event happend
                            PageManager.Instance.NotifySubscribers(element, Name + "." + element.OnHoverStartEventName);
                        }
                        break;

                    case UIEventType.ON_HOVER_END:
                        currentHoveredElement = null;
                        element.OnHoverEndEvent();
                        if (element.OnHoverEndEventName != null)
                        {
                            // notify subscribers that OnHoverEnd event happend
                            PageManager.Instance.NotifySubscribers(element, Name + "." + element.OnHoverEndEventName);
                        }
                        break;

                    case UIEventType.ON_SELECT_START:
                        currentSelectedElement = element;
                        element.OnSelectStartEvent();
                        if (element.OnSelectStartEventName != null)
                        {
                            // notify subscribers that OnSelectStart event happend
                            PageManager.Instance.NotifySubscribers(element, Name + "." + element.OnSelectStartEventName);
                        }
                        break;

                    case UIEventType.ON_SELECT_END:
                        currentSelectedElement = null;
                        element.OnSelectEndEvent();
                        if (element.OnSelectEndEventName != null)
                        {
                            // notify subscribers that OnSelectEnd event happend
                            PageManager.Instance.NotifySubscribers(element, Name + "." + element.OnSelectEndEventName);
                        }

                        // transition to a different page if transition on select is set
                        if (element.TransitionOnSelect != null)
                        {
                            element.NeutralizeEvent();
                            PageManager.Instance.TransitionToPage(element.TransitionOnSelect);
                        }
                        break;
                }
            }
        }
        #endregion
    }


    public interface PageEventHandler
    {
        void HandleEvent(object sender, string identifier);
    }
}
