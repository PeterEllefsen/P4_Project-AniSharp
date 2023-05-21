prototypes {
    group function ColorBasedOnNumber(int number);
    group function hej();
}


setup [
    sceneWidth = 1080,
    sceneHeight = 720,
    framerate = 24,
    backgroundColor = rgb(255, 255, 255)
];



seq house() {
housebase = Polygon(
        point1: (x: 100, y: 300),
        point2: (x: 300, y: 300),
        point3: (x: 300, y: 200),
        point4: (x: 100, y: 200),
    color: rgb(255, 255, 255)
)

    house->(EndFrame: 20, y: 250)->(EndFrame: 1000);

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


body->repeat(5)->(EndFrame: 1000, x: 1000, y: 100);
wheel1->repeat(5)->(EndFrame: 1000, x: 1000, y: 100);
wheel2->repeat(5)->(EndFrame: 1000, x: 1000, y: 100);

}



timeline {
    Frame 10 : car();
    Frame 20 : face();
  }