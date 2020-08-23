using Harmony12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FormationManager
{
    [HarmonyPatch(typeof(PanelBattle), "UpdateChrInfos")]
    public class PanelBattle_UpdateChrInfos_Patch
    {
        [HarmonyPrefix]
        public static bool UpdateChrInfos_Path(PanelBattle __instance)
        {
            // 如果MOD状态是关闭的话
            if (!FormationMod.enable)
            {
                // 执行原本的方法
                return true;
            }
            for (BattleTeam battleTeam = BattleTeam.TEAM_PLAYER; battleTeam < BattleTeam.Count; battleTeam++)
            {
                List<BattleEntity> list = ListPool<BattleEntity>.Get();
                BattleManager.Instance.GetEntityList((int)battleTeam, list);
                for (int i = 0; i < list.Count; i++)
                {
                    BattleEntity battleEntity = list[i];
                    int id = battleEntity.ID;
                    TransformContainer chrUI = __instance.GetChrUI(id);
                    bool flag = chrUI != null;
                    if (flag)
                    {
                        TransformContainer billboard = __instance.GetBillboard(id);
                        Vector3 transPostion = BattleManager.Instance.GetTransPostion(id);
                        Vector3 a = __instance.WorldToUIPosition(transPostion);
                        // 放到身后
                        int directFlag = battleTeam == BattleTeam.TEAM_PLAYER ? -1 : 1;
                        a.x = directFlag * 800f;
                        billboard.rectTransform.anchoredPosition = a + new Vector3(0f, 0f, 0f);
                        float num = (float)battleEntity.mChr.GetMaxValue(CharacterPropertyType.HP);
                        float num2 = (float)battleEntity.mChr.GetCurrentValue(CharacterPropertyType.HP);
                        float num3 = num2 / num;
                        float num4 = (float)battleEntity.mChr.GetEffectiveMaxValue(CharacterPropertyType.HP);
                        float fillAmount = num4 / num;
                        float num5 = (float)battleEntity.mChr.GetMaxValue(CharacterPropertyType.MP);
                        float num6 = (float)battleEntity.mChr.GetCurrentValue(CharacterPropertyType.MP);
                        float value = num6 / num5;
                        billboard["HPSlider"].Slider.value = num3;
                        chrUI["MaxHPImage"].Image.fillAmount = fillAmount;
                        chrUI["HPImage"].Image.fillAmount = num3;
                        using (Zstring.Block())
                        {
                            Zstring value2 = Zstring.Format("{0}/{1}", num2, num);
                            chrUI["HPText"].TextMeshProUGUI.SetText(value2);
                        }
                        billboard["MPSlider"].Slider.value = value;
                        chrUI["MPSlider"].Slider.value = value;
                        using (Zstring.Block())
                        {
                            Zstring value3 = Zstring.Format("{0}/{1}", num6, num5);
                            chrUI["MPText"].TextMeshProUGUI.SetText(value3);
                        }
                        int spmax = SingletonMonoBehaviour<BattleModel>.Instance.mGameData.SPMax;
                        float value4 = battleEntity.SP / (float)spmax;
                        chrUI["SPSlider"].Slider.value = value4;
                        chrUI["SPText"].TextMeshProUGUI.SetText("{0}/{1}", (float)((int)battleEntity.SP), (float)spmax);
                        chrUI["SpeedSlider"].Slider.value = battleEntity.SpeedValue;
                    }
                }
                ListPool<BattleEntity>.Recycle(list);
            }

            // 返回false表示不执行原方法
            return false;
        }
    }
}
