using FormationManager.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormationManager
{
    public static class ModifierUtil
    {
        //// 阵型种类
        //public readonly static string[] formationCN = {
        //    "散开阵",
        //    "一字阵",
        //    "冲锋阵",
        //    "施工中",
        //    "施工中",
        //    "施工中",
        //    "施工中"
        //};
        //public readonly static string[] formationPY = {
        //    "sankaizhen",
        //    "yizizhen",
        //    "chongfangzhen",
        //    "xxxx3",
        //    "xxxx4",
        //    "xxxx5",
        //    "xxxx6"
        //};
        public static string ModifierNameCN = "散开阵";    
        public static string ModifierNamePY = "sankaizhen";// 散开阵
        //// 施工了多少个阵法
        //internal static int successNum = 3;

        // 我方和敌人是否组成阵型{我方，敌人}
        public static bool[] enableModifier = new bool[] { false, false };

        /// 设置ModifierInfo
        /// </summary>
        /// <param name="nameCN">中文名</param>
        /// <param name="namePY">拼音</param>
        /// <param name="modifier">要配置的buff</param>
        /// <param name="IntValue">整数字符串</param>
        public static void setModifierInfo(ModifierInfo modifier, string nameCN, string namePY, FormationPosition formationPosition)
        {
            // 设置为战斗后移除
            modifier.flags = ModifierFlags.RemoveAfterCombat;
            // 设置鼠标移上去时的描述
            LanguageData mLanguageData = modifier.GetLanguageData();
            mLanguageData.SetText("Name", nameCN);
            mLanguageData.SetText("Desc", nameCN + "施展中");
            mLanguageData.ID = "martial-" + namePY;
            //foreach (KeyValuePair<string, string> kv in mLanguageData.Map)
            //{
            //    FormationMod.logger.Log($"LanguageData key:{kv.Key}value:{kv.Value}");
            //}
            List<AdditionalProperty> mProperties = modifier.Properties;
            // 清楚旧的属性
            mProperties.Clear();

            for (int i = 0; i < formationPosition.Buffs.Count; i++)
            {
                PositionBuff Buffs = formationPosition.Buffs[i];
                // 添加属性为参数值
                VariableValue variableValue = new VariableValue();
                variableValue.Type = VariableValueType.Value;
                variableValue.value = Buffs.Value;
                // 新的属性
                AdditionalProperty newAdditionalProperty = new AdditionalProperty();
                newAdditionalProperty.IsFactor = true;
                //newAdditionalProperty.IsFactor = false;
                newAdditionalProperty.effectType = PropertyEffectType.Add;
                newAdditionalProperty.property = Buffs.BuffType;
                newAdditionalProperty.Value = variableValue;

                // 添加属性
                mProperties.Add(newAdditionalProperty);
            }


            //foreach (AdditionalProperty additionalProperty in mProperties)
            //{
            //    FormationMod.logger.Log($"additionalProperty IsFactor:{additionalProperty.IsFactor}|effectType:{additionalProperty.effectType}|property:{additionalProperty.property}");
            //    FormationMod.logger.Log($"additionalProperty VariableValueType: { additionalProperty.Value.Type}|value: { additionalProperty.Value.value}");
            //}
        }
        // 退出战斗时还原是否组成阵型的状态
        internal static void resetEnableStatus()
        {
            enableModifier = new bool[] { false, false };
        }
    }
}
