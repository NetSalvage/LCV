using Godot;

namespace HexMath {
    public static class OddQ {
	/*	Namespaces are shortcuts, so OddQ is mostly shortcuts.
		All math in HexMath is done in the "cube" class, but users can input to OddQ.
		OddQ will do the appropriate conversions, run things through Cube,
		and then return the appropriate coordinates in "Odd-Q" hex map format.
		Any explanations or suggestions regarding these functions will be put in the "cube" class.
	*/
        public static Vector3 cubeCoords(Vector2 offsetCoords ) {
	    	float q = offsetCoords.x;
    		float r = offsetCoords.y - (q - ((int)q&1))/2;
	    	return(new Vector3 (q, r, -q-r));
	    }

		public static Vector2 coords(Vector3 cubeCoords) {
			float q = cubeCoords.x;
			float r = cubeCoords.y + (q - ((int)q&1))/2;
			return(new Vector2 (q,r));
		}		

		public static int distance(Vector2 start, Vector2 end) {
			Vector3 start3 = cubeCoords(start);
			Vector3 end3 = cubeCoords(end);
			return(cube.distance(start3, end3));
		}

		public static Vector2 neighbor(Vector2 hex, int direction){
			return(coords(cube.neighbor(cubeCoords(hex),direction)));
		}
    }

	public static class cube {
		public static int distance(Vector3 start, Vector3 end) {
			/*	Using Manhattan distances on a hex grid, this returns the number of hexes that you would have to exit
				in order to enter the "end" hex.
				For the Manhattan distance BETWEEN these two hexes, simply subtract 1 from this result.
			*/
			return ((int) ( (Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y) + Mathf.Abs(end.z - start.z))/2 ) );
		}
		public static Vector3 neighbor(Vector3 hex, int direction){
			switch (direction) {
			case 0:
				return new Vector3(hex.x, hex.y-1, hex.z+1);
			case 1:
				return new Vector3(hex.x+1, hex.y-1, hex.z);
			case 2:
				return new Vector3(hex.x+1, hex.y, hex.z-1);
			case 3:
				return new Vector3(hex.x, hex.y+1, hex.z-1);
			case 4:
				return new Vector3(hex.x-1, hex.y+1, hex.z);
			case 5:
				return new Vector3(hex.x-1, hex.y, hex.z+1);
			default:
				GD.Print("ERROR: Nonexistent direction. 0 is north, 1 is northeast...");
				return hex;
			}
		}
	}
}