prototypes {
    group function hejsa(int i, int j, float f);
    int function hej();
    int function hejsa2();
    string function ColorBasedOnNumber(int f);
    }


setup [
    sceneWidth = 500,
    sceneHeight = 300,
    framerate = 60,
    backgroundColor = Rgb(255, 255, 255)

];



string function ColorBasedOnNumber(int f){
    int i = 1;
    int a = 1 + 5;
    int j = 1;
    bool b = true;
    i = j;
    if (i < 2){
        i = 1;
    }
    else if(i > 1 and b == true){
        i = 3;
    }

    int hej = 5;
    for(i = 1; i < 3; i--){
        i = 5;
    }
    return "#FFFFFF";
}

seq CarDrivingOnScreen(int i) {

    //Headlights
  circleHeadlight = Circle(center: (x: 327, y: 135), radius: 10, color: Rgb(255,255,0)); //yellow
  
  int i = 1;

  //Back Wheel
  circle1wheel = Circle(center: (x: 187, y: 170), radius: 25, color: Rgb(0,0,0));
  circle1wheelinner = Circle(center: (x: 187, y: 170), radius: 12.5, color: Rgb(165,165,165));
    
  //Front Wheel
  circle2wheel = Circle(center: (x: 276, y: 157), radius: 25, color: Rgb(0,0,0));
  circle2wheelinner = Circle(center: (x: 276, y: 157), radius: 12.5, color: Rgb(165,165,165));  

  cirkel1 = Circle(color: red , radius: 50, borderWidth: 3, center: (x: 200, y: 50));
  
  car[ 
        circle1wheel, 
        circle1wheelinner, 
        circle2wheel, 
        circle2wheelinner, 
        circleHeadlight
  ];
  
  car->repeat(5)->(EndFrame: 180, x: 250)->(EndFrame: 180, x: 250)->(EndFrame: 180, x: 250);

}

seq CircleGoVroomInTriangle(float j) {
    cirkel1 = Circle(color: red , radius: 50, borderWidth: 3, center: (x: 200, y: 50));
    cirkel1->repeat()->(EndFrame: 50, x: 50, color: ColorBasedOnNumber(4))->(EndFrame: 100, x: 50, y: -50)->(EndFrame: 150, y: -50);
}



timeline {
    Frame 1 : CircleGoVroomInTriangle(7.5); //At frame 1 in the timeline the CircleGoVroomInTriangle sequence is called
      
    Frame 10 : CarDrivingOnScreen(4); //At frame 10, start the CarDrivingOnScreen sequence, on top of the CircleGoVroomInTriangle sequence.
  }