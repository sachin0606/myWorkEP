using System.Xml.Linq;

namespace Util.Common.ExtensionHelper
{
	public interface IXML<T>
	{
		T GetItemFormXML(XContainer element);

		XContainer GetXMLFormItem(T item);
	}
}
