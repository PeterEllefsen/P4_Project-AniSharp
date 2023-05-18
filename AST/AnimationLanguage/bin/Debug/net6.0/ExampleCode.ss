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



group function ColorBasedOnNumber(int number) {
      int purple = 1;  
      int red = 4 + (number + ((2 * 2) / 2));
      int green -= 6;
      int blue = number;

    while(purple < 10) {
        purple += 1;
    }

    if (number <= 5) {
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
    return [red, green, blue];

}

string function ColorBasedOnText(string text) {
    text = "hej jeg er sej" + "hej";
    
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

  car[
        carTop1, 
        carTop2, 
        carTop3, 
        circle1wheel, 
        circle1wheelinner, 
        circle2wheel, 
        circle2wheelinner, 
        circleHeadlight
  ];

  car->repeat(5)->(EndFrame: 180, x: 250)->(EndFrame: 180, x: 250)->(EndFrame: 180, x: 250);

}

seq CircleGoVroomInTriangle() {
    cirkel1->repeat()->(EndFrame: 50, x: 50, color: Rgb(ColorBasedOnNumber(frame)))->(EndFrame: 100, x: 50, y: -50)->(EndFrame: 150, y: -50);
}


timeline {
    Frame 1 : CircleGoVroomInTriangle(); //At frame 1 in the timeline the CircleGoVroomInTriangle sequence is called

    Frame 10 : CarDrivingOnScreen(); //At frame 10, start the CarDrivingOnScreen sequence, on top of the CircleGoVroomInTriangle sequence.
  }