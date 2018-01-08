using UnityEngine;
using System.Collections;

namespace Ganeesyan.Cubism3Compornets
{
	public class LeapmoitonWristLinerLeft : LMWristLiner
	{

		// Use this for initialization
		new void Start()
		{
			isLeft = true;
			base.Start();
		}
	}
}