using System.Linq;
using UnityEngine;

namespace MyGrid.Code
{
	public class SetCenterPosition : MonoBehaviour
	{
		[ContextMenu(nameof(SetCenter))]
		public void SetCenter()
		{
			var childrenArray = GetComponentsInChildren<Transform>();

			var children = childrenArray.ToList();
			children.RemoveAt(0);

			var centerPosition = Vector3.zero;
			foreach (var child in children)
				centerPosition += child.position;

			centerPosition /= children.Count;

			var parent = transform.parent;
			foreach (var item in children)
				item.SetParent(parent);

			transform.position = centerPosition;
			foreach (var item in children)
				item.SetParent(transform);
		}
	}
}