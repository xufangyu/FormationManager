using Harmony12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormationManager
{
    [HarmonyPatch(typeof(MartialSkillModel), "LoadAllSkillInfo")]
    public class MartialSkillModel_LoadAllSkillInfo_Patch
    {
        [HarmonyPrefix]
        public static bool LoadAllSkillInfo_Path(MartialSkillModel __instance)
        {
            // 如果MOD状态是关闭的话
            if (!FormationMod.enable)
            {
                // 执行原本的方法
                return true;
            }
            List<string> list = ListPool<string>.Get();
            Singleton<DataManager>.Instance.GetAllDirName(ref list, "Martial/MartialInfo/", DataType.Game, -1);
            for (int i = 0; i < list.Count; i++)
            {
                string key = list[i];
                MartialSkillInfo martialSkillInfo = Traverse.Create(__instance).Method("LoadSkillInfoByID", key).GetValue<MartialSkillInfo>();
                //FormationMod.logger.Log(JsonUtility.ToJson(martialSkillInfo));
            }
            ListPool<string>.Recycle(list);

            Dictionary<string, MartialSkillInfo> mMartialInfoMap = Traverse.Create(__instance).Field("mMartialInfoMap").GetValue<Dictionary<string, MartialSkillInfo>>();
            // 自定义功法,从八卦心法clone出来
            MartialSkillInfo oldMartialSkillInfo = __instance.GetSkillInfoByID("baguaxinfa");

            for (int i = 0; i < FormationFileLoad.Size; i++)
            {
                FormationInfo formationInfo = FormationFileLoad.fiList[i];
                for (int i2 = 0; i2 < formationInfo.Positions.Count; i2++)
                {
                    MartialSkillInfo newMartialSkillInfo = __instance.CloneMartial(oldMartialSkillInfo, formationInfo.Name + i2);
                    Dictionary<int, MartialMovesInfo> mMovesInfoMap = newMartialSkillInfo.GetAllMovesInfoMap();
                    // 将buff的id改成自定义的buff
                    mMovesInfoMap[2].UpdateModifierID("martial-baguaxinfa", "martial-" + formationInfo.Name + i2);
                    //FormationMod.logger.Log("martial-" + formationInfo.Name + i2);
                }
                //MartialSkillInfo newMartialSkillInfo = __instance.CloneMartial(oldMartialSkillInfo, formationInfo.Name);
                //Dictionary<int, MartialMovesInfo> mMovesInfoMap = newMartialSkillInfo.GetAllMovesInfoMap();
                //// 将buff的id改成自定义的buff
                //mMovesInfoMap[2].UpdateModifierID("martial-baguaxinfa", "martial-" + formationInfo.Name);

            }

            // 返回false表示不执行原方法
            return false;
        }
    }
}
