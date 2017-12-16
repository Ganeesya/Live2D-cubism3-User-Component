using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Core;

namespace Ganeesyan.Cubism3Compornets
{
	public static class CubismUserExtensionMethods
	{
		public static void SetToValue(this CubismParameter parameter, float value, float weight = 1f)
		{
			parameter.Value = parameter.Value + (value - parameter.Value) * weight;
		}

		public static void SetToPersent(this CubismParameter parameter, float per,float weight = 1f)
		{
			float newval = parameter.MinimumValue + (parameter.MaximumValue - parameter.MinimumValue) * per;
			parameter.SetToValue(newval, weight);
		}
		public static void SetToPersent( this CubismParameter[] self, float per, float weight = 1f)
		{
			foreach(var e in self)
			{
				e.SetToPersent(per, weight);
			}
		}

		public static void BlendToValueWeight(this CubismParameter self, CubismParameterBlendMode mode, float value, float weight = 1f)
		{
			if (mode == CubismParameterBlendMode.Additive)
			{
				self.AddToValue(value, weight);


				return;
			}


			if (mode == CubismParameterBlendMode.Multiply)
			{
				self.MultiplyValueBy(value, weight);


				return;
			}


			self.SetToValue(value, weight);
		}

		public static void BlendToValueWeight(this CubismParameter[] self, CubismParameterBlendMode mode, float value, float weight = 1f)
		{
			if (mode == CubismParameterBlendMode.Additive)
			{
				for (var i = 0; i < self.Length; ++i)
				{
					self[i].AddToValue(value, weight);
				}


				return;
			}


			if (mode == CubismParameterBlendMode.Multiply)
			{
				for (var i = 0; i < self.Length; ++i)
				{
					self[i].MultiplyValueBy(value, weight);
				}


				return;
			}


			for (var i = 0; i < self.Length; ++i)
			{
				self[i].SetToValue(value,weight);
			}
		}
	}
}
