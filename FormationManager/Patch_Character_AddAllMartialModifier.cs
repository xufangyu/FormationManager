using Harmony12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormationManager
{
    //[HarmonyPatch(typeof(Character), "AddAllMartialModifier")]
    //public class Character_AddAllMartialModifier_Patch
    //{
    //    [HarmonyPrefix]
    //    public static bool AddAllMartialModifier_Path(Character __instance)
    //    {
    //        // 如果MOD状态是关闭的话
    //        if (!FormationMod.enable)
    //        {
    //            // 执行原本的方法
    //            return true;
    //        }
    //        foreach (KeyValuePair<int, string> keyValuePair in __instance.EquipMartials)
    //        {
    //            __instance.AddMartialModifier(keyValuePair.Key, keyValuePair.Value);
    //        }
    //        // 判断时玩家还是NPC
    //        //if (__instance.IsPlayer())
    //        //{
    //        //    // 判断是否组成阵型
    //        //    if (ModifierUtil.enableModifier[0])
    //        //    {
    //        //        __instance.AddMartialModifier(0, ModifierUtil.ModifierNamePY);
    //        //    }
    //        //} else
    //        //{
    //        //    // 判断是否组成阵型
    //        //    if (ModifierUtil.enableModifier[1])
    //        //    {
    //        //        __instance.AddMartialModifier(0, ModifierUtil.ModifierNamePY);
    //        //    }
    //        //}
    //        //FormationMod.logger.Log($"cha:{ModifierUtil.ModifierNameCN}");
    //        // 返回false表示不执行原方法
    //        return false;
    //    }
    //}

    [HarmonyPatch(typeof(Character), "AddMartialModifier", new Type[] { typeof(int), typeof(string) })]
    public class Character_AddMartialModifier2_Patch
    {
        [HarmonyPrefix]
        public static bool AddMartialModifier_Path(Character __instance, int skillType, string martialSkillID)
        {
            // 如果MOD状态是关闭的话
            if (!FormationMod.enable)
            {
                // 执行原本的方法
                return true;
            }
            //FormationMod.logger.Log($"AddMartialModifier:{martialSkillID}");
            MartialSkillInfo skillInfoByID = SingletonMonoBehaviour<MartialSkillModel>.Instance.GetSkillInfoByID(martialSkillID);
            __instance.AddMartialModifier(skillType, skillInfoByID);

            // 返回false表示不执行原方法
            return false;
        }
    }
    [HarmonyPatch(typeof(Character), "AddMartialModifier", new Type[] { typeof(int), typeof(MartialSkillInfo) })]
    public class Character_AddMartialModifier_Patch
    {
        [HarmonyPrefix]
        public static bool AddMartialModifier_Path(Character __instance, int skillType, MartialSkillInfo martialSkillInfo)
        {
            // 如果MOD状态是关闭的话
            if (!FormationMod.enable)
            {
                // 执行原本的方法
                return true;
            }
            MartialSkillTypeInfo skillTypeInfo = SingletonMonoBehaviour<MartialSkillModel>.Instance.GetSkillTypeInfo(skillType);
            bool flag = skillTypeInfo.IsAttackType && __instance.GetAtkMartialSkillType() != skillType;
            //FormationMod.logger.Log($"cha:{__instance.GetName()}martialSkillInfo.ID:{martialSkillInfo.ID} flag:{flag} ");
            if (!flag)
            {
                bool flag2 = martialSkillInfo.PassiveMovesListMap != null;
                //FormationMod.logger.Log($"cha:{__instance.GetName()}martialSkillInfo.ID:{martialSkillInfo.ID} flag2:{flag2} ");
                if (flag2)
                {
                    List<int> list;
                    bool flag3 = martialSkillInfo.PassiveMovesListMap.TryGetValue(skillType, out list);
                    //FormationMod.logger.Log($"cha:{__instance.GetName()}martialSkillInfo.ID:{martialSkillInfo.ID} flag3:{flag3} ");
                    if (flag3)
                    {
                        foreach (int movesID in list)
                        {
                            MartialMovesInfo movesInfo = martialSkillInfo.GetMovesInfo(movesID);
                            bool flag4 = movesInfo.PerformInfo.PerformModifiers.Count > 0;
                            //FormationMod.logger.Log($"cha:{__instance.GetName()}ModifierID:{movesInfo.PerformInfo.PerformModifiers[0].ModifierID} flag4:{flag4} ");
                            if (flag4)
                            {
                                __instance.AddModifier(movesInfo.PerformInfo.PerformModifiers[0].ModifierID, -1);
                            }
                        }
                    }
                }
            }

            // 返回false表示不执行原方法
            return false;
        }
    }
}
