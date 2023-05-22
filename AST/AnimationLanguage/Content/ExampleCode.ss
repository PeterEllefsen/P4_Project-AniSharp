prototypes {
    group function ColorBasedOnNumber(int number);
    group function hej();
    string Rgb(int r, int g, int b);
    float function Test2();
    int function bobo(int bb);
    int function bibi(int bb);
}


setup [
    sceneWidth = 1080,
    sceneHeight = 720,
    framerate = 24,
    backgroundColor = Rgb(255, 255, 255)
];


int function bobo(int bb){
    int i = bb;
    return 1;
}

int function bibi(int bb){
    int i = bb;
    return 2;
}

float function Test2(){
    int i = bobo(4);
    i = bibi(3);
    return i;
}






seq car() {

wheel1 = Circle(center: (x: bobo(5), y: 280), radius: 30);
wheel2 = Circle(center: (x: 320, y: 280), radius: 30);

wheel1->(EndFrame: bobo(5), y: 100)->(EndFrame: 500, x: 1000);
wheel2->(EndFrame: 100, y: 100)->(EndFrame: 500, x: 1000);

}



timeline {
    Frame 0 : car();
  }