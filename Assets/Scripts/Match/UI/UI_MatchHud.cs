using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JLib.Sim;
using JLib.Utilities;
using JLib.Unity;

namespace Pit
{

    public class UI_MatchHud : PT_MonoBehaviour
    {
        public FloatyTextMgr Floaty;
        public Image HealthBar;



        protected override void OnEnable()
        {
            base.OnEnable();
            //        Events.AddGlobalListener<PawnDamagedEvent>(OnPawnDamaged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            //        Events.RemoveGlobalListener<PawnDamagedEvent>(OnPawnDamaged);
        }



        //void OnPawnDamaged(PawnDamagedEvent ev)
        //{
        //    if (ev.Pawn is PCCharacter)
        //        UpdatePCHud((PCCharacter)ev.Pawn);
        //    else
        //        Floaty.ShowFloatyText(GetPawnFloatyPoint(ev.Pawn.gameObject.transform), ev.Amount.ToString());
        //}



        //void UpdatePCHud(PCCharacter character)
        //{
        //    int hp = (character.CurrentHitPoints < 0) ? 0 : character.CurrentHitPoints;
        //    HealthBar.fillAmount = hp / (float)character.MaxHitPoints;
        //}


        //// -----------------------------------------------------------------------------
        //Vector3 GetPawnFloatyPoint(Transform trans)
        //// -----------------------------------------------------------------------------
        //{
        //    Vector3 v = trans.position;
        //    //### Vector3 camPos = Camera.main.transform.position;

        //    v.y += 2.0f;    //### add in pawn height
        //    return v;
        //}


    }
}