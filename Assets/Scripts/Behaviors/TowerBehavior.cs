﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;
using Newtonsoft.Json;

namespace Didenko.BattleCity.Behaviors
{
    public class TowerBehavior : SpriteLoader, IConfigurable, ISetupable, IModuleDrop
    {
        public int CurrentLvl { get => currentData.lvl; }
        public DataType DataType => DataType.towerDatas;

        [SerializeField]
        private CannonBehavior cannonBehavior;
        [SerializeField]
        private TeamBehavior teamBehavior;
        [SerializeField]
        private SpriteRenderer towerSprite;

        private List<TowerData> towerDatas;
        private TowerData currentData;

        public void CalculateChance(BulletBehavior bulletBehavior)
        {
            var chance = Random.Range(0, 100);
            bool isPenetrated = currentData.penetrationChance >= chance;
            //Debug.Log("isPenetrated" + isPenetrated);
            bulletBehavior.isPenetrated = isPenetrated;
        }

        public void OnReturnToPool()
        {
            cannonBehavior.OnFired -= CalculateChance;
        }

        public void SetConfings(string data)
        {
            towerDatas = JsonConvert.DeserializeObject<List<TowerData>>(data);
        }

        public void Setup(SetupData setupData)
        {
            if (setupData.dataType != DataType)
                return;
            currentData = towerDatas.Find(x => x.lvl == setupData.lvl);

            LoadSpriteForTeam(currentData.spriteName, teamBehavior.Team);
        }

        public void InitSetup()
        {
            var maxLvl = towerDatas.Count;
            int lvl = RandomMobuleSelector.ChooseRandomly(maxLvl, DataType);

            Setup(new SetupData(lvl, DataType, CannonType.None));
        }

        public DropData DropModule()
        {
            var data = new DropData(currentData.spriteName, currentData.lvl, DataType, CannonType.None);
            return data;
        }
    }

    public struct TowerData
    {
        public float penetrationChance;
        public string spriteName;
        public int lvl;
    }
}
