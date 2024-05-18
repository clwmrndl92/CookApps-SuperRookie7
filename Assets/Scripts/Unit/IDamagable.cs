﻿using System;
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
        public int maxHp => (int)GetFinalStat(_baseHp, _addHp, _addPerHp);
        public int atk => (int)GetFinalStat(_baseAtk, _addAtk, _addPerAtk);
        public int atkRange => (int)GetFinalStat(_baseAtkRange, _addAtkRange, _addPerAtkRange);
        public float atkCool => (int)GetFinalStat(_baseAtkCool, _addAtkCool, _addPerAtkCool);

        // Base Stat, 영구 성장치 적용
        protected int _baseHp;
        protected int _baseAtk;
        protected int _baseAtkRange;
        protected float _baseAtkCool;

        // Additional Stat, 아이템, 버프 등 일시적 성장치
        protected int _addHp;
        protected int _addAtk;
        protected int _addAtkRange;
        protected float _addAtkCool;

        // Additional Percent Stat, 아이템, 버프 등 일시적 성장치 (퍼센트)
        protected int _addPerHp;
        protected int _addPerAtk;
        protected int _addPerAtkRange;
        protected float _addPerAtkCool;

        public Status(StatSettings settings)
        {
            _baseHp = settings.baseHp;
            _baseAtk = settings.baseAtk;
            _baseAtkRange = settings.baseAtkRange;
            _baseAtkCool = settings.baseAtkCool;

            // UI에 업데이트 될 값
            tmpHp = new ReactiveProperty<int>(maxHp);
            tmpHp.Where(value => value < 0 || value > maxHp)
                .Subscribe(_ =>
                {
                    tmpHp.Value = Mathf.Clamp(tmpHp.Value, 0, maxHp);
                });
        }
        protected virtual float GetFinalStat(float baseStat, float addStat, float addPerStat)
        {
            // Final Stat 계산식
            return baseStat + addStat + ((baseStat + addStat) * (addPerStat / 100));
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