using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDefinition
{
	public enum StateName
	{
		Null = -1,
		GetBall,
		ThrowBall,
		Hide
	}

	public StateName stateName { get; set; }
}
