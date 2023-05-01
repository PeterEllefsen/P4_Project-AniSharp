
int function newFunc(int one, int two){
  int i = 1;
  i = 5;
  int j = 10;

  return i;
}



timeline {
    Frame 1 : CircleGoVroomInTriangle(); //At frame 1 in the timeline the CircleGoVroomInTriangle sequence is called
      
    Frame 10 : CarDrivingOnScreen(); //At frame 10, start the CarDrivingOnScreen sequence, on top of the CircleGoVroomInTriangle sequence.
  }