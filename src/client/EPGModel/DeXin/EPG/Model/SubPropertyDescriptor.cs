using System;
using System.ComponentModel;

namespace DeXin.EPG.Model
{
	internal class SubPropertyDescriptor : PropertyDescriptor
	{
		private PropertyDescriptor _parentPD;

		private PropertyDescriptor _subPD;

		public override Type ComponentType
		{
			get
			{
				return _parentPD.ComponentType;
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public override Type PropertyType
		{
			get
			{
				return _subPD.PropertyType;
			}
		}

		public SubPropertyDescriptor(PropertyDescriptor parentPD, PropertyDescriptor subPD, string PDName)
			: base(PDName, null)
		{
			_parentPD = parentPD;
			_subPD = subPD;
		}

		public override bool CanResetValue(object component)
		{
			return false;
		}

		public override object GetValue(object component)
		{
			return _subPD.GetValue(_parentPD.GetValue(component));
		}

		public override void ResetValue(object component)
		{
		}

		public override void SetValue(object component, object value)
		{
			_subPD.SetValue(_parentPD.GetValue(component), value);
			OnValueChanged(component, EventArgs.Empty);
		}

		public override bool ShouldSerializeValue(object component)
		{
			return true;
		}
	}
}
