using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDefinition
{
	public enum StateName
	{
		Null = -1,
		Search,
		GetBall,
		FindTarget,
		ThrowBall,
		Run
	}

	public StateName stateName { get; set; }
}
