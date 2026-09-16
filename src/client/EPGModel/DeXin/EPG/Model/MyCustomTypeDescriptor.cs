using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DeXin.EPG.Model
{
	public class MyCustomTypeDescriptor : CustomTypeDescriptor
	{
		public MyCustomTypeDescriptor(ICustomTypeDescriptor parent)
			: base(parent)
		{
		}

		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = base.GetProperties(attributes);
			IList<PropertyDescriptor> list = new List<PropertyDescriptor>();
			for (int i = 0; i < properties.Count; i++)
			{
				PropertyDescriptorCollection childProperties = properties[i].GetChildProperties();
				if (childProperties.Count > 0)
				{
					for (int j = 0; j < childProperties.Count; j++)
					{
						list.Add(new SubPropertyDescriptor(properties[i], childProperties[j], properties[i].Name + "_" + childProperties[j].Name));
					}
				}
			}
			PropertyDescriptor[] array = new PropertyDescriptor[properties.Count + list.Count];
			properties.CopyTo(array, 0);
			list.CopyTo(array, properties.Count);
			return new PropertyDescriptorCollection(array);
		}

		public override PropertyDescriptorCollection GetProperties()
		{
			PropertyDescriptorCollection properties = base.GetProperties();
			IList<PropertyDescriptor> list = new List<PropertyDescriptor>();
			for (int i = 0; i < properties.Count; i++)
			{
				PropertyDescriptorCollection childProperties = properties[i].GetChildProperties();
				if (childProperties.Count > 0)
				{
					for (int j = 0; j < childProperties.Count; j++)
					{
						list.Add(new SubPropertyDescriptor(properties[i], childProperties[j], properties[i].Name + "_" + list[j].Name));
					}
				}
			}
			PropertyDescriptor[] array = new PropertyDescriptor[properties.Count + list.Count];
			properties.CopyTo(array, 0);
			list.CopyTo(array, properties.Count);
			return new PropertyDescriptorCollection(array);
		}
	}
}
