using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace LineUpHeros
{
    public interface IDamagable
    {
        // 프로퍼티
        GameObject gameObjectIDamagable { get; }
        public ReactiveProperty<bool> isDead { get; set; }
        Status status { get; set; }
        // 메소드
        void TakeHeal(int healAmount);
        void TakeDamage(int damage);
        void TakeStun(float stunTime);
    }

    public class Status
    {
        public ReactiveProperty<int> tmpHp;

        // Final Stat Property
        public int maxHp => (int)GetFinalStat((int)EnumStatus.Hp);
        public int atk => (int)GetFinalStat((int)EnumStatus.Atk);
        public int atkRange => (int)GetFinalStat((int)EnumStatus.AtkRange);
        public float atkCool => (int)GetFinalStat((int)EnumStatus.AtkCool);

        // Base Stat, 영구 성장치 적용
        protected float[] baseStatus;

        // Additional Stat, 아이템, 버프 등 일시적 성장치
        protected float[] addStatus;

        // Additional Percent Stat, 아이템, 버프 등 일시적 성장치 (퍼센트)
        protected float[] addPerStatus;

        public Status(StatSettings settings, int statusNum = (int)EnumStatus.Count)
        {
            baseStatus = new float[statusNum];
            addStatus = new float[statusNum];
            addPerStatus = new float[statusNum];
            
            baseStatus[(int)EnumStatus.Hp] = settings.baseHp;
            baseStatus[(int)EnumStatus.Atk] = settings.baseAtk;
            baseStatus[(int)EnumStatus.AtkRange] = settings.baseAtkRange;
            baseStatus[(int)EnumStatus.AtkCool] = settings.baseAtkCool;

            // UI에 업데이트 될 값
            tmpHp = new ReactiveProperty<int>(maxHp);
            tmpHp.Where(value => value < 0 || value > maxHp)
                .Subscribe(_ =>
                {
                    tmpHp.Value = Mathf.Clamp(tmpHp.Value, 0, maxHp);
                });
        }
        protected float GetFinalStat(int statusIndex)
        {
            // Final Stat 계산식
            return baseStatus[statusIndex] + addStatus[statusIndex]  + ((baseStatus[statusIndex]  + addStatus[statusIndex] ) * (addPerStatus[statusIndex]  / 100));
        }

        public void AddStat(int statusIndex, float addValue, bool isPermanent = false)
        {
            // 사실 지금은 baseStatus에 더하든 addStatus에 더하든 똑같다.
            if (isPermanent) baseStatus[statusIndex] += addValue;
            else addStatus[statusIndex] += addValue;
        }
        
        public void SetPerStat(int statusIndex, float perStat)
        {
            addPerStatus[statusIndex] = perStat;
        }
        
        public float GetBaseStat(int statusIndex)
        {
            return baseStatus[statusIndex];
        }
        public float GetAddStat(int statusIndex)
        {
            return addStatus[statusIndex];
        }
        public float GetAddPerStat(int statusIndex)
        {
            return addPerStatus[statusIndex];
        }
        
        
        public enum EnumStatus
        {
            Hp = 0,
            Atk,
            AtkRange,
            AtkCool,
            Count
        }
    }

    // Scriptable Object Installer 세팅 값
    [Serializable]
    public class StatSettings
    {
        public int baseHp;
        public int baseAtk;
        public int baseAtkRange;
        public float baseAtkCool;
    }

}