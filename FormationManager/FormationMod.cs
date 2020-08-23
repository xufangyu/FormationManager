using Harmony12;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityModManagerNet;

namespace FormationManager
{
    // 使配置可以保存
    public class Settings : UnityModManager.ModSettings
    {
        [XmlIgnore]
        public int buttonSize = 18;
        /// <summary>
        /// 热键 ctrl + ？
        /// </summary>
        public KeyCode key = KeyCode.F9;
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
    public class FormationMod
    {
        public static bool enable;
        public static Settings settings;
        public static UnityModManager.ModEntry.ModLogger logger;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            // 读取用户的配置文件
            settings = Settings.Load<Settings>(modEntry);
            // 存储日志输出对象方便以后使用
            logger = modEntry.Logger;

            // 当用户打开MOD配置的时候调用
            modEntry.OnGUI = OnGUI;
            // 当用户保存MOD配置的时候调用
            modEntry.OnSaveGUI = OnSaveGUI;
            // 当用户开关MOD的时候调用
            modEntry.OnToggle = OnToggle;

            // 读取阵型文件中的阵型信息
            FormationFileLoad.Load();


            // 将更改应用到游戏上
            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }
        /// <summary>
        /// 开关MOD的时候调用
        /// </summary>
        /// <param name="arg1">MOD配置对象</param>
        /// <param name="arg2">用户世要打开还是关闭MOD</param>
        /// <returns>切换状态是否成功</returns>
        private static bool OnToggle(UnityModManager.ModEntry arg1, bool arg2)
        {
            // 当前开启状态变为用户想要的状态
            enable = arg2;
            return true;
            //throw new NotImplementedException();
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="obj">MOD的配置对象</param>
        private static void OnSaveGUI(UnityModManager.ModEntry obj)
        {
            settings.Save(obj);
            //throw new NotImplementedException();
        }
        /// <summary>
        /// 当用户打开MOD配置的时候
        /// </summary>
        /// <param name="obj"></param>
        private static void OnGUI(UnityModManager.ModEntry obj)
        {
            GUIStyle tipsStyle = new GUIStyle(GUI.skin.label);
            tipsStyle.name = "tipsStyle";
            tipsStyle.margin = new RectOffset(0, 0, 0, 0);
            tipsStyle.alignment = TextAnchor.MiddleLeft;
            tipsStyle.fontSize = FormationMod.settings.buttonSize - 1;
            tipsStyle.normal.textColor = Color.cyan;
            GUILayout.BeginVertical("Box");
            {
                //GUILayout.Label("基础资源框架：");
                //GUILayout.Label($"<color=#F28234>当前的游戏根目录:{Environment.CurrentDirectory}</color>");
                // 基本设置
                GUILayout.BeginVertical("Box");
                {
                    GUILayout.Toggle(true, $"<color=#8FBAE7>使用阵型mod的说明</color>", tipsStyle);
                    GUILayout.Label($"<color=#F28234>注意：</color> 只有在<color=#F28234>点击攻击</color>之前选择阵型才会生效，,进入<color=#F28234>准备战斗</color>之后再选择不会改变buff，只改变站位", tipsStyle);
                    GUILayout.Label($"快捷键位<color=#F28234>Ctrl+F9</color>，进行唤出/隐藏", tipsStyle);
                    GUILayout.Label($"");
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }
    }

    [HarmonyPatch(typeof(PanelMainCity), "Awake")]
    public class PanelMainCity_Awake_Patch
    {

        //public static PanelMain panelMain { get; private set; }
        [HarmonyPostfix]
        public static void Awake_Path()
        {

            //判断是否已经实例化
            if (FormationSettingPanel.Instance == null)
            {
                //FormationMod.logger.Log($"create ui: {FormationSettingPanel.Load()}");
                FormationSettingPanel.Load();
            }
            else
            {
                FormationSettingPanel.Instance.Init();
                if (!FormationSettingPanel.Instance.Open)
                {
                    FormationSettingPanel.Instance.ToggleWindow();
                }
            }
        }
    }

    //[HarmonyPatch(typeof(BattleManager), "GetScenePosByEntityPos")]
    //public class BattleManager_GetScenePosByEntityPos_Patch
    //{
    //    [HarmonyPrefix]
    //    public static bool GetScenePosByEntityPos_Path(ref Vector3 __result, BattleManager __instance, int team, int idx)
    //    {
    //        // 如果MOD状态是关闭的话
    //        if (!FormationMod.enable)
    //        {
    //            // 执行原本的方法
    //            return true;
    //        }

    //        // 返回false表示不执行原方法
    //        return false;
    //    }
    //}
}
