module Day2

open Day2_input

let map = 
    Map [
        (('A', 'X'), 4);
        (('A', 'Y'), 8);
        (('A', 'Z'), 3);
        (('B', 'X'), 1);
        (('B', 'Y'), 5);
        (('B', 'Z'), 9);
        (('C', 'X'), 7);
        (('C', 'Y'), 2);
        (('C', 'Z'), 6);
    ]

// A - Rock
// B - Paper
// C - Scissors

// X - lose
// Y - draw
// Z - win

let map2 = 
    Map [
        (('A', 'X'), 3);
        (('A', 'Y'), 4);
        (('A', 'Z'), 8);
        (('B', 'X'), 1);
        (('B', 'Y'), 5);
        (('B', 'Z'), 9);
        (('C', 'X'), 2);
        (('C', 'Y'), 6);
        (('C', 'Z'), 7);
    ]

let execute state item = 
    let value = map2[item]
    state + value

let run = 
    Array.fold execute 0 input