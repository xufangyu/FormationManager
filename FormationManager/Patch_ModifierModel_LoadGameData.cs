using Harmony12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FormationManager
{
    [HarmonyPatch(typeof(ModifierModel), "LoadGameData")]
    public class ModifierModel_LoadGameData_Patch
    {
        [HarmonyPrefix]
        public static bool LoadGameData_Path(ModifierModel __instance)
        {
            // 如果MOD状态是关闭的话
            if (!FormationMod.enable)
            {
                // 执行原本的方法
                return true;
            }
            __instance.mGameData = Singleton<DataManager>.Instance.Load<ModifierGameData>(__instance.mName, DataType.Game, null);
            bool flag = __instance.mGameData == null;
            if (flag)
            {
                //__instance.mLogService.LogError(__instance.mName + "Model 游戏数据加载失败");
                ILogService mLogService = Traverse.Create(__instance).Field("mLogService").GetValue<ILogService>();
                mLogService.LogError(__instance.mName + "Model 游戏数据加载失败");
            }
            else
            {
                List<string> list = ListPool<string>.Get();
                Singleton<DataManager>.Instance.GetAllDirName(ref list, "ModifierInfo/", DataType.Game, -1);
                for (int i = 0; i < list.Count; i++)
                {
                    string key = list[i];
                    ModifierInfo modifierInfo = __instance.LoadModifierInfoByID(key);
                }
                ListPool<string>.Recycle(list);
                //FormationMod.logger.Log($" init ModifierInfoMap");
                for (int i = 0; i < FormationFileLoad.Size; i++)
                {
                    FormationInfo formationInfo = FormationFileLoad.fiList[i];
                    for (int i2 = 0; i2 < formationInfo.Positions.Count; i2++)
                    {
                        ModifierInfo newModifierInfo_0 = __instance.CloneModifierInfo(__instance.ModifierInfoMap["martial-baguaxinfa"], "martial-" + formationInfo.Name + i2);
                        // 需要初始化所有的阵法buff,每个阵型每个位置的buff不同
                        ModifierUtil.setModifierInfo(newModifierInfo_0, formationInfo.NameCN, formationInfo.Name, formationInfo.Positions[i2]);
                    }
                }
                //ModifierInfo newModifierInfo_1 = __instance.CloneModifierInfo(__instance.ModifierInfoMap["martial-baguaxinfa"], "martial-" + ModifierUtil.formationPY[1]);
                //ModifierUtil.setModifierInfo(newModifierInfo_1, ModifierUtil.formationCN[1], ModifierUtil.formationPY[1], "10");
            }
            // 返回false表示不执行原方法
            return false;
        }


    }
}
