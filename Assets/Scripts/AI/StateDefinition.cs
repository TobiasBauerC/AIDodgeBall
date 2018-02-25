using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDefinition
{
	public enum StateName
	{
		NullState = -1,
		IdleState
	}

	public StateName stateName { get; set; }
}
