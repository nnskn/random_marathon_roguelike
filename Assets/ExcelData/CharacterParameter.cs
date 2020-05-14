using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class CharacterParameter : ScriptableObject
{
	public List<ParameterEntity> Parameter; // Replace 'EntityType' to an actual type that is serializable.
}
