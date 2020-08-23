using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FormationManager
{
    class FormationSettingPanel : MonoBehaviour
    {


        private const float designWidth = 1600f, designHeight = 900f;

        public static FormationSettingPanel Instance { get; private set; }
        // 是否进行过初始化
        private static bool isInit = false;

        private static PanelMain panelMain;
        private static GameObject obj;

        GameObject canvas;

        Rect windowRect;
        Vector2 scrollPosition;

        public bool Open { get; private set; }
        bool cursorLock;
        bool collapse;
        bool[] showItems;

        GUIStyle windowStyle, collapseStyle, seperatorStyle, seperatorStyle2;

        //DateFile df => DateFile.instance;
        //WorldMapSystem wms => WorldMapSystem.instance;

        public static bool Load()
        {
            try
            {
                if (obj == null)
                {
                    obj = new GameObject("FormationSetting", typeof(FormationSettingPanel));
                    DontDestroyOnLoad(obj);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void Awake()
        {
            //FormationMod.logger.Log("awake");
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public void Start()
        {
            //FormationMod.logger.Log("start");

            Open = false;
            collapse = true;

            showItems = new bool[] { true, true, true, true, true, true };
            scrollPosition = Vector2.zero;
            //windowRect = new Rect(designWidth * 0.85f, designHeight * 0.05f, designWidth * 0.145f, 0);
            windowRect = new Rect(designWidth * 0.60f, designHeight * 0.02f, designWidth * 0.145f, 0);

            Init();

            ToggleWindow();
        }

        public void Init()
        {
            // 设置点击事件
            //var dateText = ReflectionMethod.GetValue<PanelMain, TextMeshProUGUI>(PanelMain_Awake_Patch.panelMain, "DateText");
            //var btn = dateText.transform.parent.parent.gameObject.AddComponent<Button>();
            if (!isInit)
            {
                panelMain = SingletonMonoBehaviour<PanelMain>.Instance;

                //btn.targetGraphic = dateText;
                try
                {
                    Button btn = panelMain.gameObject.AddComponent<Button>();
                    btn.interactable = true;
                    btn.onClick.AddListener(ToggleWindow);
                }
                catch (Exception)
                {
                    // 未初始化完毕时，类型不一样防止报错（NullReferenceException: object reference not set to an instance of an object）
                    //FormationMod.logger.Log("FormationMod Init End");
                }
                isInit = true;
            }
        }
        private void PrepareGUI()
        {
            windowStyle = new GUIStyle
            {
                name = "window",
                padding = new RectOffset(5, 5, 5, 5),
            };
            collapseStyle = new GUIStyle(GUI.skin.button);
            collapseStyle.name = "collapse";
            // fontSize = 12,
            collapseStyle.margin = new RectOffset(0, 0, 0, 0);
            collapseStyle.alignment = TextAnchor.MiddleRight;
            collapseStyle.fixedWidth = 50f;
            collapseStyle.fixedHeight = 25f;
            //collapseStyle.fontSize = 14;
            collapseStyle.normal.textColor = Color.red;

            //buttonStyle = new GUIStyle(GUI.skin.button);
            //buttonStyle.name = "button";
            //buttonStyle.margin = new RectOffset(0, 0, 0, 0);
            //buttonStyle.alignment = TextAnchor.MiddleCenter;
            //buttonStyle.fontSize = FormationMod.settings.buttonSize;
            //buttonStyle.normal.textColor = Color.yellow;

            seperatorStyle = new GUIStyle(GUI.skin.button);
            seperatorStyle.name = "seperator";
            seperatorStyle.margin = new RectOffset(0, 0, 0, 0);
            seperatorStyle.alignment = TextAnchor.MiddleCenter;
            seperatorStyle.fontSize = FormationMod.settings.buttonSize - 1;
            seperatorStyle.normal.textColor = Color.cyan;

            seperatorStyle2 = new GUIStyle(GUI.skin.button);
            seperatorStyle2.name = "seperator";
            seperatorStyle2.margin = new RectOffset(0, 0, 0, 0);
            seperatorStyle2.alignment = TextAnchor.MiddleCenter;
            seperatorStyle2.fontSize = FormationMod.settings.buttonSize - 1;
            seperatorStyle2.normal.textColor = Color.red;
        }

        public void OnGUI()
        {
            if (Open)
            {
                PrepareGUI();

                var bgColor = GUI.backgroundColor;
                var color = GUI.color;

                Matrix4x4 svMat = GUI.matrix;
                Vector2 resizeRatio = new Vector2((float)Screen.width / designWidth, (float)Screen.height / designHeight);
                GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));

                GUI.backgroundColor = Color.black;
                GUI.color = Color.white;

                windowRect = GUILayout.Window(666, windowRect, WindowFunc, "",
                    windowStyle, GUILayout.Height(designHeight * 0.73f));

                GUI.matrix = svMat;
                GUI.backgroundColor = bgColor;
                GUI.color = color;
            }
        }

        private void WindowFunc(int windowId)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(designWidth * 0.145f - 30f);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("阵型", collapseStyle))
            {
                //FormationMod.logger.Log(collapse ? "展" : "收");
                collapse = !collapse;
            }
            GUILayout.EndHorizontal();

            if (!collapse)
            {
                canvas.transform.Find("panel").GetComponent<RectTransform>().anchorMin = new Vector2(windowRect.x / designWidth, 0.78f);

                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false,
                    GUILayout.Width(windowRect.width - 20), GUILayout.MaxHeight(designHeight * 0.73f));
                GUILayout.BeginVertical();
                for (int i = 0; i < FormationFileLoad.Size; i++)
                {
                    // 点击后切换不同样式，展示出不同颜色
                    if (GUILayout.Button(FormationFileLoad.fiList[i].NameCN, (showItems[i] ? seperatorStyle : seperatorStyle2)))
                        //if (GUILayout.Button(ModifierUtil.formationCN[i], (showItems[i] ? seperatorStyle : seperatorStyle2)))
                        {
                        // 暂时实现2个阵型
                        //if (i > ModifierUtil.successNum - 1)
                        //{
                        //    return;
                        //}
                        // 点击后收起
                        collapse = !collapse;
                        ModifierUtil.ModifierNamePY = FormationFileLoad.fiList[i].Name;
                        ModifierUtil.ModifierNameCN = FormationFileLoad.fiList[i].NameCN;
                        // 点击后其他按钮回复颜色，实现单一选项
                        showItems = new bool[] { true, true, true, true, true, true };
                        showItems[i] = !showItems[i];
                    }
                }
                GUILayout.EndVertical();
                GUILayout.EndScrollView();
            }
            else
            {
                canvas.transform.Find("panel").GetComponent<RectTransform>().anchorMin = new Vector2(1220f / designWidth, 1 - 21f / designHeight);
                //FormationMod.logger.Log($" flaotX: {1f - 25f / designWidth}  flaotY: {0.95f - 25f / designHeight}");
            }
        }

        public void Update()
        {
            if (!FormationMod.enable)
            {
                Destroy(canvas);
                Open = false;
            }
            if (Open)
            {
                if (Input.GetKey(KeyCode.PageUp))
                {
                    scrollPosition.y -= 40;
                }
                if (Input.GetKey(KeyCode.PageDown))
                {
                    scrollPosition.y += 40;
                }
            }
            if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
                && Input.GetKeyUp(FormationMod.settings.key))
            {
                ToggleWindow();
            }
        }

        /// <summary>
        /// 切换窗体显示状态
        /// </summary>
        public void ToggleWindow()
        {
            //FormationMod.logger.Log($"Toggle {(!Open ? "on" : "off")}");
            Open = !Open;
            BlockGameUI(Open);
            if (Open)
            {
                scrollPosition = Vector2.zero;
                cursorLock = Cursor.lockState == CursorLockMode.Locked || !Cursor.visible;
                if (cursorLock)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
            else
            {
                if (cursorLock)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }

        /// <summary>
        /// 挡住游戏的UI
        /// </summary>
        /// <param name="open"></param>
        private void BlockGameUI(bool open)
        {
            if (open)
            {
                canvas = new GameObject("canvas", typeof(Canvas), typeof(GraphicRaycaster));
                canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.GetComponent<Canvas>().sortingOrder = short.MaxValue;
                DontDestroyOnLoad(canvas);
                var panel = new GameObject("panel", typeof(Image));
                panel.transform.SetParent(canvas.transform);
                panel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.4f);
                //panel.GetComponent<RectTransform>().anchorMin = new Vector2(1f - 25f / designWidth, 0.95f - 25f / designHeight);
                //panel.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.95f);
                panel.GetComponent<RectTransform>().anchorMin = new Vector2(1220f / designWidth, 1 - 21f / designHeight);
                panel.GetComponent<RectTransform>().anchorMax = new Vector2(1220f / designWidth, 1 - 21f / designHeight);
                panel.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                panel.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            }
            else
            {
                Destroy(canvas);
            }
        }

    }
}
