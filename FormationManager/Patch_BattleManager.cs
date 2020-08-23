using Harmony12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FormationManager
{
    [HarmonyPatch(typeof(BattleManager), "EnterBattle")]
    public class BattleManager_EnterBattle_Patch
    {
        public static AudioClip BGMLoop = null;
        [HarmonyPrefix]
        public static bool EnterBattle_Path(BattleManager __instance, BattleType battleType, List<int> playerChrIDList)
        {
            // 如果MOD状态是关闭的话
            if (!FormationMod.enable)
            {
                // 执行原本的方法
                return true;
            }
            __instance.mAutoBattle = false;
            __instance.IsOver = false;
            TimeModel instance = SingletonMonoBehaviour<TimeModel>.Instance;
            int timeStop = instance.TimeStop;
            instance.TimeStop = timeStop + 1;
            GameTempValue.GameSpeedHotKeyLock++;
            GameTempValue.MapMoveHotKeyLock++;
            GameTempValue.SamllMapKeyLock++;
            GameTempValue.TapKeyLock++;
            GameTempValue.BackpackKeyLock++;
            //__instance.mBattleEntityIDMap.Clear();
            Dictionary<int, int> mBattleEntityIDMap = Traverse.Create(__instance).Field("mBattleEntityIDMap").GetValue<Dictionary<int, int>>();
            mBattleEntityIDMap.Clear();

            __instance.Show();
            // battle类型无法直接赋值
            Traverse mBattleTypeTraverse = Traverse.Create(__instance).Property("BattleType");
            mBattleTypeTraverse.SetValue(battleType);
            bool flag = battleType != BattleType.Invade;
            if (flag)
            {
                SingletonMonoBehaviour<MapModel>.Instance.AddState(MapPlayerState.Battle);
            }
            List<int> list;
            using (ListPool<int>.Block(out list))
            {
                foreach (KeyValuePair<int, string> keyValuePair in __instance.mEnemyTeamMap)
                {
                    int key = keyValuePair.Key;
                    list.Add(key);
                    // 不在这里进行buff添加
                    //Character chrByID = SingletonMonoBehaviour<CharacterModel>.Instance.GetChrByID(key);
                    //chrByID.AddAllMartialModifier();
                }
                __instance.PrepareBattleEntities(list, 1);
            }
            List<BattleEntity> list2;
            using (ListPool<BattleEntity>.Block(out list2))
            {
                __instance.GetEntityList(1, list2);
                for (int i = 0; i < list2.Count; i++)
                {
                    BattleEntityView battleEntityView = __instance.AddEntityView(list2[i], EntityForward.Left);
                    battleEntityView.transform.localPosition = __instance.GetScenePosByEntityPos(1, i);
                }
            }
            __instance.mPlayerTeamChrIDList.Clear();
            foreach (int num in playerChrIDList)
            {
                Character chrByID2 = SingletonMonoBehaviour<CharacterModel>.Instance.GetChrByID(num);
                bool flag2 = chrByID2.IsPlayer();
                if (flag2)
                {
                    __instance.mPlayerTeamChrIDList.Add(num);
                }
            }
            __instance.PrepareBattleEntities(playerChrIDList, 0);
            List<BattleEntity> list3 = ListPool<BattleEntity>.Get();
            __instance.GetEntityList(0, list3);
            for (int j = 0; j < list3.Count; j++)
            {
                BattleEntityView battleEntityView2 = __instance.AddEntityView(list3[j], EntityForward.Right);
                battleEntityView2.transform.localPosition = __instance.GetScenePosByEntityPos(0, j);
            }
            ListPool<BattleEntity>.Recycle(list3);
            string text = null;
            bool flag3 = battleType != BattleType.Invade;
            if (flag3)
            {
                RegionInfo curRegionInfo = SingletonMonoBehaviour<MapModel>.Instance.GetCurRegionInfo();
                text = curRegionInfo.BattleBG;
            }
            bool flag4 = string.IsNullOrEmpty(text);
            if (flag4)
            {
                text = "battle_bg_town0.prefab";
            }
            GameObject gameObject = (GameObject)Singleton<ResourcesManager>.Instance.GetInstance(text, string.Empty);
            bool flag5 = gameObject != null;
            if (flag5)
            {
                TransformContainer mContainer = Traverse.Create(__instance).Field("mContainer").GetValue<TransformContainer>();
                gameObject.transform.SetParent(mContainer["BackgroundRoot"].transform);
                gameObject.transform.localScale = Vector3.one;

                Traverse mBackground = Traverse.Create(__instance).Field("mBackground");
                mBackground.SetValue(new TransformContainer(gameObject.transform));
                //mBackground = new TransformContainer(gameObject.transform);
            }
            SingletonMonoBehaviour<UIManager>.Instance.HideAll();
            SingletonMonoBehaviour<UIManager>.Instance.OpenUnique(PanelEnum.Battle, Array.Empty<object>());
            AudioClip audioClip = SingletonMonoBehaviour<AudioModel>.Instance.GetAudioClip(SingletonMonoBehaviour<AudioModel>.Instance.mGameData.BattleStart);
            BGMLoop = SingletonMonoBehaviour<AudioModel>.Instance.GetAudioClip(SingletonMonoBehaviour<AudioModel>.Instance.mGameData.BattleLoop);
            SingletonMonoBehaviour<AudioManager>.Instance.PauseGameBGM();

            // bgm添加监听
            SingletonMonoBehaviour<AudioManager>.Instance.PlayMusic(audioClip, false, 0, new Action(OnBGMStartEnd_Patch));

            // 返回false表示不执行原方法
            return false;
        }

        public static void OnBGMStartEnd_Patch()
        {
            SingletonMonoBehaviour<AudioManager>.Instance.PlayMusic(BGMLoop, true, 0, null);
        }
    }
    [HarmonyPatch(typeof(BattleManager), "PrepareBattleEntities")]
    public class BattleManager_PrepareBattleEntities_Patch
    {
        [HarmonyPrefix]
        public static bool PrepareBattleEntities_Path(BattleManager __instance, List<int> chrIDList, int team)
        {
            // 如果MOD状态是关闭的话
            if (!FormationMod.enable)
            {
                // 执行原本的方法
                return true;
            }
            if (chrIDList.Count == 5)
            {
                // 启用阵法
                ModifierUtil.enableModifier[team] = true;
            } else
            {
                // 不启用阵法
                ModifierUtil.enableModifier[team] = false;
            }
            for (int i = 0; i < chrIDList.Count; i++)
            {
                int chrID = chrIDList[i];
                Character chrByID = SingletonMonoBehaviour<CharacterModel>.Instance.GetChrByID(chrID);
                bool flag = chrByID != null;
                if (flag)
                {
                    // 先移除已有buff，再添加
                    chrByID.RemoveAllMartialModifier();
                    chrByID.AddAllMartialModifier();
                    // 添加指定阵法buff
                    chrByID.AddMartialModifier(0, ModifierUtil.ModifierNamePY + i);

                    global::CharacterInfo chrInfoByID = SingletonMonoBehaviour<CharacterModel>.Instance.GetChrInfoByID(chrByID.InfoID);
                    bool flag2 = chrInfoByID != null && chrInfoByID.ItemsUseForBattle != null;
                    if (flag2)
                    {
                        foreach (string itemInfoID in chrInfoByID.ItemsUseForBattle)
                        {
                            chrByID.NPCUseItem(itemInfoID);
                        }
                    }
                    // 如果为敌人，则判断难度，添加buff
                    bool flag3 = team == 1 && SingletonMonoBehaviour<SchoolModel>.Instance.mUserData.IronmanMode;
                    if (flag3)
                    {
                        chrByID.AddModifier(SingletonMonoBehaviour<ModifierModel>.Instance.mGameData.MasterBuff, -1);
                    }
                    BattleEntity entity = __instance.GenerateEntity(chrByID, team, i);
                    __instance.AddEntity(entity);
                }
            }

            // 返回false表示不执行原方法
            return false;
        }
    }

    [HarmonyPatch(typeof(BattleManager), "GetScenePosByEntityPos")]
    public class BattleManager_GetScenePosByEntityPos_Patch
    {
        public static Dictionary<int, List<int>> mTeamEntities = null;
        [HarmonyPrefix]
        public static bool GetScenePosByEntityPos_Path(ref Vector3 __result, BattleManager __instance, int team, int idx)
        {
            // 如果MOD状态是关闭的话
            if (!FormationMod.enable)
            {
                // 执行原本的方法
                return true;
            }

            // 获取队伍实体
            if (mTeamEntities == null)
            {
                mTeamEntities = Traverse.Create(__instance).Field("mTeamEntities").GetValue<Dictionary<int, List<int>>>();
            }
            // 获取每个队伍中的人数
            int num = mTeamEntities[team].Count;
            // 5个人齐才能组成阵型
            if (num < 5)
            {
                // 小于5人为默认的散开阵
                __result = GetScenePosByEntityPos_sankaizhen(team, idx);
                // 返回false表示不执行原方法
                return false;
            }
            // 默认位散开阵
            switch (FormationFileLoad.getIndexForName(ModifierUtil.ModifierNamePY))
            {
                case 0:
                    __result = GetScenePosByEntityPos_sankaizhen(team, idx);
                    break;
                case 1:
                    __result = GetScenePosByEntityPos_yizizhen(team, idx);
                    break;
                case 2:
                    __result = GetScenePosByEntityPos_chongfangzhen(team, idx);
                    break;
                default:
                    __result = GetScenePosByEntityPos_sankaizhen(team, idx);
                    break;
            }

            // 返回false表示不执行原方法
            return false;
        }
        /// <summary>
        /// 散开阵
        /// </summary>
        /// <param name="team"></param>
        /// <param name="idx"></param>
        public static Vector3 GetScenePosByEntityPos_sankaizhen(int team, int idx)
        {
            int num = mTeamEntities[team].Count;
            float num2 = 1.5f * (float)(1 + idx / 3);
            bool flag = idx < 3;
            float y;
            if (flag)
            {
                bool flag2 = num > 3;
                if (flag2)
                {
                    num = 3;
                }
                y = 0.75f + 0.75f * (float)num - 1.5f * (float)idx;
            }
            else
            {
                idx -= 3;
                y = 3.5f - (float)idx * 1.5f;
            }
            bool flag3 = team == 0;
            if (flag3)
            {
                num2 *= -1f;
            }
            Vector3 result = new Vector3(num2, y, 0f);
            return result;
        }
        /// <summary>
        /// 一字阵
        /// </summary>
        /// <param name="team"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static Vector3 GetScenePosByEntityPos_yizizhen(int team, int idx)
        {
            // 设置坐标x,y
            float scaleX = 3f;
            float scaleY = 0f;
            // 从上到下排列
            scaleY = 4f - 1f * (float)idx;
            // 队伍编号为0时是玩家
            bool isPlayer = team == 0;
            if (isPlayer)
            {
                // 如果是玩家，x坐标反转
                scaleX *= -1f;
            }
            Vector3 result = new Vector3(scaleX, scaleY, 0f);
            // 返回结果
            return result;
        }
        /// <summary>
        /// 冲方阵
        /// </summary>
        /// <param name="team"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static Vector3 GetScenePosByEntityPos_chongfangzhen(int team, int idx)
        {
            // 设置坐标x,y
            float scaleX = 3f;
            float scaleY = 0f;
            // 从上到下排列
            scaleY = 4f - 1f * (float)idx;
            // 队伍编号为0时是玩家
            bool isPlayer = team == 0;
            if (isPlayer)
            {
                // 如果是玩家，x坐标反转
                scaleX *= -1f;
            }
            // x坐标第（idx = 1）位和第（idx = 3）位后退2个身位
            if (idx == 1 || idx == 3)
            {
                scaleX += 1f;
            }

            Vector3 result = new Vector3(scaleX, scaleY, 0f);
            // 返回结果
            return result;
        }
    }

    [HarmonyPatch(typeof(BattleManager), "Exit")]
    public class BattleManager_Exit_Patch
    {
        [HarmonyPrefix]
        public static bool Exit_Path(BattleManager __instance)
        {
            // 如果MOD状态是关闭的话
            if (!FormationMod.enable)
            {
                // 执行原本的方法
                return true;
            }
            // 退出战斗时还原是否组成阵型的状态
            ModifierUtil.resetEnableStatus();

            __instance.DieEntityList.Clear();
            __instance.BrokenEquipmentMap.Clear();

            Dictionary<int, List<int>> mTeamEntities = Traverse.Create(__instance).Field("mTeamEntities").GetValue<Dictionary<int, List<int>>>();
            for (int i = 0; i < 2; i++)
            {
                mTeamEntities[i].Clear();
            }
            Dictionary<int, BattleEntityView> mEntityViewMap = Traverse.Create(__instance).Field("mEntityViewMap").GetValue<Dictionary<int, BattleEntityView>>();
            foreach (KeyValuePair<int, BattleEntityView> keyValuePair in mEntityViewMap)
            {
                BattleEntityView value = keyValuePair.Value;
                Singleton<ResourcesManager>.Instance.ReleaseInstance(value.mContainer.transform.gameObject);
            }
            mEntityViewMap.Clear();
            __instance.Hide();
            SingletonMonoBehaviour<UIManager>.Instance.CloseUnique(PanelEnum.Battle);
            SingletonMonoBehaviour<AudioManager>.Instance.StopAllMusic();
            SingletonMonoBehaviour<AudioManager>.Instance.PlayGameBGM();
            SingletonMonoBehaviour<UIManager>.Instance.ShowAll();
            TimeModel instance = SingletonMonoBehaviour<TimeModel>.Instance;
            int timeStop = instance.TimeStop;
            instance.TimeStop = timeStop - 1;
            GameTempValue.GameSpeedHotKeyLock--;
            GameTempValue.MapMoveHotKeyLock--;
            GameTempValue.SamllMapKeyLock--;
            GameTempValue.TapKeyLock--;
            GameTempValue.BackpackKeyLock--;
            bool flag = false;

            Dictionary<int, BattleEntity> mEntities = Traverse.Create(__instance).Field("mEntities").GetValue<Dictionary<int, BattleEntity>>();
            foreach (KeyValuePair<int, BattleEntity> keyValuePair2 in mEntities)
            {
                BattleEntity value2 = keyValuePair2.Value;
                bool flag2 = value2.mChr.GetCurrentValue(CharacterPropertyType.HP) == 0;
                if (flag2)
                {
                    value2.mChr.SetStatRawValue(CharacterPropertyType.HP, 1);
                }
                bool flag3 = !value2.mChr.IsPlayer();
                if (flag3)
                {
                    bool flag4 = value2.DroppedWeapon != null;
                    if (flag4)
                    {
                        value2.mChr.EquipItem(value2.DroppedWeapon);
                    }
                    value2.mChr.RemoveAllMartialModifier();
                }
                else
                {
                    bool flag5 = value2.TakenDamageMap.Count > 0;
                    if (flag5)
                    {
                        flag = true;
                    }
                }
                bool flag6 = value2.Team != 0 || value2.mChr.IsPlayer();
                if (flag6)
                {
                    value2.AddInjury();
                }
                __instance.RecycleEntity(value2);
            }
            mEntities.Clear();
            bool flag7 = flag;
            if (flag7)
            {
                SingletonMonoBehaviour<GuideModel>.Instance.TryGuide(0);
            }
            switch (__instance.BattleType)
            {
                case BattleType.Kill:
                case BattleType.Compete:
                    SingletonMonoBehaviour<MapModel>.Instance.BattleEnd(__instance.BattleType, __instance.Result);
                    break;
                case BattleType.Invade:
                    SingletonMonoBehaviour<SchoolModel>.Instance.BattleEnd(__instance.BattleType, __instance.Result);
                    break;
                case BattleType.LunWu:
                    SingletonMonoBehaviour<MartialCompeteModel>.Instance.BattleEnd(__instance.BattleType, __instance.Result);
                    break;
            }
            // 获取背景
            Traverse mBackground = Traverse.Create(__instance).Field("mBackground");
            TransformContainer mBackgroundValue =  mBackground.GetValue<TransformContainer>();
            // 释放资源
            Singleton<ResourcesManager>.Instance.ReleaseInstance(mBackgroundValue.transform.gameObject);
            mBackground.SetValue(null);
            // 返回false表示不执行原方法
            return false;
        }
    }
}
