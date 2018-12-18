using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capacities //身体健康状况能力因子
{
    public CapacityDef Consciousness;  // △ 依赖因素：大脑、疼痛、意志、血液循环、呼吸、血液过滤   △ 影响其他：几乎所有其他能力

    public CapacityDef Sight;  // △ 依赖因素：眼睛、意识   △ 影响其他：影响正前方180°的躲闪、拆招、格挡、精妙

    public CapacityDef Hearing;  // △ 依赖因素：耳朵、意识  △ 影响其他：影响正前方360°的躲闪、拆招、格挡、精妙，与视力之间为取最大值关系

    public CapacityDef Moving;  // △ 依赖因素：下肢、意识   △ 影响其他：影响躲闪、速度

    public CapacityDef Manipulation;  // △ 依赖因素：上肢、意识   △ 影响其他：影响所有用到手的功法的效率

    public CapacityDef Sounding;  // △ 依赖因素：下巴、颈部、意识

    public CapacityDef Eating;  // △ 依赖因素：下巴、颈部、意识

    public CapacityDef Breathing;  // △ 依赖因素：肺、颈部、胸腔

    public CapacityDef BloodFiltration;  // △ 依赖因素：肾脏、肝脏

    public CapacityDef BloodPumping;  // △ 依赖因素：心脏

    public CapacityDef Metabolism;  // △ 依赖因素：胃
}
