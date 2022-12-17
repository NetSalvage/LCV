using Godot;

namespace HexMath {
    public static class OddQ {
	/*	Namespaces are shortcuts, so OddQ is mostly shortcuts.
		All math in HexMath is done in the "Cube" class, but users can input to OddQ.
		OddQ will do the appropriate conversions, run things through Cube,
		and then return the appropriate coordinates in "Odd-Q" hex map format.
		Any explanations or suggestions regarding these functions will be put in the "Cube" class.
	*/
        public static Vector3 CubeCoords(Vector2 offsetCoords ) {
	    	float q = offsetCoords.x;
    		float r = offsetCoords.y - (q - ((int)q&1))/2;
	    	return(new Vector3 (q, r, -q-r));
	    }

		public static Vector2 Coords(Vector3 cubeCoords) {
			float q = cubeCoords.x;
			float r = cubeCoords.y + (q - ((int)q&1))/2;
			return(new Vector2 (q,r));
		}

		public static Vector2[] Coords(Vector3[] cubeCoords) {
			Vector2[] output = new Vector2[cubeCoords.Length];
			for (int i = 0; i < output.Length; i++) {
				output[i] = new Vector2 (cubeCoords[i].x, cubeCoords[i].y + (cubeCoords[i].x - ((int)cubeCoords[i].x&1))/2);
			}
			return(output);
		}

		public static int Distance(Vector2 start, Vector2 end) {
			Vector3 start3 = CubeCoords(start);
			Vector3 end3 = CubeCoords(end);
			return(Cube.Distance(start3, end3));
		}

		public static Vector2 Neighbor(Vector2 hex, int direction){
			return(Coords(Cube.Neighbor(CubeCoords(hex),direction)));
		}

		public static Vector2[] Line(Vector2 start, Vector2 end, MapMgr mgr) {
			return(Coords( Cube.Line( CubeCoords(start),CubeCoords(end), mgr)));
		}
    }

	public static class Cube {
		public static int Distance(Vector3 start, Vector3 end) {
			/*	Using Manhattan distances on a hex grid, this returns the number of hexes that you would have to exit
				in order to enter the "end" hex.
				For the Manhattan distance BETWEEN these two hexes, simply subtract 1 from this result.
			*/
			return ((int) ( (Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y) + Mathf.Abs(end.z - start.z))/2 ) );
		}
		public static Vector3 Neighbor(Vector3 hex, int direction){
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
					GD.Print("ERROR: Nonexistent direction. 0 is \"up\", 1 is \"up-right\"...original hex returned instead.");
					return hex;
			}
		}
		public static Vector3 FirstDiag(Vector3 hex, int direction) {
			//Returns first hex "diagonally" from a given corner
			switch (direction) {
				case 0:
					return new Vector3(hex.x-1, hex.y-1, hex.z+2);
				case 1:
					return new Vector3(hex.x+1, hex.y-2, hex.z+1);
				case 2:
					return new Vector3(hex.x+1, hex.y, hex.z-1);
				case 3:
					return new Vector3(hex.x+2, hex.y-1, hex.z-1);
				case 4:
					return new Vector3(hex.x+1, hex.y+1, hex.z-2);
				case 5:
					return new Vector3(hex.x-2, hex.y+1, hex.z+1);
				default:
					GD.Print("ERROR: Nonexistent direction. 0 is diagonal from top left corner, 1 is diagonal from top right corner..."+
						" Original hex returned instead.");
					return hex;
			}
		}
		public static Vector3[] Line(Vector3 start, Vector3 end, MapMgr mgr) {
			//FAR FUTURE: rewrite this to more naturally use a hex grid. Will probably need that TileMap supports hex grids.
			//Let's bias towards clockwise for "first moves" and "tiebreakers".
			Vector3[] lineOut = new Vector3 [Cube.Distance((start),(end))+1];
			lineOut[0] = start;
			lineOut[lineOut.Length-1] = end;
			//determine biases, then set prim and sec directions based on that.
			Vector3 diff = end - start; //displacement in cube coords

			//Working through all possibilities, clockwise, starting at "directly north":
			int prim = -1; //The more-clockwise movement direction.
			int sec = -1; //The alternative.
			if (diff.x == 0 && diff.y < 0) { //perfect direction: north
				for (int i = 1; i < lineOut.Length-1; i++) {
					lineOut[i] = Cube.Neighbor(lineOut[i-1],0);						
				}
				return lineOut;
			}
			else if (diff.x > 0) { //rightwards
				if (diff.y < 0 && diff.z > 0) { //between "always U" and "always UR"
					prim = 1;
					sec = 0;
				}
				else if (diff.z == 0) { //always UR
					for (int i = 1; i < lineOut.Length-1; i++) {
						lineOut[i] = Cube.Neighbor(lineOut[i-1],1);						
					}
					return lineOut;
				}
				else if (diff.y < 0 && diff.z < 0) { //between "always UR" and "always DR"
					prim = 2;
					sec = 1;
				}
				else if (diff.y == 0) { //always DR
					for (int i = 1; i < lineOut.Length-1; i++) {
						lineOut[i] = Cube.Neighbor(lineOut[i-1],2);						
					}
					return lineOut;
				}
				else if (diff.y > 0 && diff.z < 0) { //between "always DR" and "always D"
					prim = 3;
					sec = 2;
				}
			}
			else if (diff.x == 0 && diff.y > 0) { //always D
				for (int i = 1; i < lineOut.Length-1; i++) {
					lineOut[i] = Cube.Neighbor(lineOut[i-1],3);						
				}
				return lineOut;
			}
			else { //diff.x < 0, which means moving left
				if (diff.y > 0 && diff.z < 0) { //between "always D" and "always DL"
					prim = 4;
					sec = 3;
				}
				else if (diff.z == 0) { //always DL
					for (int i = 1; i < lineOut.Length-1; i++) {
						lineOut[i] = Cube.Neighbor(lineOut[i-1],4);						
					}
					return lineOut;
				}
				else if (diff.y > 0 && diff.z > 0) { //between "always DL" and "always UL"
					prim = 5;
					sec = 4;
				}
				else if (diff.y == 0) { //always UL
					for (int i = 1; i < lineOut.Length-1; i++) {
						lineOut[i] = Cube.Neighbor(lineOut[i-1],5);						
					}
					return lineOut;
				}
				else if (diff.y < 0 && diff.z > 0) { //between "always UL" and "always U"
					prim = 0;
					sec = 5;
				}
			}
			//If we didn't hit a "perfect direction", we now have to assemble the rest of the line.
			Vector2 startCartesian = mgr.thisTileMap.MapToWorld(OddQ.Coords(start));
			Vector2 diffCartesian = mgr.thisTileMap.MapToWorld(OddQ.Coords(diff));
			//i think this is better memory management:
			Vector3 primCube;
			Vector2 primCartesian;
			Vector2 nextVertex;
			float distance;
			for (int i = 1; i < lineOut.Length-1; i++) {
				nextVertex = startCartesian + (diffCartesian*i/(lineOut.Length-1));
				primCube = Neighbor(lineOut[i-1], prim);
				primCartesian = mgr.thisTileMap.MapToWorld(OddQ.Coords(primCube));
				distance = nextVertex.DistanceTo(primCartesian);
				if (distance <= mgr.hexRadius) {
					lineOut[i] = primCube;
				} else {
					lineOut[i] = Cube.Neighbor(lineOut[i-1],sec);
				}
			}
			return lineOut;
		}
	}
}