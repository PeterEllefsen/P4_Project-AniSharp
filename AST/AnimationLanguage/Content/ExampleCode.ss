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
 
 
 
  circle1wheelinner = Circle(center: (x: 187, y: 170), radius: 12.5, color: Rgb(165,165,165));
 
  circle2wheelinner = Circle(center: (x: 276, y: 157), radius: 12.5, color: Rgb(165,165,165));
 
  circle1wheelinner->repeat(5)->(EndFrame: 50, x: 100)->(EndFrame: 100, x: 200)->(EndFrame: 200, x: 300, color: rgb(255, 0, 255));
  circle2wheelinner->repeat(5)->(EndFrame: 50, x: 100)->(EndFrame: 100, x: 200)->(EndFrame: 200, x: 300, color: rgb(255, 255, 0));
 
 
}
 
 
timeline {
    Frame 1 : CarDrivingOnScreen(); //At frame 10, start the CarDrivingOnScreen sequence, on top of the CircleGoVroomInTriangle sequence.
 
  }