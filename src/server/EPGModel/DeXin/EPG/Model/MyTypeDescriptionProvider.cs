using System;
using System.ComponentModel;

namespace DeXin.EPG.Model
{
	public class MyTypeDescriptionProvider : TypeDescriptionProvider
	{
		private ICustomTypeDescriptor td;

		public MyTypeDescriptionProvider(TypeDescriptionProvider parent)
			: base(parent)
		{
		}

		public MyTypeDescriptionProvider()
			: this(TypeDescriptor.GetProvider(typeof(object)))
		{
		}

		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			if (td == null)
			{
				td = base.GetTypeDescriptor(objectType, instance);
				td = new MyCustomTypeDescriptor(td);
			}
			return td;
		}
	}
}
