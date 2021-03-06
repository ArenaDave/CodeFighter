﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Utility;
using CodeFighter.Logic.Animations;

namespace CodeFighter.Logic.Orders
{
    public class WeaponFiredEventArgs : MessageEventArgs
    {
        public int ShooterID { get; set; }
        public int TargetID { get; set; }
        public bool IsHit { get; set; }
        public bool IsCrit { get; set; }
        public string FiringType { get; set; }

        public WeaponFiredEventArgs(int shooterId, int targetId, bool isHit, bool isCrit, string firingType, List<string> messages)
            : base(messages)
        {
            this.ShooterID = shooterId;
            this.TargetID = targetId;
            this.IsHit = isHit;
            this.IsCrit = isCrit;
            this.FiringType = firingType;
        }
    }

    public delegate void WeaponFiredEvent(object sender, WeaponFiredEventArgs e);

    public class FireWeaponOrder : BaseOrder
    {
        #region Events
        public event WeaponFiredEvent OnWeaponFired;
        public event ShipDestroyedEvent OnTargetDestroyed;
        public event MessageEvent OnMessageResult;
        #endregion

        #region Public Properties
        public Ship TargetShip { get; set; }
        public WeaponPart WeaponToFire { get; set; }
        public override int Priority { get { return 0; } }
        #endregion

        #region Public Methods
        internal override void ExecuteOrder()
        {
            List<string> result = new List<string>();
            result.Add(string.Format("Attempting to execute order: {0}", this.ToString()));

            if(WeaponToFire.HasFiredThisRound) // has already fired
            {
                result.Add(string.Format("{0} has already fired this round!", WeaponToFire.Name));
                OnMessageResult?.Invoke(this, new MessageEventArgs(result));
            }
            else if (WeaponToFire.IsDestroyed) // weapon is destroyed
            {
                result.Add(string.Format("{0} is destroyed and cannot be fired!", WeaponToFire.Name));
                OnMessageResult?.Invoke(this, new MessageEventArgs(result));
            }
            else if (!WeaponToFire.IsLoaded) // weapon not reloaded
            {
                result.Add(string.Format("{0} will be reloaded in {1} turns", WeaponToFire.Name, WeaponToFire.Reload()));
                OnMessageResult?.Invoke(this, new MessageEventArgs(result));
            }
            else if (TargetShip.HP.Current <= 0) // target is dead
            {
                result.Add("Target Already Defeated");
                OnMessageResult?.Invoke(this, new MessageEventArgs(result));
            }
            else
            {
                double distanceToTarget = CurrentShip.Position.DistanceTo(TargetShip.Position);
                
                if (distanceToTarget >= WeaponToFire.Range + 1) // target out of range
                {
                    result.Add("Target Out Of Range");
                    OnMessageResult?.Invoke(this, new MessageEventArgs(result));
                }
                else
                {
                    WeaponToFire.Target = TargetShip;
                    AttackResult attackResult = WeaponToFire.Fire();
                    result = result.Concat(attackResult.Messages).ToList<string>();
                    OnWeaponFired?.Invoke(this, new WeaponFiredEventArgs(CurrentShip.ID, TargetShip.ID, attackResult.IsHit, attackResult.IsCrit, WeaponToFire.FiringType, result));
                    if (attackResult.TargetDestroyed)
                    {
                        OnTargetDestroyed?.Invoke(this, new ShipEventArgs(TargetShip.ID, new List<string>() { string.Format("{0} was destroyed by this attack!", TargetShip.ToString()) }));
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Format("Fire {0} at {1}", WeaponToFire.ToString(), TargetShip.ToString());
        }
        #endregion

        #region Constructor
        public FireWeaponOrder(Ship currentShip, WeaponPart weaponToFire, Ship targetShip) : base(currentShip)
        {
            this.WeaponToFire = weaponToFire;
            this.TargetShip = targetShip;
        }

        #endregion
    }
}
