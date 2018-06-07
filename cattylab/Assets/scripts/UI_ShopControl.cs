﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShopControl : MonoBehaviour {

	// Use this for initialization
	private UI_GeneralListControl _listCtrl;
	public GameObject _list;
	public List<GameObject> _defaultItems;
	public List<ListItemData> _merchLIDlist = new List<ListItemData>();
	public UI_Control _overallControl;
	IEnumerator Start () {
		_listCtrl = _list.GetComponent<UI_GeneralListControl>();
		foreach(GameObject go in _defaultItems){
			Instantiate(go,Vector3.zero,new Quaternion(), _list.transform);
		}
		yield return new WaitForSeconds(1f);
		RefreshMerchList();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void RefreshMerchList(){
		_merchLIDlist.Clear();
		List<item> items = _overallControl.overallData.OwnedItems;
		foreach(item it in items){
			ListItemData lid = new ListItemData();
			lid.EntityID = it.ent_id;
			lid.EntityType = "item";
			lid.EntityName = _overallControl.CLD.GetItemName(it.ent_id) + GetRarityStars(_overallControl.CLD.GetItemData(it.ent_id).level);
			lid.MiscData = "$"+_overallControl.CLD.GetItemData(it.ent_id).price +" X" + it.count;
			lid.Interable = it.count > 0;
			_merchLIDlist.Add(lid);
		}
		_listCtrl.listItemData = _merchLIDlist;
		_listCtrl.RefreshItems();
	}

	void ItemClicked(int orderInList){
		_overallControl.overallData.changeMoney(_overallControl.CLD.GetItemData(_merchLIDlist[orderInList].EntityID).price);
		_overallControl.overallData.ItemControl(_merchLIDlist[orderInList].EntityID,-1);
		RefreshMerchList();
	}

	private string GetRarityStars(int rarity){
		string output = "<color=#e5c110>";
		for(int i = 0;i<rarity;i++){
			output += "★";
		}
		output+="</color>";
		return output;
	}
}