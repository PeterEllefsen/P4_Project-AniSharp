seq CircleGoVroomInTriangle(float f, bool b) {
    cirkel1->repeat(4)->(EndFrame: 50, x: 50, color: Rgb(ColorBasedOnNumber(5)))->(EndFrame: 100, x: 50, y: -50)->(EndFrame: 150, y: -50);
}


timeline {
    Frame 1 : CircleGoVroomInTriangle(); //At frame 1 in the timeline the CircleGoVroomInTriangle sequence is called
      
    Frame 10 : CarDrivingOnScreen(); //At frame 10, start the CarDrivingOnScreen sequence, on top of the CircleGoVroomInTriangle sequence.
  }