﻿using Didenko.BattleCity.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Behaviors
{
    public class DroppedModuleBehavior : SpriteLoader, IOnPoolReturn
    {
        //public DropData DropData => dropData;

        [SerializeField]
        private PoolObjBehavior poolObj;
        private DropData dropData;

        private bool isDestroy = false;

        private void Awake()
        {
            //Init(Team.Blue, new DropData("PC3", 3, SetupType.Cannon));
        }

        public void Init(Team team, DropData data)
        {
            this.dropData = data;
            LoadSpriteForTeam(data.spriteName, team);
            StartCoroutine(Rotate());
        }

        public DropData PickUp()
        {
            var data = new DropData(dropData.spriteName, dropData.lvl, dropData.setupType, dropData.cannonType);
            Destroy();
            return data;
        }

        public void Destroy()
        {
            isDestroy = true;
            StopCoroutine(Rotate());
            poolObj.ReturnToPool();
        }

        public void OnReturnToPool()
        {
            dropData = new DropData();
            isDestroy = false;
        }

        private IEnumerator Rotate()
        {
            while (true)
            {
                spriteRenderer.transform.Rotate(0f,0f,1);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Entered trigger: " + collision.name);
            if(collision.gameObject.TryGetComponent(out TankBehavior tankBehavior))
            {
                tankBehavior.OnDropEntered(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out TankBehavior tankBehavior))
            {
                tankBehavior.OnDropExited(this);
            }
        }
    }
}

