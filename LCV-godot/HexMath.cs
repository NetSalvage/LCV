using Godot;

namespace HexMath {
	struct lineOddQ { //weirdly enough this will be the most straightforward way to do this
		Vector2[] cell; //"start" will be vector2[0], "end" will be vector2[count-1]
	}

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

		public static Vector2 Coords(Vector3 CubeCoords) {
			float q = CubeCoords.x;
			float r = CubeCoords.y + (q - ((int)q&1))/2;
			return(new Vector2 (q,r));
		}		

		public static int Distance(Vector2 start, Vector2 end) {
			Vector3 start3 = CubeCoords(start);
			Vector3 end3 = CubeCoords(end);
			return(Cube.Distance(start3, end3));
		}

		public static Vector2 Neighbor(Vector2 hex, int direction){
			return(Coords(Cube.Neighbor(CubeCoords(hex),direction)));
		}

		public static Vector2[] Line(Vector2 start, Vector2 end, TileMap tileMap) {
			//my logic makes this easiest with oddQ, so vector3 will be the "redirecting" function.
			//FAR FUTURE: rewrite this to more naturally use a hex grid. Will probably need that TileMap supports hex grids.
			Vector2[] lineOut = new Vector2 [(Cube.Distance(CubeCoords(start),CubeCoords(end)))];
			//determine biases, then set prim and sec directions based on that.
			Vector2 diff = end - start;
			//handle potential edge cases

			float angle = Mathf.Atan2(diff.y,diff.x); //this is in radians
			int prim; //most common move direction. Integers match those in "Neighbor" function
			int sec; //alternative move direction.

			if (0 <= angle && angle < 30) { //either SE or NE, NE primary
			}
			else if (30 <= angle && angle < 60) { //either NE or N, NE primary
			}
			else if (60 <= angle && angle < 90) { //either NE or N, N primary
			}

			return(lineOut);
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

		public static Vector3[] Line(Vector3 start, Vector3 end, TileMap tileMap) {
			/*teach yourself bresenham, then implement it:
			https://zvold.blogspot.com/2010/01/bresenhams-line-drawing-algorithm-on_26.html
			*/
			//FAR FUTURE: rewrite this to more naturally use a hex grid. Will probably need that TileMap supports hex grids.
			//Let's bias towards clockwise for "first moves" and "tiebreakers".
			Vector3[] lineOut = new Vector3 [(Cube.Distance((start),(end)))];
			//determine biases, then set prim and sec directions based on that.
			Vector3 diff = end - start;
			
			//check for "perfect" directions
			if (diff.x == 0) { //vertical
				if (diff.y > 0) { //"r" is larger, which means it's going down
					//fill in lineOut with each hex in that vertical line. +R, -S each time.
				} else {
					//fill in lineOut. -R, +S each time.
				}
			}
			else if (diff.x == -diff.y) { //UR or DL
				if (diff.x > 0) { //going "right", so, UR
					//fill in lineOut. +Q, -R each time.
				} else {
					//fill in lineOut. -Q, +R each time.
				}
			}
			else if (diff.x == -diff.z) { //DR or UL
				if (diff.x > 0) { //going "right", so, DR
					//fill in lineOut. +Q, -S each time.
				} else {
					//fill in lineOut. -Q, +S each time.
				}
			}
			else if (diff.y == diff.z) { //L or R
				if (diff.y < 0) { //both are negative, so moving "right"
					//fill in lineOut. Alternate between -R and -S. +Q each time. 
				} else {
					//fill in lineOut. Alternate between +R and +S. -Q each time.
				}
			}
			else { //All other cases go in here.
				//Start by determining the Two Possible Movement Directions.
				int prim; //most common move direction. Integers match those in "Neighbor" function
				int sec; //alternative move direction.
				if (diff.x > 0) { //it goes somewhat R.
					if (diff.y > 0) { //Each move is either DR or D.
						if (-2*diff.y <= diff.z){ //abs(r) is half or more of abs(s), which means the line is equal or more "vertical".
							prim = 3;
							sec = 2;
						} else {
							prim = 2;
							sec = 3;
						}
					} else { //Each move is either UR or U.
						if (-2*diff.y > diff.z){ //abs(r) is more than half of abs(s), which means the line is more "vertical".
							prim = 0;
							sec = 1;
						} else {
							prim = 1;
							sec = 0;
						}
					}
				} else { //it goes somewhat L.
					if (diff.y > 0) { //Each move is either DL or D.
						if (-2*diff.y < diff.z){ //abs(r) is more than half of abs(s), which means the line is more "vertical".
							prim = 3;
							sec = 4;
						} else {
							prim = 4;
							sec = 3;
						}
					} else { //Each move is either UL or U.
						if (-2*diff.y >= diff.z){ //abs(r) is half or more of abs(s), which means the line is equal or more "vertical".
							prim = 0;
							sec = 5;
						} else {
							prim = 5;
							sec = 0;
						}
					}
				}
				//Now we can assemble the line.
				lineOut[0] = start;
				Vector2 startCartesian = tileMap.MapToWorld(OddQ.Coords(start));
				Vector3 nextCell;
				Vector2 nextVertex;
				Vector2 diffCartesian = tileMap.MapToWorld(OddQ.Coords(diff));
				for (int i = 1; i < lineOut.Length-1; i++) {
					nextVertex = startCartesian + (diffCartesian*i/(lineOut.Length));
					//compare prim vs sec, the one closest to nextVertex is the next cell
					//CONTINUE HERE
					//lineOut[i] = nextCell; 
				}
				lineOut[lineOut.Length] = end;
				return lineOut;
			}

			/*if no perfect direction, we do this somewhere between redblob and bresenham:
				-Draw a line in Cartesian space, from the center of the start hex to the center of the end hex.
					More precisely, make a list of points, "line length" long,
					where the start point is in the start hex, and the end point is in the end hex.  
					We'll also be using the tilemap to get Cartesian coordinates for this.
				-For each prospective hex in our line, we decide between two neighbors to move into.
					We can determine which two neighbors to choose between based on
					which "straight line" cases we're between (see above).
				-Repeat until we have all hexes filled in.
			*/

			return(lineOut);


			Vector3[] hexLine = new Vector3[3];
			return hexLine;
		}
	}
}