using System.Collections.Generic;

using UnityEngine;

namespace Jampacked.ProjectInca
{
	public class MyHelper
	{
		public static GameObject FindFirstParentWithComponent(GameObject a_childObject, System.Type a_componentType)
		{
			Transform nextTransform = a_childObject.transform;
			while (nextTransform.parent != null)
			{
				if (nextTransform.parent.GetComponent(a_componentType))
					return nextTransform.parent.gameObject;

				nextTransform = nextTransform.parent.transform;
			}
			return null; // Could not find a parent with given component
		}

		public static int IndexOf<T>(LinkedList<T> a_list, T a_item)
		{
			var count = 0;
			for (var node = a_list.First; node != null; node = node.Next, count++)
			{
				if (a_item.Equals(node.Value))
					return count;
			}
			return -1;
		}
	}
}
