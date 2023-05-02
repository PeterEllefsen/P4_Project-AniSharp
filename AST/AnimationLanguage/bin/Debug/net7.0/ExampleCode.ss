
int function newFunc(int one, int two){
  int i = 1;
  int j = 2;
  int k = 3;

  for(int i = 1; i <= 10; i += 1){
    int j = 4;
  }
}



timeline {
    Frame 1 : CircleGoVroomInTriangle(); //At frame 1 in the timeline the CircleGoVroomInTriangle sequence is called
      
    Frame 10 : CarDrivingOnScreen(); //At frame 10, start the CarDrivingOnScreen sequence, on top of the CircleGoVroomInTriangle sequence.
  }