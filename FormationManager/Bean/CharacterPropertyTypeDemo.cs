using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormationManager.Bean
{
    using System;
    using System.ComponentModel;

    // Token: 0x020000BE RID: 190
    public enum CharacterPropertyTypeDemo
    {
        // Token: 0x0400067D RID: 1661
        [Description("膂力")]
        LvLi,
        // Token: 0x0400067E RID: 1662
        [Description("身法")]
        ShenFa,
        // Token: 0x0400067F RID: 1663
        [Description("根骨")]
        GenGu,
        // Token: 0x04000680 RID: 1664
        [Description("悟性")]
        WuXing,
        // Token: 0x04000681 RID: 1665
        [Description("外功攻击")]
        Atk,
        // Token: 0x04000682 RID: 1666
        [Description("内功攻击")]
        ForceAtk,
        // Token: 0x04000683 RID: 1667
        [Description("外功防御")]
        Def,
        // Token: 0x04000684 RID: 1668
        [Description("内功防御")]
        ForceDef,
        // Token: 0x04000685 RID: 1669
        [Description("攻速")]
        AtkSpeed,
        // Token: 0x04000686 RID: 1670
        [Description("精准")]
        HitChance,
        // Token: 0x04000687 RID: 1671
        [Description("闪避")]
        DodgeChance,
        // Token: 0x04000688 RID: 1672
        [Description("招架")]
        ParryChance,
        // Token: 0x04000689 RID: 1673
        [Description("御气")]
        ForceResistance,
        // Token: 0x0400068A RID: 1674
        [Description("暴击")]
        CritHitChance,
        // Token: 0x0400068B RID: 1675
        [Description("暴击伤害")]
        CritHitDamage,
        // Token: 0x0400068C RID: 1676
        [Description("暴击抵抗")]
        AntiCriHitChance,
        // Token: 0x0400068D RID: 1677
        [Description("减伤")]
        DamageReduce,
        // Token: 0x0400068E RID: 1678
        [Description("反伤")]
        DamageReturn,
        // Token: 0x0400068F RID: 1679
        [Description("实战")]
        Experience,
        // Token: 0x04000690 RID: 1680
        [Description("拳脚造诣")]
        FistAttainment,
        // Token: 0x04000691 RID: 1681
        [Description("剑法造诣")]
        SwordAttainment,
        // Token: 0x04000692 RID: 1682
        [Description("刀法造诣")]
        BladeAttainment,
        // Token: 0x04000693 RID: 1683
        [Description("棍法造诣")]
        StaffAttainment,
        // Token: 0x04000694 RID: 1684
        [Description("暗器造诣")]
        HiddenAttainment,
        // Token: 0x04000695 RID: 1685
        [Description("气血")]
        HP,
        // Token: 0x04000696 RID: 1686
        [Description("内力")]
        MP,
        // Token: 0x04000697 RID: 1687
        [Description("气血恢复")]
        HPRecover,
        // Token: 0x04000698 RID: 1688
        [Description("内力恢复")]
        MPRecover,
        // Token: 0x04000699 RID: 1689
        [Description("体力")]
        Stamina,
        // Token: 0x0400069A RID: 1690
        [Description("体力恢复")]
        StaminaRecover,
        // Token: 0x0400069B RID: 1691
        [Description("体力消耗")]
        StaminaReduce,
        // Token: 0x0400069C RID: 1692
        [Description("饱食度")]
        Hunger,
        // Token: 0x0400069D RID: 1693
        [Description("饱食度消耗")]
        HungerReduce,
        // Token: 0x0400069E RID: 1694
        [Description("效率")]
        WorkEfficiency = 37,
        // Token: 0x0400069F RID: 1695
        [Description("内功修炼速度")]
        ForceEfficiency,
        // Token: 0x040006A0 RID: 1696
        [Description("轻功修炼速度")]
        DodgeEfficiency,
        // Token: 0x040006A1 RID: 1697
        [Description("拳脚修炼速度")]
        FistEfficiency = 41,
        // Token: 0x040006A2 RID: 1698
        [Description("剑法修炼速度")]
        SwordEfficiency,
        // Token: 0x040006A3 RID: 1699
        [Description("刀法修炼速度")]
        BladeEfficiency,
        // Token: 0x040006A4 RID: 1700
        [Description("棍法修炼速度")]
        StaffEfficiency,
        // Token: 0x040006A5 RID: 1701
        [Description("暗器修炼速度")]
        HiddenEfficiency,
        // Token: 0x040006A6 RID: 1702
        [Description("负重上限")]
        MaxLoad = 47,
        // Token: 0x040006A7 RID: 1703
        [Description("道德")]
        Moral,
        // Token: 0x040006A8 RID: 1704
        [Description("福缘")]
        Lucky,
        // Token: 0x040006A9 RID: 1705
        [Description("破气")]
        ForcePenetration = 53,
        // Token: 0x040006AA RID: 1706
        [Description("交谈兴趣")]
        TalkIntrest = 55,
        // Token: 0x040006AB RID: 1707
        [Description("切磋兴趣")]
        CompeteIntrest,
        // Token: 0x040006AC RID: 1708
        [Description("采矿兴趣")]
        MineIntrest,
        // Token: 0x040006AD RID: 1709
        [Description("种植兴趣")]
        FarmIntrest,
        // Token: 0x040006AE RID: 1710
        [Description("医术兴趣")]
        MedicineIntrest,
        // Token: 0x040006AF RID: 1711
        [Description("锻造兴趣")]
        SmithingIntrest,
        // Token: 0x040006B0 RID: 1712
        [Description("厨艺兴趣")]
        CookingIntrest,
        // Token: 0x040006B1 RID: 1713
        [Description("探索兴趣")]
        ExploreIntrest,
        // Token: 0x040006B2 RID: 1714
        [Description("好感")]
        Favor = 64,
        // Token: 0x040006B3 RID: 1715
        [Description("熟悉度")]
        Familiarity = 66,
        // Token: 0x040006B4 RID: 1716
        [Description("需求声望")]
        RequiredPrestige,
        // Token: 0x040006B5 RID: 1717
        [Description("心情")]
        Mood = 69,
        // Token: 0x040006B6 RID: 1718
        [Description("容貌")]
        Appearance,
        // Token: 0x040006B7 RID: 1719
        [Description("破招")]
        PoZhao = 73,
        // Token: 0x040006B8 RID: 1720
        [Description("巧匠兴趣")]
        HandworkIntrest = 75,
        // Token: 0x040006B9 RID: 1721
        [Description("受到伤害")]
        RecieveDamage,
        // Token: 0x040006BA RID: 1722
        [Description("内功造诣")]
        ForceAttainment,
        // Token: 0x040006BB RID: 1723
        [Description("轻功造诣")]
        DodgeAttainment,
        // Token: 0x040006BC RID: 1724
        [Description("反击")]
        CounterAttack,
        // Token: 0x040006BD RID: 1725
        [Description("刚系伤害")]
        GangDamage,
        // Token: 0x040006BE RID: 1726
        [Description("巧系伤害")]
        QiaoDamage,
        // Token: 0x040006BF RID: 1727
        [Description("柔系伤害")]
        RouDamage,
        // Token: 0x040006C0 RID: 1728
        [Description("拙系伤害")]
        ZhuoDamage,
        // Token: 0x040006C1 RID: 1729
        [Description("快系伤害")]
        KuaiDamage,
        // Token: 0x040006C2 RID: 1730
        [Description("险系伤害")]
        XianDamage,
        // Token: 0x040006C3 RID: 1731
        [Description("化劲")]
        Huajing,
        // Token: 0x040006C4 RID: 1732
        [Description("内力吸取")]
        GainMP,
        // Token: 0x040006C5 RID: 1733
        [Description("气血吸取")]
        GainHP,
        // Token: 0x040006C6 RID: 1734
        [Description("云雨")]
        SexTimes,
        // Token: 0x040006C7 RID: 1735
        [Description("内力消耗")]
        MPCostFactor,
        // Token: 0x040006C8 RID: 1736
        [Description("罪恶")]
        Criminal,
        // Token: 0x040006C9 RID: 1737
        [Description("恢复效果")]
        HealFactor
    }

}
