using UnityEngine;

namespace MyGrid
{
	public class TileController : MonoBehaviour
	{
		[SerializeField] private TileController up;
		[SerializeField] private TileController down;
		[SerializeField] private TileController right;
		[SerializeField] private TileController left;

		public Vector2 coordinate;

		public TileController GetNeighbour(Direction direction)
		{
			return direction switch
			{
				Direction.Up => up,
				Direction.Down => down,
				Direction.Left => left,
				Direction.Right => right,
				Direction.UpRight => up != null ? up.right : null,
				Direction.UpLeft => up != null ? up.left : null,
				Direction.DownRight => down != null ? down.right : null,
				Direction.DownLeft => down != null ? down.left : null,
				_ => null
			};
		}

		public void PrepareNeighbour(TileController upTile, TileController downTile, TileController leftTile, TileController rightTile)
		{
			up = upTile;
			down = downTile;
			left = leftTile;
			right = rightTile;
		}
	}
}