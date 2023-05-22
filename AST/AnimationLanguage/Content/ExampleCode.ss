prototypes {
    group function ColorBasedOnNumber(int number);
    group function hej();
    string Rgb(int r, int g, int b);
}


setup [
    sceneWidth = 1080,
    sceneHeight = 720,
    framerate = 24,
    backgroundColor = Rgb(255, 255, 255)
];



seq road() {

road = Polygon(
    point1: (x: 0, y: 250),
    point2: (x: 1080, y: 250),
    point3: (x: 1080, y: 500),
    point4: (x: 0, y: 500),
    color: Rgb(50, 50, 50)
)


road->(EndFrame: 1000);

}

seq house() {
housebase = Polygon(
    point1: (x: 100, y: 300),
    point2: (x: 300, y: 300),
    point3: (x: 300, y: 200),
    point4: (x: 100, y: 200),
    color: Rgb(150, 0, 0)
)

roof = Polygon(
    point1: (x: 100, y: 200),
    point2: (x: 200, y: 100),
    point3: (x: 300, y: 200),
    color: Rgb(255, 0, 0)
)

door = Polygon(
    point1: (x: 150, y: 300),
    point2: (x: 200, y: 300),
    point3: (x: 200, y: 220),
    point4: (x: 150, y: 220),
    color: Rgb(255, 255, 255)
)

window = Polygon(
    point1: (x: 220, y: 250),
    point2: (x: 280, y: 250),
    point3: (x: 280, y: 220),
    point4: (x: 220, y: 220),
    color: Rgb(10, 0, 255)
)

    housebase->(EndFrame: 1000);
    roof->(EndFrame: 1000);
    door->(EndFrame: 1000);
    window->(EndFrame: 1000);
    
}



seq car() {

wheel1 = Circle(center: (x: 220, y: 280), radius: 30);
wheel2 = Circle(center: (x: 320, y: 280), radius: 30);

body = Polygon(
    point1: (x: 200, y: 250),
    point2: (x: 340, y: 250),
    point3: (x: 360, y: 220),
    point4: (x: 330, y: 190),
    point5: (x: 210, y: 190),
    point6: (x: 180, y: 220),
    color: Rgb(55, 125, 155)
); 


body->(EndFrame: 100, y: 100)->(EndFrame: 500, x: 1000);
wheel1->(EndFrame: 100, y: 100)->(EndFrame: 500, x: 1000);
wheel2->(EndFrame: 100, y: 100)->(EndFrame: 500, x: 1000);

}



timeline {
   
    Frame 0 : house();
    Frame 0 : road();
    Frame 0 : car();
  }