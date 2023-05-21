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
  circle1 = Circle(center: (x: 327, y: 135), radius: 10, color: Rgb(255,255,0));
  circle2 = Circle(center: (x: 0, y: 0), radius: 20, color: Rgb(0,255,0));
    

  circle1->repeat(5)->(EndFrame: 5, x: 100)->(EndFrame: 10, x: 200)->(EndFrame: 20, x: 300, color: rgb(255, 0, 255));
  circle2->repeat(5)->(EndFrame: 5, x: 100)->(EndFrame: 10, x: 200)->(EndFrame: 20, x: 300, color: rgb(255, 0, 255));
}

seq CarDrivingOnScreen2() {

    //Headlights
  circle1 = Circle(center: (x: 327, y: 135), radius: 10, color: Rgb(255,255,0));
  circle2 = Circle(center: (x: 0, y: 0), radius: 20, color: Rgb(0,255,0));
    

  circle1->repeat(5)->(EndFrame: 5, x: 10)->(EndFrame: 10, x: 20)->(EndFrame: 20, x: 30, color: rgb(255, 0, 25));
  circle2->repeat(5)->(EndFrame: 5, x: 100)->(EndFrame: 10, x: 200)->(EndFrame: 20, x: 300, color: rgb(255, 0, 25));
}


timeline {
    Frame 0 : CarDrivingOnScreen(); 
    Frame 10 : CarDrivingOnScreen2(); 
  }