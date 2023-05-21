prototypes {
    group function ColorBasedOnNumber(int number);
    group function hej();
}
 
 
setup [
    sceneWidth = 500,
    sceneHeight = 300,
    framerate = 60,
    backgroundColor = rgb(255, 255, 255)
 
];
 
 
 
int function ColorBasedOnNumber(int number) {
      int purple = 0;
      int red = 4 + (number + ((2 * 2) / 2));
      int green -= 6;
      int blue = number;
 
    while(purple < 10) {
        purple += 1;
    }
 
    if (number < 5) {
       blue = 255-69; 
    }else if((number > 5) or (number < 10)) {
        blue = 50;
    }else if((number > 2) or (number < 10)) {
        blue = 50;
    }
 
      for(int i = 0; i < 10; i++) {
          green += ((number / 2) / i);
 
        if (green > 255) {
            green = 100-69;
        }
    }
    return red;
 
}
 
string function ColorBasedOnText(string text) {
 
    text += "hej jeg er sej" + "hej";
 
    return text;
}
 
 
seq CarDrivingOnScreen() {
 
    //Headlights
  circleHeadlight = Circle(center: (x: 327, y: 135), radius: 10, color: Rgb(255,255,0)); //yellow
 
 
  //Back Wheel
  circle1wheel = Circle(center: (x: 187, y: 170), radius: 25, color: Rgb(0,0,0));
  circle1wheelinner = Circle(center: (x: 187, y: 170), radius: 12.5, color: Rgb(165,165,165));
 
  //Front Wheel
  circle2wheel = Circle(center: (x: 276, y: 157), radius: 25, color: Rgb(0,0,0));
  circle2wheelinner = Circle(center: (x: 276, y: 157), radius: 12.5, color: Rgb(165,165,165));
 
  cirkel1 = Circle(color: red , radius: 50, borderWidth: 3, center: (x: 200, y: 50));

  poly1 = Polygon(color: Rgb(165,165,165), borderWidth: 3, point1: (x: 327, y: 135), point2: (x: 327, y: 135), point3: (x: 327, y: 135));
 
  car[
        circle1wheel, 
        circle1wheelinner, 
        circle2wheel, 
        circle2wheelinner, 
        circleHeadlight
  ];

  poly1->(EndFrame: 5, x: 100)->(EndFrame: 10, x: 200)->(EndFrame: 20, x: 300, color: rgb(255, 0, 255));
 
  //circle2wheel->repeat(5)->(EndFrame: 5, x: 100)->(EndFrame: 10, x: 200)->(EndFrame: 20, x: 300, color: rgb(255, 0, 255));
  //circle1wheelinner->repeat(5)->(EndFrame: 5, x: 100)->(EndFrame: 10, x: 200)->(EndFrame: 20, x: 300, color: rgb(255, 0, 255));
 
 
}
 
 
timeline {
    Frame 1 : CarDrivingOnScreen(); //At frame 10, start the CarDrivingOnScreen sequence, on top of the CircleGoVroomInTriangle sequence.
 
  }