using System;
using System.Collections.Generic;
using Assets.Scripts.Server;
using UnityEngine;
using Warsmiths.Client;
using Warsmiths.Common;
using Warsmiths.Common.Domain;
using Warsmiths.Common.Domain.CommonCharacterProfile;
using Warsmiths.Common.Domain.Elements;
using Warsmiths.Common.Domain.Enums;
using Warsmiths.Common.Domain.Equipment;
using Warsmiths.Common.ListContainer;
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery

namespace Assets.Scripts.Common
{
    public class PlayerModel : MonoBehaviour
    {
        public static PlayerModel I
        {
            get; private set;
        }

        public Warsmiths.Common.Domain.Player Data
        {
            get; private set;
        }

        //
        //public CommonCharacterProfile CommonProfile;
        public List<ElementOrderItemResult> ElementsOrder = new List<ElementOrderItemResult>();

        public Character Character
        {
            get; private set;
        }

        public readonly List<Lot> LotList = new List<Lot>();

        public readonly Dictionary<string, ElementPriceItemResult> ElementsPrices = new Dictionary<string, ElementPriceItemResult>();

        public Action OnLogged;

        public Action OnUpdateProfile;
        public Action OnUpdateCharacterList;
        public Action OnUpdateInventory;
        public Action OnUpdateEquipment;
        public Action OnUpdateAuction;
        public Action OnUpdateElementsOrder;
        public Action OnRegistretedSuccess;
        public Action OnUpdateElementsPrices;


        // Errors
        public ErrorAction OnRegistretedFailed;
        public ErrorAction OnLoginFailed;

        public void Awake()
        {
            if (I != null)
            {
                Destroy(this);
                return;
            }
            I = this;
           
        }

        private bool _isInit;
        public void Init()
        {
            
        }

        private void SelfOnAddAbilityResult()
        {
            
        }

        private void SelfRegistretedFailed(ErrorCode errorcode)
        {

          
        }

        private void SelfOnLoggedFailed(ErrorCode errorCode)
        {
           
        }

        private void SelfUpdateElementPricesEvent(ElementsPricesContainer data)
        {
           
        }

        private void SelfRegistretedSuccess()
        {
         
        }

        private void SelfOnUpdateInventory(PlayerInventory inventory)
        {
         
        }

        private void SelfOnUpdateCharacterList(CharactersListContainer charsList)
        {
           
        }

        private void SelfOnUpdateEquipment(CharacterEquipment eq)
        {
            
        }

        private void SelfOnUpdateElementsOrder(ElementOrderListContainer orders)
        {
           
        }

        private void SelfOnUpdateAuction(LotsListContainer lots)
        {
           
        }

        private void SelfOnPlayerProfile(Warsmiths.Common.Domain.Player profile)
        {
           
        }

        public void SelfOnLogged()
        {
            
        }

        public void RequestProfile()
        {
            
        }

        internal void SellItem(BaseEquipment item, int price)
        {
            
        }

        public void RequestAuctionLots()
        {
            
        }

        public void BuyItem(Lot lot)
        {
            
        }

        public void SendCreateArmor(BaseReciept recepie)
        {
            
        }


        public void GetElementsOrder()
        {
           
        }

        public void CreateCharacter(string characterName, RaceTypes race, HeroTypes hero, ClassTypes characterClass, StartBonusTypes bonus)
        {
           
        }

        public void SelectCharacter(string charName)
        {
           
        }

        public void RemoveCharacter(string charName)
        {
            
        }

        public void SellElement(string selectedElement, int value)
        {
           
        }

        public void BuyElement(string selectedElement, int value)
        {
           
        }

        public void GetItemBackFromAuction(Lot lotItem)
        {
           
        }

        public void SendSaveReservedFields()
        {
           
        }

       

        public void UnmakeItem(BaseEquipment item)
        {
           
        }

        public void AddAbility(int position, string abiName)
        {  
        }

        public void RemoveAbility(string abiName)
        {
           
        }

       
        public void AddClass(ClassTypes classType)
        {
           
        }
    }
}
