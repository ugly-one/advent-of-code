package main

import (
	"fmt"
	"os"
)

type Stat struct {
	zeros, ones, twos int
}

func part1(layers_count int, size int, content []byte) {
	stats := make(map[int]*Stat)
	for i := range layers_count {
		stats[i] = &Stat{0, 0, 0}
	}

	index := 0
	for y := range layers_count {
		for range size {
			char := string(content[index])
			a := stats[y]
			if char == "0" {
				a.zeros++
			}
			if char == "1" {
				a.ones++
			}
			if char == "2" {
				a.twos++
			}
			index++
		}
	}

	var min_zeros *Stat
	for y := range layers_count {
		if min_zeros == nil {
			min_zeros = stats[y]
		} else {
			if min_zeros.zeros > stats[y].zeros {
				min_zeros = stats[y]
			}
		}
	}
	fmt.Println(min_zeros.ones * min_zeros.twos)
}

func part2(content []byte, width int, height int, depth int) {

	for y := range height {
		for x := range width {
			for z := range depth {
				pixel := string(content[z*(width*height)+y*width+x])
				if pixel == "1" {
					fmt.Print("#")
					break
				} else if pixel == "0" {
					fmt.Print(" ")
					break
				}
			}
		}
		fmt.Println()
	}
}

func main() {
	width := 25
	height := 6
	size := width * height

	content, _ := os.ReadFile("/home/tgn/Code/advent-of-code-input/2019/day8/input.txt")
	content_length := len(content)
	layers_count := content_length / size
	part1(layers_count, size, content)
	part2(content, width, height, layers_count)
}
